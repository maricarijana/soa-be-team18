using Tours.Core.Domain;

using Microsoft.EntityFrameworkCore;

namespace Tours.Infrastructure.Database;

public class ToursContext : DbContext
{

    public DbSet<KeyPoint> KeyPoints { get; set; }
    public DbSet<Tour> Tour { get; set; }
    public DbSet<TourReview> TourReview { get; set; }
    public DbSet<PositionSimulator> Positions { get; set; }



    public ToursContext(DbContextOptions<ToursContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("tours");


        //modelBuilder.Entity<TourExecution>().Property(item => item.CompletedKeys).HasColumnType("jsonb"); //value object cuva kao json
        //ConfigureTourExecution(modelBuilder);


        //modelBuilder.Entity<PositionSimulator>()
        //  .HasIndex(ps => ps.TouristId)
        //  .IsUnique();

        //modelBuilder.Entity<Tour>()
        //  .HasMany(t => t.KeyPoints)
        //  .WithOne()
        //  .HasForeignKey(kp => kp.TourId);

        ConfigureTour(modelBuilder);
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

        modelBuilder.Entity<PositionSimulator>()
         .HasIndex(ps => ps.TouristId)
         .IsUnique();

        //modelBuilder.Entity<TourReview>()
        //.Property(tr => tr.Images)
        //.HasColumnType("text[]");



    }

}