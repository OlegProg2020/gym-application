namespace GymApplication.Models
{
    public partial class Schedule
    {
        public Schedule()
        {
            ScheduleDays = new HashSet<ScheduleDay>();
        }

        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;

        public virtual ICollection<ScheduleDay> ScheduleDays { get; set; }
    }
}
