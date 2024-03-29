﻿using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Text;
using System.Web;

namespace EducationManual.Models
{
    public static class DataSave
    {
        private static string photo;

        public static int? SchoolId { get; set; } = null;

        public static string SchoolName { get; set; } = "";

        public static string Photo 
        {
            get 
            {
                if (photo is null)
                {
                    using(ApplicationContext db = new ApplicationContext())
                    {
                        string id = HttpContext.Current.User.Identity.GetUserId();
                        try
                        {
                            var user = db.Users.First(u => u.Id == id);
                            if (user.ProfilePicture != null)
                            {
                                photo = Encoding.ASCII.GetString(user.ProfilePicture);
                            }
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                return photo ?? DefaultPicture;
            }  
            set
            {
                photo = value;
            }
        }

        public static string DefaultPicture { get; set; } = "data:image/png;base64,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";
    }
}