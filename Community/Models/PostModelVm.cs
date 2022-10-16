using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Community.Models
{
    public class PostModelVm
    {
        public List<CategoryVM> categories;
        public List<Country> countries;
        public List<State> states;
        public List<City> cities;
        public List<Post> posts;
    }
}