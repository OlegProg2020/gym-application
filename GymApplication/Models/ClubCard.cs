namespace GymApplication.Models
{
    public partial class ClubCard
    {
        public ClubCard()
        {
            Memberships = new HashSet<Membership>();
        }

        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public long Price { get; set; }
        public string Status { get; set; } = null!;
        public long Duration { get; set; }
        public string TimeLimit { get; set; } = null!;
        public string Image { get; set; } = null!;

        public virtual ICollection<Membership> Memberships { get; set; }
    }
}
