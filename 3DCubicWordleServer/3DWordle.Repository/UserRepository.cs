using _3DWordle.DAL;
using _3DWordle.DAL.entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DWordle.Repository
{
    public class UserRepository
    {
        public DataContext Context { get; set; }

        public UserRepository(DataContext context)
        {
            Context = context;
        }

        public async Task<UserEntity> GetByIdAsync(Guid id)
        {
            var model = await Context.Users.FirstOrDefaultAsync(x => x.Id == id);

            return model;
        }

        public async Task<UserEntity> AddAsync(UserEntity user)
        {
            await Context.Users.AddAsync(user);
            await Context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            bool removed = false;


            var model = await Context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
                return removed;

            removed = true;

            return removed;
        }
    }
}
