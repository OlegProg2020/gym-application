namespace GymApplication.Models
{
    public partial class Membership
    {
        public Membership()
        {
            Users = new HashSet<User>();
        }

        public long Id { get; set; }
        public string StartDate { get; set; } = null!;
        public long Duration { get; set; }
        public long? ClubCardId { get; set; }

        public virtual ClubCard? ClubCard { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
