using System;
using System.Linq;
using DAL.Models;

namespace DAL.Repositories
{
    public class RefreshTokenRepository
    {
        private JWTContext Context { get; }

        public RefreshTokenRepository(JWTContext context)
        {
            Context = context;
        }

        public RefreshToken Get(string id)
        {
            var token = Context.RefreshTokens.FirstOrDefault(x => x.Id == id);

            return CheckExpires(token);
        }

        public void Add(RefreshToken token)
        {
            Context.RefreshTokens.Add(token);

            Context.SaveChanges();
        }

        private RefreshToken CheckExpires(RefreshToken entity)
        {
            if (entity?.ExpiresUtc < DateTime.UtcNow)
            {
                Context.RefreshTokens.Remove(entity);
                Context.SaveChanges();

                return null;
            }
            else
            {
                return entity;
            }
        }
    }
}
