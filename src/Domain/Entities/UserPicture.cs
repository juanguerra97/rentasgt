using System;
using System.Collections.Generic;
using System.Text;

namespace rentasgt.Domain.Entities
{
    public class UserPicture
    {

        public UserPicture()
        { }

        public UserPicture(AppUser user, Picture picture)
        {
            User = user;
            Picture = picture;
        }

        public UserPicture(Picture picture, AppUser user)
            : this(user, picture)
        { }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public long PictureId { get; set; }
        public Picture Picture { get; set; }

    }
}
