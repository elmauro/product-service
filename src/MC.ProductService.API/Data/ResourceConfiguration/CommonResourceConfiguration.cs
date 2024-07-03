using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MC.ProductService.API.Data.ResourceConfiguration
{
    /// <summary>
    /// This is used to keep track of who created or changed a record and when it happened.
    /// </summary>
    public interface IResource
    {
        /// <summary>
        /// Who made this record? Could be a user's name or an app's name.
        /// </summary>
        string CreatedBy { get; set; }

        /// <summary>
        /// Who last changed this record?
        /// </summary>
        string LastUpdatedBy { get; set; }

        /// <summary>
        /// When was this record first made?
        /// </summary>
        DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// When was this record last changed?
        /// </summary>
        DateTimeOffset LastUpdatedAt { get; set; }
    }

    /// <summary>
    /// This sets up the basic rules for saving records in the database that track creation and changes.
    /// </summary>
    /// <typeparam name="T">The type of the record. It needs to be a class that can be created without any arguments.</typeparam>
    public class CommonResourceConfiguration<T> : IEntityTypeConfiguration<T> where T : class, IResource, new()
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            // Makes sure there's a quick way to look up when records were created.
            builder.HasIndex((T resource) => resource.CreatedAt);
        }
    }

    /// <summary>
    /// Sets up specific rules for PostgreSQL to manage when records were created or changed.
    /// </summary>
    /// <typeparam name="T">The type of the record.</typeparam>
    public class PostgresResourceConfiguration<T> : CommonResourceConfiguration<T> where T : class, IResource, new()
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            // Sets up the date and time types for PostgreSQL and makes sure new records get the current time.
            builder.Property((T e) => e.CreatedAt).HasColumnType("timestamp with time zone").HasDefaultValueSql("now() at time zone 'utc'");
            builder.Property((T e) => e.LastUpdatedAt).HasColumnType("timestamp with time zone").HasDefaultValueSql("now() at time zone 'utc'");
        }
    }
}
