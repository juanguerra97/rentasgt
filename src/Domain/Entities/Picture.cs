using rentasgt.Domain.Enums;

namespace rentasgt.Domain.Entities
{

    /// <summary>
    /// Info about pictures
    /// </summary>
    public class Picture
    {

        public Picture()
        { }

        public Picture(PictureStorageType storageType, string pictureContent)
        {
            StorageType = storageType;
            PictureContent = pictureContent;
        }

        public long Id { get; set; }

        /// <summary>
        /// Type of storage of the picture
        /// </summary>
        public PictureStorageType StorageType { get; set; }

        /// <summary>
        /// Content of the picture which depends on the storage type
        /// </summary>
        public string PictureContent { get; set; }

    }
}
