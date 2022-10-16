using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Community.Models
{
    public class TagPost
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public int PostId { get; set; }

        public PostModel Post { get; set; }
        public TagModel Tag { get; set; }
    }
}