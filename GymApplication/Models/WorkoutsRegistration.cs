namespace GymApplication.Models
{
    public partial class WorkoutsRegistration
    {
        public WorkoutsRegistration()
        {
            WorkoutRegistrationUsers = new HashSet<WorkoutRegistrationUser>();
        }

        public long Id { get; set; }
        public long ScheduleDayWorkoutId { get; set; }
        public string Date { get; set; } = null!;
        public long QuantityOfPlaces { get; set; }

        public virtual ScheduleDayWorkout ScheduleDayWorkout { get; set; } = null!;
        public virtual ICollection<WorkoutRegistrationUser> WorkoutRegistrationUsers { get; set; }
    }
}
