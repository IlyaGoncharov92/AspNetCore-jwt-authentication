using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthJWT.Models
{
    public static class DbData
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
                    Email = "admin@gmail.com",
                    Password = "123",
                    Name = "Yasya"
                });

                context.Users.Add(new User
                {
                    Email = "user@gmail.com",
                    Password = "3124",
                    Name = "Yasya"
                });

                context.SaveChanges();
            }
        }
    }
}
