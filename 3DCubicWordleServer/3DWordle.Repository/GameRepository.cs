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
    public class GameRepository
    {
        public DataContext Context { get; set; }

        public GameRepository(DataContext context)
        {
            Context = context;
        }

        public async Task CreateGame(UserEntity user1,  UserEntity user2)
        {
            var face1 = new FacesEntity()
            {
                User = user1,
                Face1 = DateTime.MinValue,
                Face2 = DateTime.MinValue,
                Face3 = DateTime.MinValue,
                Face4 = DateTime.MinValue,
                Face5 = DateTime.MinValue,
                Face6 = DateTime.MinValue,
            };

            var face2 = new FacesEntity()
            {
                User = user2,
                Face1 = DateTime.MinValue,
                Face2 = DateTime.MinValue,
                Face3 = DateTime.MinValue,
                Face4 = DateTime.MinValue,
                Face5 = DateTime.MinValue,
                Face6 = DateTime.MinValue,
            };

            var game = new GameEntity()
            {
                Started = DateTime.Now,
                Faces = new List<FacesEntity>() { face1, face2 },
            };

            await Context.AddAsync(game);
            await Context.SaveChangesAsync();
        }

        public async Task<GameEntity> GetByIdAsync(Guid id)
        {
            var model = await Context.Games.FirstOrDefaultAsync(x => x.Id == id);

            return model;
        }

        public async Task AddAsync(GameEntity game)
        {
            await Context.Games.AddAsync(game);
            await Context.SaveChangesAsync();
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            bool removed = false;


            var model = await Context.Games.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
                return removed;

            removed = true;

            return removed;
        }
    }
}
