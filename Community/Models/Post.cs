using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Community.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AdType { get; set; }
        public int AdvertiserType { get; set; }
        public DateTime ExpireTime { get; set; }
        public int CountryId { get; set; }
        public int RegionId { get; set; }
        public int CityId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string VideoUrl { get; set; }
        public int Status { get; set; }
        public DateTime created_at { get; set; }

        public virtual ICollection<Attachment> Images { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<ReplyAds> ReplyAdses { get; set; }
        public virtual Category Category { get; set; }
        public virtual Country Country { get; set; }
        public virtual State State { get; set; }
        public virtual City City { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}