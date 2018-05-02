using System.Linq;
using DAL.Models;

namespace DAL.Repositories
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
