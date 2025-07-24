using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.Problems;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Explorer.Stakeholders.Infrastructure.Database;

public class StakeholdersContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<AppReview> AppReviews { get; set; }
    public DbSet<Problem> Problem { get; set; }

    public DbSet<Club> Clubs { get; set; }
    public DbSet<ClubMember> ClubMembers { get; set; }
    public DbSet<ClubTour> ClubTours { get; set; }
    public DbSet<ClubInvitation> ClubInvitations { get; set; }
    public DbSet<ClubJoinRequest> ClubJoinRequests { get; set; }
    public DbSet <Notification> Notification { get; set; }

    public StakeholdersContext(DbContextOptions<StakeholdersContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("stakeholders");

        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<Problem>().HasIndex(p => p.Id).IsUnique();

        ConfigureStakeholder(modelBuilder);
        ConfigureAppReview(modelBuilder);
        ConfigureProblems(modelBuilder);


    }

    private static void ConfigureStakeholder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasOne<User>()
            .WithOne()
            .HasForeignKey<Person>(s => s.UserId);
    }

    private static void ConfigureAppReview(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppReview>()
            .HasOne<User>() 
            .WithOne() 
            .HasForeignKey<AppReview>(ar => ar.UserId); 
    }

    private static void ConfigureProblems(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Problem>()
            .Property(problem => problem.Comments).HasColumnType("jsonb");
    }


}