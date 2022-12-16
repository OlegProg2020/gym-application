using GymApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GymApplication.Controllers
{
    public class Home : Controller
    {
        private readonly ILogger<Home> _logger;

        public Home(ILogger<Home> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                using (var db = new GymContext())
                {
                    List<Service> services = db.Services.Include(s => s.ServiceImages).ToList();
                    ViewBag.Services = services;

                    List<string> images = new List<string>();
                    foreach (var service in services)
                    {
                        images.Add(service.ServiceImages.First().Path);
                    }
                    ViewBag.Images = images;
                }
            }
            catch
            {
                return RedirectToAction("Error", "Messages");
            }

            return View();
        }

        [HttpGet]
        public IActionResult AboutClub()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Services(long? id, int? AgeCategory)
        {
            try
            {
                if (AgeCategory == null)
                {
                    AgeCategory = 0;
                }

                using (var db = new GymContext())
                {
                    List<Service> services = new List<Service>();

                    if (id != null)
                    {
                        services = db.Services.Where(s => s.Id == id).Include(s => s.ServiceImages).Include(s => s.ServicePrices).ToList();
                    }
                    else
                    {
                        /* (0 - 15) - child, (16+) - adult */
                        if (AgeCategory >= 16)
                        {
                            services = db.Services.Where(s => s.AgeCategory >= AgeCategory).Include(s => s.ServicePrices).Include(s => s.ServiceImages).ToList();
                        }
                        else
                        {
                            services = db.Services.Where(s => (s.AgeCategory >= AgeCategory) && (s.AgeCategory < 16)).Include(s => s.ServicePrices).Include(s => s.ServiceImages).ToList();
                        }
                    }
                    ViewBag.Services = services;
                }
            }
            catch
            {
                return RedirectToAction("Error", "Messages");
            }

            return View();
        }

        [HttpGet]
        public IActionResult ClubCards()
        {
            try
            {
                using (var db = new GymContext())
                {
                    List<ClubCard> cards = db.ClubCards.Where(c => c.Status == "активная").ToList();
                    ViewBag.Cards = cards;
                }
            }
            catch
            {
                return RedirectToAction("Error", "Messages");
            }

            return View();
        }

        [HttpGet]
        public IActionResult OurTeam()
        {
            try
            {
                using (var db = new GymContext())
                {
                    List<Coach> coaches = db.Coaches.ToList();
                    ViewBag.Coaches = coaches;
                }
            }
            catch
            {
                return RedirectToAction("Error", "Messages");
            }

            return View();
        }


        [HttpGet]
        public IActionResult GuestVisit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GuestVisit(string? name, string? phone)
        {
            /* code... */

            return RedirectToAction("Message", "Messages");
        }


        [HttpGet]
        public IActionResult Schedule(long? id, string? date)
        {
            try
            {
                using (var db = new GymContext())
                {
                    List<Schedule> schedules = db.Schedules.ToList();
                    ViewBag.Schedules = schedules;

                    Schedule? schedule = null;

                    if (id == null)
                    {
                        id = schedules.FirstOrDefault()?.Id;
                    }
                    if (id == null)
                    {

                        return View();
                    }
                    ViewBag.Id = id;

                    schedule = db.Schedules.Include(s => s.ScheduleDays.OrderBy(d => d.DayNumber))
                        .ThenInclude(s => s.ScheduleDayWorkouts.OrderBy(w => w.StartTime))
                          .ThenInclude(w => w.Workout).ThenInclude(w => w.Coach).First(s => s.Id == id);

                    List<WorkoutsRegistration> workoutRegistrations = db.WorkoutsRegistrations
                        .Where(s => s.ScheduleDayWorkout.ScheduleDay.ScheduleId == id).ToList();
                    ViewBag.WorkoutsRegistrations = workoutRegistrations;

                    DateTime start = DateTime.Parse(schedule.StartDate);
                    DateTime end = DateTime.Parse(schedule.EndDate);
                    DateTime date1 = date == null ? DateTime.Today : DateTime.Parse(date);

                    if ((date1 >= start) && (date1 <= end))
                    {
                        ViewBag.Schedule = schedule;
                    }
                    else
                    {
                        ViewBag.Schedule = null;
                    }

                    ViewBag.Date = date1.Year + "-" + (date1.Month >= 10 ? date1.Month.ToString() : ("0" + date1.Month))
                        + "-" + (date1.Day >= 10 ? date1.Day.ToString() : ("0" + date1.Day));
                }
            }
            catch
            {
                return RedirectToAction("Error", "Messages");
            }

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "user")]
        public IActionResult SignUpForWorkout(long? scheduleDayWorkoutId, string? date)
        {
            if ((scheduleDayWorkoutId != null) && (date != null))
            {
                try
                {
                    DateTime date1 = DateTime.Parse(date);
                    date = date1.Year + "-" + (date1.Month >= 10 ? date1.Month.ToString() : ("0" + date1.Month))
                        + "-" + (date1.Day >= 10 ? date1.Day.ToString() : ("0" + date1.Day));

                    string? phone = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

                    if (phone != null)
                    {
                        using (var db = new GymContext())
                        {
                            User? user = db.Users.FirstOrDefault(u => u.Phone == phone);

                            if (user != null)
                            {
                                WorkoutsRegistration? registration = db.WorkoutsRegistrations
                                .Include(r => r.WorkoutRegistrationUsers).ThenInclude(r => r.User)
                                .Include(r => r.ScheduleDayWorkout)
                                  .FirstOrDefault(r => (r.ScheduleDayWorkoutId == scheduleDayWorkoutId)
                                    && (r.Date == date));

                                if (registration != null)
                                {
                                    WorkoutRegistrationUser workoutRegistrationUser =
                                        new WorkoutRegistrationUser()
                                        {
                                            User = user
                                        };

                                    if ((registration.QuantityOfPlaces >= 1) &&
                                        !registration.WorkoutRegistrationUsers.Any(u => u.UserId == user.Id))
                                    {
                                        registration.WorkoutRegistrationUsers.Add(workoutRegistrationUser);
                                        registration.QuantityOfPlaces--;
                                        db.WorkoutsRegistrations.Update(registration);
                                    }
                                }
                                else
                                {
                                    ScheduleDayWorkout? scheduleDayWorkout = db.ScheduleDayWorkouts
                                        .FirstOrDefault(s => s.Id == scheduleDayWorkoutId);

                                    registration = new WorkoutsRegistration()
                                    {
                                        ScheduleDayWorkout = scheduleDayWorkout,
                                        Date = date,
                                        QuantityOfPlaces = scheduleDayWorkout.QuantityOfPlaces
                                    };

                                    WorkoutRegistrationUser workoutRegistrationUser =
                                        new WorkoutRegistrationUser()
                                        {
                                            User = user
                                        };

                                    if (registration.QuantityOfPlaces >= 1)
                                    {
                                        registration.WorkoutRegistrationUsers.Add(workoutRegistrationUser);
                                        registration.QuantityOfPlaces--;

                                        db.WorkoutsRegistrations.Add(registration);
                                    }
                                }


                                db.SaveChanges();
                            }
                            else
                            {
                                throw new Exception();
                            }
                        }
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    return RedirectToAction("Error", "Messages");
                }
            }
            else
            {
                return Redirect("/Messages/NotFound?href=/Home/Schedule");
            }

            return Redirect("/Messages/Message?href=/Home/Schedule");
        }
    }
}