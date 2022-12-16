namespace GymApplication.Models
{
    public partial class ServicePrice
    {
        public long Id { get; set; }
        public long Price { get; set; }
        public long CountOfVisits { get; set; }
        public long ServiceId { get; set; }

        public virtual Service Service { get; set; } = null!;
    }
}
