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

                if (i == 0 || i  == 4)
                {
                    resultHtml += "<div class='item "+ active + @"' style='margin-left:40px'>";
                    i = 0;
                }

                resultHtml += @"<div class='col-xs-2' style='width: auto;'>
                                   <a href='/post/details/" + item.Id+@"'>
                                      <img src ='"+item.Images.First().FilePath+@"' class='img-responsive' style='height:211px; width: 230px' />
                                    </a>
                                </div>";

                if (i  == 3 || (posts.Count() % 4 !=0 && j == posts.Count()))
                {
                    resultHtml += "</div>";
                }

                i++;
            }

            return resultHtml;


        }
    }
}