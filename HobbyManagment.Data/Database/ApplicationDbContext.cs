using HobbyManagment.Data.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace HobbyManagment.Data.Database;

/// <summary>
/// A database context for the Hobby Management application.
/// </summary>
public class ApplicationDbContext : DbContext
{
    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="options">Context options.</param>
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        
    }

    #endregion

    #region DbSets
    
    /// <summary>
    /// Database context for hobbies.
    /// </summary>
    public DbSet<HobbyEntity> Hobbies { get; set; }

    #endregion

    #region Methods

    
    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            optionsBuilder.EnableSensitiveDataLogging(sensitiveDataLoggingEnabled: true);
        }
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new HobbyConfiguration());
    }

    #endregion
}
