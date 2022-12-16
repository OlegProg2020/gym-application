namespace GymApplication.Models
{
    public partial class Service
    {
        public Service()
        {
            ServiceImages = new HashSet<ServiceImage>();
            ServicePrices = new HashSet<ServicePrice>();
        }

        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public long AgeCategory { get; set; }

        public virtual ICollection<ServiceImage> ServiceImages { get; set; }
        public virtual ICollection<ServicePrice> ServicePrices { get; set; }
    }
}
