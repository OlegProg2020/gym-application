namespace GymApplication.Models
{
    public partial class WorkoutRegistrationUser
    {
        public long Id { get; set; }
        public long WorkoutRegistrationId { get; set; }
        public long UserId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual WorkoutsRegistration WorkoutRegistration { get; set; } = null!;
    }
}
