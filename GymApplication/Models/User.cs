namespace GymApplication.Models
{
    public partial class User
    {
        public User()
        {
            WorkoutRegistrationUsers = new HashSet<WorkoutRegistrationUser>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string? Patronymic { get; set; }
        public string Birthdate { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public long? MembershipId { get; set; }

        public virtual Membership? Membership { get; set; }
        public virtual ICollection<WorkoutRegistrationUser> WorkoutRegistrationUsers { get; set; }
    }
}
