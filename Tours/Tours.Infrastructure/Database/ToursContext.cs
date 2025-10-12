using Tours.Core.Domain;

using Microsoft.EntityFrameworkCore;

namespace Tours.Infrastructure.Database;

public class ToursContext : DbContext
{

    public DbSet<KeyPoint> KeyPoints { get; set; }
    public DbSet<Tour> Tour { get; set; }
    public DbSet<TourReview> TourReview { get; set; }
    public DbSet<PositionSimulator> Positions { get; set; }
    public DbSet<TourDuration> TourDurations { get; set; }

    public DbSet<TourExecution> TourExecutions { get; set; }



    public ToursContext(DbContextOptions<ToursContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("tours");

        modelBuilder.Entity<TourExecution>()
       .Property(te => te.CompletedKeyPoints)
       .HasColumnType("jsonb");

        modelBuilder.Entity<PositionSimulator>()
                .HasIndex(ps => ps.TouristId)
                .IsUnique();

        ConfigureTourExecution(modelBuilder);


        //modelBuilder.Entity<PositionSimulator>()
        //  .HasIndex(ps => ps.TouristId)
        //  .IsUnique();

        //modelBuilder.Entity<Tour>()
        //  .HasMany(t => t.KeyPoints)
        //  .WithOne()
        //  .HasForeignKey(kp => kp.TourId);

        ConfigureTour(modelBuilder);
   
    }

    private static void ConfigureTourExecution(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TourExecution>()
            .HasOne<Tour>()
            .WithMany()
            .HasForeignKey(s => s.TourId);

    }

    private static void ConfigureTour(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tour>()
           .HasMany(t => t.KeyPoints)
           .WithOne()
           .HasForeignKey(kp => kp.TourId);

        // modelBuilder.Entity<TourReview>()
        //.ToTable("TourReview") 
        //.HasKey(tr => tr.Id);
        modelBuilder.Entity<TourReview>()
         .HasOne<Tour>()              
         .WithMany()                   
         .HasForeignKey(tr => tr.IdTour);

        //ili prebaci u on model creating

        //modelBuilder.Entity<PositionSimulator>()
        // .HasIndex(ps => ps.TouristId)
        // .IsUnique();

        modelBuilder.Entity<Tour>()
        .HasMany(t => t.Durations)
        .WithOne()
        .OnDelete(DeleteBehavior.Cascade);


        //modelBuilder.Entity<TourReview>()
        //.Property(tr => tr.Images)
        //.HasColumnType("text[]");



    }

}