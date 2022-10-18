using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Community.Models
{
    public class City
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public long? CountryId { get; set; }
        public long? StateId { get; set; }
        //public Country Country { get; set; }
        //public State State { get; set; }
    }
}