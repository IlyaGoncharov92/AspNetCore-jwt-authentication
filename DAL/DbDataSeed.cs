﻿using System.Linq;
using DAL.Models;

namespace DAL
{
    public static class DbDataSeed
    {
        public static void Seed(JWTContext context)
        {
            CreateUsers(context);
        }

        private static void CreateUsers(JWTContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.Add(new User
                {
                    Email = "superAdmin@gmail.com",
                    Password = "123",
                    Name = "Yasya",
                    Role = Role.SuperAdmin
                });

                context.Users.Add(new User
                {
                    Email = "admin@gmail.com",
                    Password = "123",
                    Name = "Yasya2",
                    Role = Role.Admin
                });

                context.Users.Add(new User
                {
                    Email = "user@gmail.com",
                    Password = "123",
                    Name = "Yasya3",
                    Role = Role.None
                });

                context.SaveChanges();
            }
        }
    }
}
