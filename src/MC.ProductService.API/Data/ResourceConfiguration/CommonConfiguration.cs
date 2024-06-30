using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MC.ProductService.API.Data.ResourceConfiguration
{
    /// <summary>
    ///     POCO for a simple db record.
    ///     Ideally, it is in our best interest to know when data changes (and by whom)
    ///     Record status is a concept for something a little more advanced than hidden/visible
    /// </summary>
    public interface IResource
    {
        /// <summary>
        ///     Represents some kind of identifier of the resource creator.
        ///     This could be a user id, or simply an application name/id.
        /// </summary>
        string CreatedBy { get; set; }

        /// <summary>
        ///     Ditto <see cref="CreatedBy" />
        /// </summary>
        string LastUpdatedBy { get; set; }

        /// <summary>
        ///     Gets the created date and time of the resource in UTC.
        /// </summary>
        DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        ///     Gets the last modified date and time of the resource in UTC.
        /// </summary>
        DateTimeOffset LastUpdatedAt { get; set; }
    }

    /// <summary>
    /// Base configuration class for entity types that implement the <see cref="IResource"/> interface. 
    /// This class implements the <see cref="IEntityTypeConfiguration{T}"/> to provide common configurations 
    /// for resource entities, ensuring consistency in how entities are mapped to the database schema.
    /// </summary>
    /// <typeparam name="T">The type of the resource entity. Must be a class, implement <see cref="IResource"/>, 
    /// and have a parameterless constructor.</typeparam>

    public class ResourceConfiguration<T> : IEntityTypeConfiguration<T> where T : class, IResource, new()
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasIndex((T resource) => resource.CreatedAt);
        }
    }

    /// <summary>
    /// Provides PostgreSQL-specific configuration for resource entities of type <typeparamref name="T"/>,
    /// where <typeparamref name="T"/> is a class implementing <see cref="IResource"/>.
    /// This class extends the generic <see cref="ResourceConfiguration{T}"/> to apply database-specific
    /// configurations such as column types and default values.
    /// </summary>
    /// <typeparam name="T">The type of the resource entity. Must be a class, implement <see cref="IResource"/>, and have a parameterless constructor.</typeparam>
    public class PostgresResourceConfiguration<T> : ResourceConfiguration<T> where T : class, IResource, new()
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            builder.Property((T e) => e.CreatedAt).HasColumnType("timestamp with time zone").HasDefaultValueSql("now() at time zone 'utc'");
            builder.Property((T e) => e.LastUpdatedAt).HasColumnType("timestamp with time zone").HasDefaultValueSql("now() at time zone 'utc'");
        }
    }
}
