using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Community.Models
{
    public class State
    {
        internal string id;

        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string country_code { get; set; }
        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }
        public long? CountryId { get; set; }

        //public Country Country { get; set; }
        //public virtual ICollection<City> Cities { get; set; }
    }
}