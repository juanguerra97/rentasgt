using rentasgt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<TodoList> TodoLists { get; set; }

        DbSet<TodoItem> TodoItems { get; set; }

        DbSet<AddressPicture> AddressPictures { get; set; }

        DbSet<Category> Categories { get; set; }

        DbSet<ChatMessage> ChatMessages { get; set; }

        DbSet<ChatRoom> ChatRooms { get; set; }

        DbSet<DpiPicture> DpiPictures { get; set; }

        DbSet<Picture> Pictures { get; set; }

        DbSet<Product> Products { get; set; }

        DbSet<ProductCategory> ProductCategories { get; set; }

        DbSet<ProductPicture> ProductPictures { get; set; }

        DbSet<ProfilePicture> ProfilePictures { get; set; }

        DbSet<Rent> Rents { get; set; }

        DbSet<RentEvent> RentEvents { get; set; }

        DbSet<RentCost> RentCosts { get; set; }

        DbSet<RentRequest> RentRequests { get; set; }

        DbSet<RequestEvent> RequestEvents { get; set; }

        DbSet<UserChatRoom> UserChatRooms { get; set; }

        DbSet<UserPicture> UserPictures { get; set; }

        DbSet<UserProfileEvent> UserProfileEvents { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
