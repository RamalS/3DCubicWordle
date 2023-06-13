using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DWordle.DAL.entities
{
    public class FacesEntity
    {
        public Guid GameId { get; set; }
        public GameEntity Game { get; set; }
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }
        public DateTime Face1 { get; set; }
        public DateTime Face2 { get; set; }
        public DateTime Face3 { get; set; }
        public DateTime Face4 { get; set; }
        public DateTime Face5 { get; set; }
        public DateTime Face6 { get; set; }
    }
}
