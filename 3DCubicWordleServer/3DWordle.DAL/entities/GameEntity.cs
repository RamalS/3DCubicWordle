using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DWordle.DAL.entities
{
    public class GameEntity
    {
        public Guid Id { get; set; }
        public DateTime Started { get; set; }
        public DateTime Ended { get; set; }

        public virtual ICollection<FacesEntity> Faces { get; set; }
    }
}
