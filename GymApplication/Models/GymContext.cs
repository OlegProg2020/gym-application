using Microsoft.EntityFrameworkCore;

namespace GymApplication.Models
{
    public partial class GymContext : DbContext
    {
        public GymContext()
        {
        }

        public GymContext(DbContextOptions<GymContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ClubCard> ClubCards { get; set; } = null!;
        public virtual DbSet<Coach> Coaches { get; set; } = null!;
        public virtual DbSet<Membership> Memberships { get; set; } = null!;
        public virtual DbSet<Schedule> Schedules { get; set; } = null!;
        public virtual DbSet<ScheduleDay> ScheduleDays { get; set; } = null!;
        public virtual DbSet<ScheduleDayWorkout> ScheduleDayWorkouts { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;
        public virtual DbSet<ServiceImage> ServiceImages { get; set; } = null!;
        public virtual DbSet<ServicePrice> ServicePrices { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Workout> Workouts { get; set; } = null!;
        public virtual DbSet<WorkoutRegistrationUser> WorkoutRegistrationUsers { get; set; } = null!;
        public virtual DbSet<WorkoutsRegistration> WorkoutsRegistrations { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(@"Data Source=\Gym.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClubCard>(entity =>
            {
                entity.ToTable("Club_cards");

                entity.Property(e => e.TimeLimit).HasColumnName("Time_limit");
            });

            modelBuilder.Entity<Membership>(entity =>
            {
                entity.Property(e => e.ClubCardId).HasColumnName("Club_card_Id");

                entity.Property(e => e.StartDate).HasColumnName("Start_date");

                entity.HasOne(d => d.ClubCard)
                    .WithMany(p => p.Memberships)
                    .HasForeignKey(d => d.ClubCardId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasIndex(e => e.Title, "IX_Schedules_Title")
                    .IsUnique();

                entity.Property(e => e.EndDate).HasColumnName("End_date");

                entity.Property(e => e.StartDate).HasColumnName("Start_date");
            });

            modelBuilder.Entity<ScheduleDay>(entity =>
            {
                entity.ToTable("Schedule_days");

                entity.Property(e => e.DayNumber).HasColumnName("Day_number");

                entity.Property(e => e.ScheduleId).HasColumnName("Schedule_Id");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.ScheduleDays)
                    .HasForeignKey(d => d.ScheduleId);
            });

            modelBuilder.Entity<ScheduleDayWorkout>(entity =>
            {
                entity.ToTable("Schedule_day_workouts");

                entity.Property(e => e.EndTime).HasColumnName("End_time");

                entity.Property(e => e.QuantityOfPlaces).HasColumnName("Quantity_of_places");

                entity.Property(e => e.ScheduleDayId).HasColumnName("Schedule_day_id");

                entity.Property(e => e.StartTime).HasColumnName("Start_time");

                entity.Property(e => e.WorkoutId).HasColumnName("Workout_Id");

                entity.HasOne(d => d.ScheduleDay)
                    .WithMany(p => p.ScheduleDayWorkouts)
                    .HasForeignKey(d => d.ScheduleDayId);

                entity.HasOne(d => d.Workout)
                    .WithMany(p => p.ScheduleDayWorkouts)
                    .HasForeignKey(d => d.WorkoutId);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.Property(e => e.AgeCategory).HasColumnName("Age_category");
            });

            modelBuilder.Entity<ServiceImage>(entity =>
            {
                entity.ToTable("Service_images");

                entity.Property(e => e.ServiceId).HasColumnName("Service_Id");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServiceImages)
                    .HasForeignKey(d => d.ServiceId);
            });

            modelBuilder.Entity<ServicePrice>(entity =>
            {
                entity.ToTable("Service_prices");

                entity.Property(e => e.CountOfVisits).HasColumnName("Count_of_visits");

                entity.Property(e => e.ServiceId).HasColumnName("Service_Id");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServicePrices)
                    .HasForeignKey(d => d.ServiceId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Phone, "IX_Users_Phone")
                    .IsUnique();

                entity.Property(e => e.MembershipId).HasColumnName("Membership_Id");

                entity.HasOne(d => d.Membership)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.MembershipId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Workout>(entity =>
            {
                entity.Property(e => e.CoachId).HasColumnName("Coach_Id");

                entity.HasOne(d => d.Coach)
                    .WithMany(p => p.Workouts)
                    .HasForeignKey(d => d.CoachId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<WorkoutRegistrationUser>(entity =>
            {
                entity.ToTable("Workout_registration_users");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.Property(e => e.WorkoutRegistrationId).HasColumnName("Workout_registration_Id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.WorkoutRegistrationUsers)
                    .HasForeignKey(d => d.UserId);

                entity.HasOne(d => d.WorkoutRegistration)
                    .WithMany(p => p.WorkoutRegistrationUsers)
                    .HasForeignKey(d => d.WorkoutRegistrationId);
            });

            modelBuilder.Entity<WorkoutsRegistration>(entity =>
            {
                entity.ToTable("Workouts_registration");

                entity.Property(e => e.QuantityOfPlaces).HasColumnName("Quantity_of_places");

                entity.Property(e => e.ScheduleDayWorkoutId).HasColumnName("Schedule_day_workout_Id");

                entity.HasOne(d => d.ScheduleDayWorkout)
                    .WithMany(p => p.WorkoutsRegistrations)
                    .HasForeignKey(d => d.ScheduleDayWorkoutId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
