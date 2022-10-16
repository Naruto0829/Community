using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Community.Models
{
    public class Country
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string NumericCode { get; set; }
        public string Phonecode { get; set; }
        public string Capital { get; set; }
        public string Currency { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string Tld { get; set; }
        public string Native { get; set; }
        public string Region { get; set; }
        public string Subregion { get; set; }
        public string Timezones { get; set; }
        public string Translations { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int? Flag { get; set; }
        public string WikiDataId { get; set; }

        //public virtual ICollection<City> Cities { get; set; }
        //public virtual ICollection<State> States { get; set; }
    }
}