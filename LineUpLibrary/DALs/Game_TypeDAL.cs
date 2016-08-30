using LineUpLibrary.DALs;
using LineUpLibrary.DTOs;
using LineUpLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineUpLibrary.DALs
{

    public class Game_TypeDAL
    {
        Entities db = new Entities();

        public IList<LookupSimpleDTO> DTOGetAsLookupList()
        {
            IList<LookupSimpleDTO> theList = db.game_type
                .Where(g => g.lock_date == null || g.lock_date > DateTime.UtcNow)
                .Select(g => new LookupSimpleDTO()
                {
                    Lookup_ID = g.id,
                    Lookup_Name = g.name,
                    Description = g.description
                }).OrderBy(gt => gt.Lookup_Name).ToList();

            return theList;
        }

        public Game_TypeDTO Get(int id)
        {
            game_type gt = db.game_type.Where(g => g.id == id).FirstOrDefault();

            return new Game_TypeDTO()
            {
                id = gt.id,
                name = gt.name,
                description = gt.description
            };
        }

        public IList<Game_TypeDTO> GetList()
        {
            return db.game_type
                .Where(g => g.lock_date == null || g.lock_date > DateTime.UtcNow)
                .Select(g => new Game_TypeDTO()
                {
                    id = g.id,
                    name = g.name,
                    description = g.description
                }).ToList();
        }

    }
}
