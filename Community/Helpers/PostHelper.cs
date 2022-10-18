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

            string resultHtml = "";

            foreach (var item in posts)
            {
                

                string active = i == 0 ? "active" : "";

                if (i == 0 || i + 1 % 3 == 0)
                {
                    resultHtml += "<div class='item "+ active + @"'>";
                }

                resultHtml += @"<div class='col-xs-4'>
                                   <a href='/post/details/"+item.Id+@"'>
                                      <img src ='"+item.Images.First().FilePath+@"' class='img-responsive' />
                                    </a>
                                </div>";

                if ((posts.Count < 4 && posts.Count == i + 1) || i + 1 % 3 == 0)
                {
                    resultHtml += "</div>";
                }

                i++;
            }

            return resultHtml;


        }
    }
}