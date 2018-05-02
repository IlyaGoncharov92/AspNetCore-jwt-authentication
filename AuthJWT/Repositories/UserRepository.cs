using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthJWT.Models;

namespace AuthJWT.Repositories
{
    public class UserRepository
    {
        private JWTContext Context { get; }

        public UserRepository(JWTContext context)
        {
            Context = context;
        }

        public User Find(string email, string password)
        {
            return Context.Users.FirstOrDefault(x => x.Email == email && x.Password == password);
        }
    }
}
