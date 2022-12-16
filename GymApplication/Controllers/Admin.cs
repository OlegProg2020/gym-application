using GymApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GymApplication.Controllers
{
    [Authorize(Roles = "admin")]
    public class Admin : Controller
    {
        IWebHostEnvironment _appEnvironment;

        public Admin(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }


        [HttpGet]
        public IActionResult TeamEditor(long? id, string? Search_lastname, int? page)
        {
            const int pageSize = 30;
            if ((page == null) || (page < 0))
            {
                page = 0;
            }
            ViewBag.Page = page;

            List<Coach> coaches;

            try
            {
                using (var db = new GymContext())
                {
                    if (Search_lastname == null)
                    {
                        coaches = db.Coaches.ToList();
                    }
                    else
                    {
                        coaches = db.Coaches.Where(c => c.Lastname == Search_lastname).ToList();
                    }
                }
            }
            catch
            {
                return RedirectToAction("Error", "Messages");
            }

            var paginatedCoaches = coaches.Skip((int)page * pageSize)
                                          .Take(pageSize)
                                          .ToList();
            ViewBag.Coaches = paginatedCoaches;

            if (id != null)
            {
                try
                {
                    using (var db = new GymContext())
                    {
                        Coach? coach = db.Coaches.Find(id);

                        if (coach != null)
                        {
                            ViewBag.Coach = coach;
                        }
                        else
                        {
                            return Redirect("/Messages/NotFound?href=/Admin/TeamEditor");
                        }
                    }
                }
                catch
                {
                    return RedirectToAction("Error", "Messages");
                }
            }
            else
            {
                ViewBag.Coach = null;
            }

            return View();
        }

        [HttpPost]
        public IActionResult TeamEditor(Coach? coach, IFormFile UploadedImage)
        {
            try
            {
                if (coach != null)
                {
                    using (var db = new GymContext())
                    {
                        Coach? coachOld = db.Coaches.Find(coach.Id);

                        coach.Image = "/img/content/" + UploadedImage.FileName;

                        if (coachOld == null)
                        {
                            using (var fileStream = new FileStream(_appEnvironment.WebRootPath
                                                                + coach.Image, FileMode.Create))
                            {
                                UploadedImage.CopyTo(fileStream);
                            }

                            db.Coaches.Add(coach);
                            db.SaveChanges();
                        }
                        else
                        {
                            coachOld.Name = coach.Name;
                            coachOld.Lastname = coach.Lastname;
                            coachOld.Patronymic = coach.Patronymic;
                            coachOld.Specialization = coach.Specialization;
                            coachOld.Description = coach.Description;

                            if (coachOld.Image != coach.Image)
                            {
                                using (var fileStream = new FileStream(_appEnvironment.WebRootPath
                                                                + coach.Image, FileMode.Create))
                                {
                                    UploadedImage.CopyTo(fileStream);
                                }
                                coachOld.Image = coach.Image;
                            }
                            else
                            {

                            }

                            db.Update(coachOld);
                            db.SaveChanges();
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

            return Redirect("/Messages/Message?href=/Admin/TeamEditor");
        }

        [HttpGet]
        public IActionResult TeamEditor_Delete(long? Id)
        {
            try
            {
                if (Id != null)
                {
                    using (var db = new GymContext())
                    {
                        Coach? coach = db.Coaches.Find(Id);

                        if (coach != null)
                        {
                            db.Coaches.Remove(coach);
                            db.SaveChanges();
                        }
                        else
                        {
                            return Redirect("/Messages/NotFound?href=/Admin/TeamEditor");
                        }
                    }
                }
                else
                {
                    return Redirect("/Messages/NotFound?href=/Admin/TeamEditor");
                }
            }
            catch
            {
                return RedirectToAction("Error", "Messages");
            }

            return Redirect("/Messages/Message?href=/Admin/TeamEditor");
        }


        [HttpGet]
        public IActionResult WorkoutEditor(long? id, string? Search_title, int? page)
        {
            const int pageSize = 30;
            if ((page == null) || (page < 0))
            {
                page = 0;
            }
            ViewBag.Page = page;

            List<Workout> workouts;

            try
            {
                using (var db = new GymContext())
                {
                    if (Search_title == null)
                    {
                        workouts = db.Workouts.Include(w => w.Coach).ToList();
                    }
                    else
                    {
                        workouts = db.Workouts.Include(w => w.Coach).Where(w => w.Title == Search_title).ToList();
                    }

                    List<Coach> coaches = db.Coaches.ToList();
                    ViewBag.Coaches = coaches;
                }
            }
            catch
            {
                return RedirectToAction("Error", "Messages");
            }

            var paginatedWorkouts = workouts.Skip((int)page * pageSize)
                                          .Take(pageSize)
                                          .ToList();
            ViewBag.Workouts = paginatedWorkouts;

            if (id != null)
            {
                try
                {
                    using (var db = new GymContext())
                    {
                        Workout? workout = db.Workouts.Find(id);

                        if (workout != null)
                        {
                            ViewBag.Workout = workout;
                        }
                        else
                        {
                            return Redirect("/Messages/NotFound?href=/Admin/WorkoutEditor");
                        }
                    }
                }
                catch
                {
                    return RedirectToAction("Error", "Messages");
                }
            }
            else
            {
                ViewBag.Workout = null;
            }

            return View();
        }

        [HttpPost]
        public IActionResult WorkoutEditor(Workout? workout)
        {
            try
            {
                if (workout != null)
                {
                    using (var db = new GymContext())
                    {
                        Coach? coach = db.Coaches.Find(workout.CoachId);
                        if (coach != null)
                        {
                            workout.Coach = coach;
                        }
                        else
                        {
                            throw new Exception();
                        }

                        Workout? workoutOld = db.Workouts.Find(workout.Id);

                        if (workoutOld == null)
                        {
                            db.Workouts.Add(workout);
                            db.SaveChanges();
                        }
                        else
                        {
                            workoutOld.Title = workout.Title;
                            workoutOld.Coach = coach;

                            db.Update(workoutOld);
                            db.SaveChanges();
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

            return Redirect("/Messages/Message?href=/Admin/WorkoutEditor");
        }

        [HttpGet]
        public IActionResult WorkoutEditor_Delete(long? Id)
        {
            try
            {
                if (Id != null)
                {
                    using (var db = new GymContext())
                    {
                        Workout? workout = db.Workouts.Find(Id);

                        if (workout != null)
                        {
                            db.Workouts.Remove(workout);
                            db.SaveChanges();
                        }
                        else
                        {
                            return Redirect("/Messages/NotFound?href=/Admin/WorkoutEditor");
                        }
                    }
                }
                else
                {
                    return Redirect("/Messages/NotFound?href=/Admin/WorkoutEditor");
                }
            }
            catch
            {
                return RedirectToAction("Error", "Messages");
            }

            return Redirect("/Messages/Message?href=/Admin/WorkoutEditor");
        }


        [HttpGet]
        public IActionResult ClubCardEditor(long? id, string? Search_title, int? page)
        {
            const int pageSize = 30;
            if ((page == null) || (page < 0))
            {
                page = 0;
            }
            ViewBag.Page = page;

            List<ClubCard> clubCards;

            try
            {
                using (var db = new GymContext())
                {
                    if (Search_title == null)
                    {
                        clubCards = db.ClubCards.ToList();
                    }
                    else
                    {
                        clubCards = db.ClubCards.Where(c => c.Title == Search_title).ToList();
                    }
                }
            }
            catch
            {
                return RedirectToAction("Error", "Messages");
            }

            var paginatedClubCards = clubCards.Skip((int)page * pageSize)
                                          .Take(pageSize)
                                          .ToList();
            ViewBag.ClubCards = paginatedClubCards;

            if (id != null)
            {
                try
                {
                    using (var db = new GymContext())
                    {
                        ClubCard? clubCard = db.ClubCards.Find(id);

                        if (clubCard != null)
                        {
                            ViewBag.ClubCard = clubCard;
                        }
                        else
                        {
                            return Redirect("/Messages/NotFound?href=/Admin/ClubCardEditor");
                        }
                    }
                }
                catch
                {
                    return RedirectToAction("Error", "Messages");
                }
            }
            else
            {
                ViewBag.ClubCard = null;
            }

            return View();
        }

        [HttpPost]
        public IActionResult ClubCardEditor(ClubCard? clubCard, IFormFile UploadImage)
        {
            try
            {
                if (clubCard != null)
                {
                    using (var db = new GymContext())
                    {
                        ClubCard? clubCardOld = db.ClubCards.Find(clubCard.Id);

                        clubCard.Image = "/img/content/" + UploadImage.FileName;

                        if (clubCardOld == null)
                        {
                            using (var fileStream = new FileStream(_appEnvironment.WebRootPath
                                                                + clubCard.Image, FileMode.Create))
                            {
                                UploadImage.CopyTo(fileStream);
                            }

                            db.ClubCards.Add(clubCard);
                            db.SaveChanges();
                        }
                        else
                        {
                            clubCardOld.Title = clubCard.Title;
                            clubCardOld.Description = clubCard.Description;
                            clubCardOld.Price = clubCard.Price;
                            clubCardOld.Status = clubCard.Status;
                            clubCardOld.Duration = clubCard.Duration;
                            clubCardOld.TimeLimit = clubCard.TimeLimit;

                            if (clubCardOld.Image != clubCard.Image)
                            {
                                using (var fileStream = new FileStream(_appEnvironment.WebRootPath
                                                                + clubCard.Image, FileMode.Create))
                                {
                                    UploadImage.CopyTo(fileStream);
                                }
                                clubCardOld.Image = clubCard.Image;
                            }
                            else
                            {

                            }

                            db.Update(clubCardOld);
                            db.SaveChanges();
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

            return Redirect("/Messages/Message?href=/Admin/ClubCardEditor");
        }

        [HttpGet]
        public IActionResult ClubCardEditor_Delete(long? Id)
        {
            try
            {
                if (Id != null)
                {
                    using (var db = new GymContext())
                    {
                        ClubCard? clubCard = db.ClubCards.Find(Id);

                        if (clubCard != null)
                        {
                            db.ClubCards.Remove(clubCard);
                            db.SaveChanges();
                        }
                        else
                        {
                            return Redirect("/Messages/NotFound?href=/Admin/ClubCardEditor");
                        }
                    }
                }
                else
                {
                    return Redirect("/Messages/NotFound?href=/Admin/ClubCardEditor");
                }
            }
            catch
            {
                return RedirectToAction("Error", "Messages");
            }

            return Redirect("/Messages/Message?href=/Admin/ClubCardEditor");
        }


        [HttpGet]
        public IActionResult AccountEditor(long? id, string? Search_lastname, int? page)
        {
            const int pageSize = 30;
            if ((page == null) || (page < 0))
            {
                page = 0;
            }
            ViewBag.Page = page;

            List<User> users;

            try
            {
                using (var db = new GymContext())
                {
                    if (Search_lastname == null)
                    {
                        users = db.Users.Include(u => u.Membership).ToList();
                    }
                    else
                    {
                        users = db.Users.Include(u => u.Membership).Where(u => u.Lastname == Search_lastname).ToList();
                    }

                    List<ClubCard> clubCards = db.ClubCards.ToList();
                    ViewBag.ClubCards = clubCards;
                }
            }
            catch
            {
                return RedirectToAction("Error", "Messages");
            }

            var paginatedUsers = users.Skip((int)page * pageSize)
                                          .Take(pageSize)
                                          .ToList();
            ViewBag.Users = paginatedUsers;

            if (id != null)
            {
                try
                {
                    using (var db = new GymContext())
                    {
                        User? user = db.Users.Include(u => u.Membership).FirstOrDefault(u => u.Id == id);

                        if (user != null)
                        {
                            ViewBag.User = user;

                            Membership? membership = user.Membership;
                            ViewBag.Membership = membership;
                        }
                        else
                        {
                            return Redirect("/Messages/NotFound?href=/Admin/AccountEditor");
                        }
                    }
                }
                catch
                {
                    return RedirectToAction("Error", "Messages");
                }
            }
            else
            {
                ViewBag.User = null;
            }

            return View();
        }

        [HttpPost]
        public IActionResult AccountEditor(User? user, Membership? membership)
        {
            try
            {
                if (user != null)
                {
                    using (var db = new GymContext())
                    {
                        Membership? newMembership = null;
                        if (membership != null)
                        {
                            newMembership = new Membership();
                            newMembership.StartDate = membership.StartDate;
                            newMembership.Duration = membership.Duration;
                            newMembership.ClubCard = db.ClubCards.Find(membership.ClubCardId);

                            db.Memberships.Add(newMembership);
                        }
                        else
                        { }

                        User? userOld = db.Users.Include(u => u.Membership).FirstOrDefault(u => u.Id == user.Id);

                        if (userOld == null)
                        {
                            user.Membership = newMembership;

                            db.Users.Add(user);
                            db.SaveChanges();
                        }
                        else
                        {
                            userOld.Name = user.Name;
                            userOld.Lastname = user.Lastname;
                            userOld.Patronymic = user.Patronymic;
                            userOld.Birthdate = user.Birthdate;
                            userOld.Phone = user.Phone;
                            userOld.Password = user.Password;
                            userOld.Role = user.Role;

                            if (userOld.Membership != null)
                            {
                                Membership? oldMembership = db.Memberships.Find(userOld.Membership.Id);
                                if (oldMembership != null)
                                {
                                    db.Memberships.Remove(oldMembership);
                                }
                                else
                                {
                                    throw new Exception();
                                }
                            }
                            else
                            { }

                            userOld.Membership = newMembership;

                            db.Update(userOld);
                            db.SaveChanges();
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

            return Redirect("/Messages/Message?href=/Admin/AccountEditor");
        }

        [HttpGet]
        public IActionResult AccountEditor_Delete(long? Id)
        {
            try
            {
                if (Id != null)
                {
                    using (var db = new GymContext())
                    {
                        User? user = db.Users.Find(Id);

                        if (user != null)
                        {
                            Membership? membership = db.Memberships.Find(user.MembershipId);
                            if (membership != null)
                            {
                                db.Memberships.Remove(membership);
                            }
                            else
                            { }

                            db.Users.Remove(user);
                            db.SaveChanges();
                        }
                        else
                        {
                            return Redirect("/Messages/NotFound?href=/Admin/AccountEditor");
                        }
                    }
                }
                else
                {
                    return Redirect("/Messages/NotFound?href=/Admin/AccountEditor");
                }
            }
            catch
            {
                return RedirectToAction("Error", "Messages");
            }

            return Redirect("/Messages/Message?href=/Admin/AccountEditor");
        }


        [HttpGet]
        public IActionResult ServiceEditor(long? id, string? Search_title, int? page)
        {
            const int pageSize = 30;
            if ((page == null) || (page < 0))
            {
                page = 0;
            }
            ViewBag.Page = page;

            List<Service> services;

            try
            {
                using (var db = new GymContext())
                {
                    if (Search_title == null)
                    {
                        services = db.Services.ToList();
                    }
                    else
                    {
                        services = db.Services.Where(s => s.Title == Search_title).ToList();
                    }
                }
            }
            catch
            {
                return RedirectToAction("Error", "Messages");
            }

            var paginatedServices = services.Skip((int)page * pageSize)
                                          .Take(pageSize)
                                          .ToList();
            ViewBag.Services = paginatedServices;

            if (id != null)
            {
                try
                {
                    using (var db = new GymContext())
                    {
                        Service? service = db.Services.Include(s => s.ServicePrices).FirstOrDefault(s => s.Id == id);

                        if (service != null)
                        {
                            ViewBag.Service = service;

                            ViewBag.ServicePrices = service.ServicePrices ?? new List<ServicePrice>();
                        }
                        else
                        {
                            return Redirect("/Messages/NotFound?href=/Admin/ServiceEditor");
                        }
                    }
                }
                catch
                {
                    return RedirectToAction("Error", "Messages");
                }
            }
            else
            {
                ViewBag.Service = null;
                ViewBag.ServicePrices = new List<ServicePrice>();
            }

            return View();
        }

        [HttpPost]
        public IActionResult ServiceEditor(Service? service, List<int>? Prices, List<int>? CountsOfVisits, IFormFileCollection UploadedImages)
        {
            try
            {
                if (service != null)
                {
                    using (var db = new GymContext())
                    {
                        List<ServicePrice> servicePrices = new List<ServicePrice>();
                        if ((Prices != null) && (CountsOfVisits != null))
                        {
                            for (int i = 0; (i < Prices.Count) && (i < CountsOfVisits.Count); i++)
                            {
                                ServicePrice servicePrice = new ServicePrice();
                                servicePrice.Price = Prices[i];
                                servicePrice.CountOfVisits = CountsOfVisits[i];

                                servicePrices.Add(servicePrice);
                            }
                        }
                        else
                        {

                        }
                        db.ServicePrices.AddRange(servicePrices);

                        List<ServiceImage> serviceImages = new List<ServiceImage>();
                        foreach (var image in UploadedImages)
                        {
                            ServiceImage serviceImage = new ServiceImage();
                            serviceImage.Path = "/img/content/" + image.FileName;

                            using (var fileStream = new FileStream(_appEnvironment.WebRootPath
                                                                + serviceImage.Path, FileMode.Create))
                            {
                                image.CopyTo(fileStream);
                            }

                            serviceImages.Add(serviceImage);

                            db.ServiceImages.Add(serviceImage);
                        }

                        Service? serviceOld = db.Services.Include(s => s.ServicePrices).Include(s => s.ServiceImages)
                                                .FirstOrDefault(s => s.Id == service.Id);
                        if (serviceOld == null)
                        {
                            service.ServicePrices = servicePrices;
                            service.ServiceImages = serviceImages;

                            db.Services.Add(service);
                            db.SaveChanges();
                        }
                        else
                        {
                            serviceOld.Title = service.Title;
                            serviceOld.Description = service.Description;
                            serviceOld.AgeCategory = service.AgeCategory;

                            db.ServicePrices.RemoveRange(serviceOld.ServicePrices);
                            db.ServiceImages.RemoveRange(serviceOld.ServiceImages);

                            serviceOld.ServicePrices = servicePrices;
                            serviceOld.ServiceImages = serviceImages;

                            db.Services.Update(serviceOld);
                            db.SaveChanges();
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

            return Redirect("/Messages/Message?href=/Admin/ServiceEditor");
        }

        [HttpGet]
        public IActionResult ServiceEditor_Delete(long? Id)
        {
            try
            {
                if (Id != null)
                {
                    using (var db = new GymContext())
                    {
                        Service? service = db.Services.Find(Id);

                        if (service != null)
                        {
                            db.Services.Remove(service);
                            db.SaveChanges();
                        }
                        else
                        {
                            return Redirect("/Messages/NotFound?href=/Admin/ServiceEditor");
                        }
                    }
                }
                else
                {
                    return Redirect("/Messages/NotFound?href=/Admin/ServiceEditor");
                }
            }
            catch
            {
                return RedirectToAction("Error", "Messages");
            }

            return Redirect("/Messages/Message?href=/Admin/ServiceEditor");
        }


        [HttpGet]
        public IActionResult ScheduleEditor(long? id, string? Search_title, int? page)
        {
            const int pageSize = 30;
            if ((page == null) || (page < 0))
            {
                page = 0;
            }
            ViewBag.Page = page;

            List<Schedule> schedules;

            try
            {
                using (var db = new GymContext())
                {
                    if (Search_title == null)
                    {
                        schedules = db.Schedules.ToList();
                    }
                    else
                    {
                        schedules = db.Schedules.Where(s => s.Title == Search_title).ToList();
                    }

                    List<Workout> workouts = db.Workouts.Include(w => w.Coach).ToList();
                    ViewBag.Workouts = workouts;
                }
            }
            catch
            {
                return RedirectToAction("Error", "Messages");
            }

            var paginatedSchedules = schedules.Skip((int)page * pageSize)
                                          .Take(pageSize)
                                          .ToList();
            ViewBag.Schedules = paginatedSchedules;

            if (id != null)
            {
                try
                {
                    using (var db = new GymContext())
                    {
                        Schedule? schedule = db.Schedules.Include(s => s.ScheduleDays.OrderBy(s => s.DayNumber))
                                               .ThenInclude(d => d.ScheduleDayWorkouts.OrderBy(w => w.StartTime))
                                                 .FirstOrDefault(s => s.Id == id);

                        if (schedule != null)
                        {
                            ViewBag.Schedule = schedule;
                            ViewBag.ScheduleDays = schedule.ScheduleDays.ToList();

                            List<ScheduleDayWorkout> scheduleDayWorkouts = new List<ScheduleDayWorkout>();
                            foreach (var scheduleDay in schedule.ScheduleDays)
                            {
                                scheduleDayWorkouts.AddRange(scheduleDay.ScheduleDayWorkouts);
                            }
                            ViewBag.ScheduleDayWorkouts = scheduleDayWorkouts;
                        }
                        else
                        {
                            return Redirect("/Messages/NotFound?href=/Admin/ScheduleEditor");
                        }
                    }
                }
                catch
                {
                    return RedirectToAction("Error", "Messages");
                }
            }
            else
            {
                ViewBag.Schedule = null;
            }

            return View();
        }

        [HttpPost]
        public IActionResult ScheduleEditor(Schedule? schedule)
        {
            try
            {
                if (schedule != null)
                {
                    using (var db = new GymContext())
                    {
                        db.Schedules.Add(schedule);
                        db.SaveChanges();
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

            return Redirect("/Messages/Message?href=/Admin/ScheduleEditor");
        }

        [HttpGet]
        public IActionResult ScheduleEditor_Delete(long? scheduleId)
        {
            try
            {
                if (scheduleId != null)
                {
                    using (var db = new GymContext())
                    {
                        Schedule? schedule = db.Schedules.Find(scheduleId);

                        if (schedule != null)
                        {
                            db.Schedules.Remove(schedule);
                            db.SaveChanges();
                        }
                        else
                        {
                            return Redirect("/Messages/NotFound?href=/Admin/ScheduleEditor");
                        }
                    }
                }
                else
                {
                    return Redirect("/Messages/NotFound?href=/Admin/ScheduleEditor");
                }
            }
            catch
            {
                return RedirectToAction("Error", "Messages");
            }

            return Redirect("/Messages/Message?href=/Admin/ScheduleEditor");
        }


        [HttpGet]
        public IActionResult WorkoutRegistrationLists(long? id, string? date)
        {

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
        }

        [HttpGet]
        public IActionResult WorkoutRegistrationEditor(long? scheduleDayWorkoutId, string? date)
        {
            if ((scheduleDayWorkoutId != null) && (date != null))
            {
                try
                {
                    DateTime date1 = DateTime.Parse(date);
                    date = date1.Year + "-" + (date1.Month >= 10 ? date1.Month.ToString() : ("0" + date1.Month))
                        + "-" + (date1.Day >= 10 ? date1.Day.ToString() : ("0" + date1.Day));

                    using (var db = new GymContext())
                    {
                        WorkoutsRegistration? registration = db.WorkoutsRegistrations
                            .Include(r => r.WorkoutRegistrationUsers).ThenInclude(r => r.User)
                              .FirstOrDefault(r => (r.ScheduleDayWorkoutId == scheduleDayWorkoutId)
                                && (r.Date == date));

                        if (registration != null)
                        {
                            ViewBag.Registration = registration;
                        }
                        else
                        {
                            return Redirect("/Messages/NotFound?href=/Admin/WorkoutRegistrationLists");
                        }
                    }
                }
                catch
                {
                    return RedirectToAction("Error", "Messages");
                }
            }
            else
            {
                return Redirect("/Messages/NotFound?href=/Admin/WorkoutRegistrationLists");
            }

            return View();
        }

        [HttpGet]
        public IActionResult WorkoutRegistrationEditor_Delete(long? userId, long? workoutRegistrationId)
        {
            if ((userId != null) && (workoutRegistrationId != null))
            {
                try
                {
                    using (var db = new GymContext())
                    {
                        WorkoutsRegistration? workoutRegistration = db.WorkoutsRegistrations
                            .Include(r => r.WorkoutRegistrationUsers).FirstOrDefault(r => r.Id == workoutRegistrationId);
                        if (workoutRegistration != null)
                        {
                            WorkoutRegistrationUser? record = workoutRegistration.WorkoutRegistrationUsers
                                .FirstOrDefault(r => r.UserId == userId);
                            if (record != null)
                            {
                                db.WorkoutRegistrationUsers.Remove(record);
                                workoutRegistration.QuantityOfPlaces++;

                                db.WorkoutsRegistrations.Update(workoutRegistration);
                                db.SaveChanges();
                            }
                            else
                            {
                                throw new Exception();
                            }
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                }
                catch
                {
                    return RedirectToAction("Error", "Messages");
                }
            }
            else
            {
                return Redirect("/Messages/NotFound?href=/Admin/WorkoutRegistrationLists");
            }

            return Redirect("/Messages/Message?href=/Admin/WorkoutRegistrationLists");
        }
    }
}
