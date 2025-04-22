namespace RedressAPI.Models
{
    public class Favorite
    {
        public int FavoriteId { get; set; } // обранеID
        public int ProfileId { get; set; } // профільID (FK)
        public Profile Profile { get; set; }

        public int AnnouncementId { get; set; } // оголошенняID (FK)
        public Announcement Announcement { get; set; }
    }
}
