using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Community.Models;

namespace Community.Helpers
{
    public class PostHelper
    {
        public static string CarouselHtml(List<Post> posts)
        {
            int i = 0;
            int j = 0;
            string resultHtml = "";

            foreach (var item in posts)
            {
                string active = i == 0 ? "active" : "";
                j++;

                if (i == 0 || i  == 3)
                {
                    resultHtml += "<div class='item "+ active + @"'>";
                    i = 0;
                }

                resultHtml += @"<div class='col-xs-4'>
                                   <a href='/post/details/"+item.Id+@"'>
                                      <img src ='"+item.Images.First().FilePath+@"' class='img-responsive' style='height:150px' />
                                    </a>
                                </div>";

                if (i  == 2 || (posts.Count() % 3 !=0 && j == posts.Count()))
                {
                    resultHtml += "</div>";
                }

                i++;
            }

            return resultHtml;


        }
    }
}