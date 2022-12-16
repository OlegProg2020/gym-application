namespace GymApplication.Models
{
    public partial class Workout
    {
        public Workout()
        {
            ScheduleDayWorkouts = new HashSet<ScheduleDayWorkout>();
        }

        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public long? CoachId { get; set; }

        public virtual Coach? Coach { get; set; }
        public virtual ICollection<ScheduleDayWorkout> ScheduleDayWorkouts { get; set; }
    }
}
