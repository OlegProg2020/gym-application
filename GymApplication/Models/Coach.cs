namespace GymApplication.Models
{
    public partial class Coach
    {
        public Coach()
        {
            Workouts = new HashSet<Workout>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string? Patronymic { get; set; }
        public string Specialization { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Image { get; set; } = null!;

        public virtual ICollection<Workout> Workouts { get; set; }
    }
}
