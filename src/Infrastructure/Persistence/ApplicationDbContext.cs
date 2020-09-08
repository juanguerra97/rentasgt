using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Common;
using rentasgt.Domain.Entities;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using rentasgt.Application.Common.DB;

namespace rentasgt.Infrastructure.Persistence
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<AppUser>, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        private IDbContextTransaction _currentTransaction;

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions,
            ICurrentUserService currentUserService,
            IDateTime dateTime) : base(options, operationalStoreOptions)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        public DbSet<TodoList> TodoLists { get; set; }

        public DbSet<TodoItem> TodoItems { get; set; }

        public DbSet<AddressPicture> AddressPictures { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<ChatMessage> ChatMessages { get; set; }

        public DbSet<ChatRoom> ChatRooms { get; set; }

        public DbSet<DpiPicture> DpiPictures { get; set; }

        public DbSet<Picture> Pictures { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<ProductPicture> ProductPictures { get; set; }

        public DbSet<ProfilePicture> ProfilePictures { get; set; }

        public DbSet<Rent> Rents { get; set; }

        public DbSet<RentEvent> RentEvents { get; set; }

        public DbSet<RentCost> RentCosts { get; set; }

        public DbSet<RentRequest> RentRequests { get; set; }

        public DbSet<RequestEvent> RequestEvents { get; set; }

        public DbSet<UserChatRoom> UserChatRooms { get; set; }

        public DbSet<UserPicture> UserPictures { get; set; }

        public DbSet<UserProfileEvent> UserProfileEvents { get; set; }

        public DbSet<Conflict> Conflicts { get; set; }

        public DbSet<ConflictRecord> ConflictRecords { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.Created = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = await base.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false);
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync().ConfigureAwait(false);

                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.HasDbFunction(() => CustomDbFunctions.CalculateDistance(0, 0, 0, 0));
            builder.Entity<AppUser>(userBuilder =>
            {
                userBuilder.Property(u => u.ProfileStatus)
                    .IsRequired();

                userBuilder.Property(u => u.FirstName)
                    .HasMaxLength(AppUser.MAX_FIRSTNAME_LENGTH)
                    .IsRequired();

                userBuilder.Property(u => u.LastName)
                    .HasMaxLength(AppUser.MAX_LASTNAME_LENGTH)
                    .IsRequired();

                userBuilder.Property(u => u.Cui)
                    .HasMaxLength(13)
                    .IsFixedLength()
                    .IsRequired(false);

                userBuilder.HasIndex(u => u.Cui).IsUnique();

                userBuilder.Property(u => u.Address)
                    .HasMaxLength(AppUser.MAX_ADDRESS_LENGTH)
                    .IsRequired(false);

                userBuilder.HasOne(u => u.DpiPicture)
                    .WithOne(p => p.User)
                    .HasForeignKey((DpiPicture p) => p.UserId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Cascade);

                userBuilder.HasOne(u => u.UserPicture)
                    .WithOne(p => p.User)
                    .HasForeignKey((UserPicture p) => p.UserId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Cascade);

                userBuilder.HasOne(u => u.ProfilePicture)
                    .WithOne(p => p.User)
                    .HasForeignKey((ProfilePicture p) => p.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                userBuilder.HasOne(u => u.AddressPicture)
                    .WithOne(p => p.User)
                    .HasForeignKey((AddressPicture p) => p.UserId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Cascade);

                userBuilder.HasMany(u => u.Products)
                    .WithOne(p => p.Owner)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                userBuilder.HasMany(u => u.RentRequests)
                    .WithOne(r => r.Requestor)
                    .HasForeignKey(r => r.RequestorId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                userBuilder.HasMany(u => u.ProfileEvents)
                    .WithOne(e => e.UserProfile)
                    .HasForeignKey(e => e.UserProfileId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);


            });

        }
    }
}
