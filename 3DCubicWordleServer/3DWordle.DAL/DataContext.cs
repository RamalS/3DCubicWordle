using _3DWordle.DAL.entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DWordle.DAL
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<GameEntity> Games { get; set; }
        public DbSet<FacesEntity> Faces { get; set; }
    }
}
