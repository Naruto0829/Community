using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Community.Models
{
    public class Attachment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string FileName { get; set; }
        public string HashName { get; set; }
        public string FilePath { get; set; }
    }
}