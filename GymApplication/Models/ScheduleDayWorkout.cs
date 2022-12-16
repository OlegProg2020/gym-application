namespace GymApplication.Models
{
    public partial class ScheduleDayWorkout
    {
        public ScheduleDayWorkout()
        {
            WorkoutsRegistrations = new HashSet<WorkoutsRegistration>();
        }

        public long Id { get; set; }
        public long WorkoutId { get; set; }
        public string Place { get; set; } = null!;
        public long QuantityOfPlaces { get; set; }
        public string StartTime { get; set; } = null!;
        public string EndTime { get; set; } = null!;
        public long ScheduleDayId { get; set; }

        public virtual ScheduleDay ScheduleDay { get; set; } = null!;
        public virtual Workout Workout { get; set; } = null!;
        public virtual ICollection<WorkoutsRegistration> WorkoutsRegistrations { get; set; }
    }
}
