﻿using EducationManual.Models;
using Microsoft.AspNet.SignalR;
using System.Linq;

namespace EducationManual.Hubs
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest connection)
        {
            var db = new ApplicationContext();
            var userId = db.Users.FirstOrDefault(u => u.UserName == connection.User.Identity.Name).Id;
            return userId.ToString();
        }
    }
}