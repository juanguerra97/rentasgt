
namespace rentasgt.Domain.Entities
{
    public class ProfilePicture
    {

        public ProfilePicture()
        { }

        public ProfilePicture(AppUser user, Picture picture)
        {
            User = user;
            Picture = picture;
        }

        public ProfilePicture(Picture picture, AppUser user)
            : this(user, picture)
        { }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public long PictureId { get; set; }
        public Picture Picture { get; set; }

    }
}
