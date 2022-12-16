namespace GymApplication.Models
{
    public partial class ScheduleDay
    {
        public ScheduleDay()
        {
            ScheduleDayWorkouts = new HashSet<ScheduleDayWorkout>();
        }

        public long Id { get; set; }
        public long DayNumber { get; set; }
        public long ScheduleId { get; set; }

        public virtual Schedule Schedule { get; set; } = null!;
        public virtual ICollection<ScheduleDayWorkout> ScheduleDayWorkouts { get; set; }
    }
}
