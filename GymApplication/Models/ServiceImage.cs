namespace GymApplication.Models
{
    public partial class ServiceImage
    {
        public long Id { get; set; }
        public string Path { get; set; } = null!;
        public long ServiceId { get; set; }

        public virtual Service Service { get; set; } = null!;
    }
}
