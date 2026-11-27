using Coffee.Journal.Domain.Contracts;
using Coffee.Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Coffee.Journal.Infrastructure.Database.Contexts;

internal sealed class CoffeeJournalDbContext : DbContext
{
    /// <summary>Same limit as the email in a profile.</summary>
    private const int AuditUserMaxLength = 320;

    private readonly IAuditUser _auditUser;

    public CoffeeJournalDbContext(DbContextOptions<CoffeeJournalDbContext> options, IAuditUser auditUser)
        : base(options)
    {
        _auditUser = auditUser;
    }

    public DbSet<Profile> Profiles { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<ProfileRole> ProfileRoles { get; set; }

    public DbSet<CoffeeRoaster> CoffeeRoasters { get; set; }

    public DbSet<CoffeeRoasterImage> CoffeeRoasterImages { get; set; }

    public DbSet<CoffeeRoasterReview> CoffeeRoasterReviews { get; set; }

    public DbSet<CoffeeRoasterClassification> CoffeeRoasterClassifications { get; set; }

    public DbSet<CoffeeShop> CoffeeShops { get; set; }

    public DbSet<CoffeeShopImage> CoffeeShopImages { get; set; }

    public DbSet<CoffeeShopRoaster> CoffeeShopRoasters { get; set; }

    public DbSet<CoffeeShopBrewingMethod> CoffeeShopBrewingMethods { get; set; }

    public DbSet<CoffeeShopAmenity> CoffeeShopAmenities { get; set; }

    public DbSet<CoffeeShopOpeningHour> CoffeeShopOpeningHours { get; set; }

    public DbSet<CoffeeShopReview> CoffeeShopReviews { get; set; }

    public DbSet<Domain.Entities.Coffee> Coffees { get; set; }

    public DbSet<CoffeePrice> CoffeePrices { get; set; }

    public DbSet<CoffeeImage> CoffeeImages { get; set; }

    public DbSet<CoffeeReview> CoffeeReviews { get; set; }

    public DbSet<CoffeeEquipment> CoffeeEquipments { get; set; }

    public DbSet<CoffeeEquipmentCategory> CoffeeEquipmentCategories { get; set; }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        List<EntityEntry<IAuditable>> entries = GetPendingAuditableEntries();

        if (entries.Count > 0)
        {
            string? profileEmail = null;

            if (string.IsNullOrWhiteSpace(_auditUser.Email) && !string.IsNullOrWhiteSpace(_auditUser.ExternalId))
            {
                profileEmail = FindProfileEmailQuery(_auditUser.ExternalId).FirstOrDefault();
            }

            StampAuditUser(entries, profileEmail);
        }

        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        List<EntityEntry<IAuditable>> entries = GetPendingAuditableEntries();

        if (entries.Count > 0)
        {
            string? profileEmail = null;

            if (string.IsNullOrWhiteSpace(_auditUser.Email) && !string.IsNullOrWhiteSpace(_auditUser.ExternalId))
            {
                profileEmail = await FindProfileEmailQuery(_auditUser.ExternalId)
                    .FirstOrDefaultAsync(cancellationToken);
            }

            StampAuditUser(entries, profileEmail);
        }

        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoffeeJournalDbContext).Assembly);

        foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes()
            .Where(type => typeof(IAuditable).IsAssignableFrom(type.ClrType)))
        {
            foreach (string propertyName in new[] { nameof(IAuditable.CreatedBy), nameof(IAuditable.UpdatedBy) })
            {
                modelBuilder
                    .Entity(entityType.ClrType)
                    .Property(propertyName)
                    .HasMaxLength(AuditUserMaxLength)
                    .IsRequired();
            }
        }
    }

    private List<EntityEntry<IAuditable>> GetPendingAuditableEntries()
    {
        return ChangeTracker
            .Entries<IAuditable>()
            .Where(entry => entry.State is EntityState.Added or EntityState.Modified)
            .ToList();
    }

    private IQueryable<string> FindProfileEmailQuery(string externalId)
    {
        return Profiles
            .AsNoTracking()
            .Where(profile => profile.ExternalId == externalId)
            .Select(profile => profile.Email);
    }

    /// <summary>
    /// The email comes from the access token. If the token has none, it comes from the profile of the user.
    /// A profile that is being created has no saved profile yet, so it uses its own email.
    /// </summary>
    private void StampAuditUser(List<EntityEntry<IAuditable>> entries, string? profileEmail)
    {
        string? email = !string.IsNullOrWhiteSpace(_auditUser.Email) ? _auditUser.Email : profileEmail;

        foreach (EntityEntry<IAuditable> entry in entries)
        {
            string? entryEmail = email;

            if (string.IsNullOrWhiteSpace(entryEmail)
                && entry is { State: EntityState.Added, Entity: Profile newProfile })
            {
                entryEmail = newProfile.Email;
            }

            if (string.IsNullOrWhiteSpace(entryEmail))
            {
                throw new InvalidOperationException(
                    "There is no authenticated user email to record in CreatedBy and UpdatedBy.");
            }

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = entryEmail;
            }

            entry.Entity.UpdatedBy = entryEmail;
        }
    }
}
