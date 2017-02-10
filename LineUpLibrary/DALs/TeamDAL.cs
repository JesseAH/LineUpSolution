using LineUpLibrary.DTOs;
using LineUpLibrary.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineUpLibrary.DALs
{
    public class TeamDAL
    {
        Entities db = new Entities();

        private TeamDTO EFtoDTO(team ef)
        {
            TeamDTO dto = new TeamDTO();

            dto.id = ef.id;
            dto.name = ef.name;
            dto.description = ef.description;
            dto.game_type_id = ef.game_type_id;
            dto.created_on = ef.created_on;
            dto.modified_on = ef.modified_on;

            return dto;
        }

        private team DTOtoEF(TeamDTO dto, team ef)
        {
            ef.id = dto.id;
            ef.name = dto.name;
            ef.description = dto.description;
            ef.game_type_id = dto.game_type_id;

            return ef;
        }

        public TeamDTO Get(int id)
        {
            team myTeam = db.teams.Where(p => p.id == id).FirstOrDefault();

            if (myTeam == null)
                throw new System.ArgumentException("A team with this id does not exist: " + id, " pick.id");

            return EFtoDTO(myTeam);
        }

        public IList<TeamDTO> GetByGame(int id)
        {
            IList<team> myTeams = db.teams.Where(t => t.game_type_id == id).AsNoTracking().ToList();

            return myTeams.Select(t => EFtoDTO(t)).ToList();
        }

        public int DTOSave(TeamDTO dto)
        {
            team ef;

            if (dto.id == 0)
            {
                ef = new team();
            }
            else
            {
                ef = db.teams.Where(g => g.id == dto.id).FirstOrDefault();
            }


            return Save(DTOtoEF(dto, ef));

        }

        public int Save(team ef)
        {

            ef.modified_on = DateTime.Now.ToUniversalTime();

            if (ef.id == 0)
            {
                ef.created_on = DateTime.Now.ToUniversalTime();
                db.teams.Add(ef);
            }


            db.SaveChanges();

            return ef.id;
        }
    }
}
