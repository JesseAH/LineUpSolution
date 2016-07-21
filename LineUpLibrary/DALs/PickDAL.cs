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
    public class PickDAL
    {

        Entities db = new Entities();

        private PickDTO EFtoDTO(pick ef)
        {
            PickDTO dto = new PickDTO();

            dto.id = ef.id;
            dto.match_id = ef.match_id;
            dto.league_team_id = ef.picked_team_id;
            dto.confidence_value = ef.confidence_value;
            dto.picked_team_id = ef.picked_team_id;

            dto.league_team_name = ef.league_team == null ? null : ef.league_team.name;
            dto.picked_team_name = ef.team == null ? null : ef.team.name;
            dto.is_winner = ef.match == null ? false : ef.match.winning_team_id == null ? false : ef.match.winning_team_id == ef.id ? true : false;
            dto.round_id = ef.match == null ? 0 : ef.match.round == null ? 0 : ef.match.round.id;
            dto.round_name = ef.match == null ? null : ef.match.round == null ? null : ef.match.round.name;

            return dto;
        }

        private pick DTOtoEF(PickDTO dto, pick ef)
        {
            ef.match_id = dto.match_id;
            ef.league_team_id = dto.league_team_id;
            ef.confidence_value = dto.confidence_value;
            ef.picked_team_id = dto.picked_team_id;

            return ef;
        }

        public IList<PickDTO> GetList()
        {
            IList<pick> list = db.picks
                                .Include(p => p.league_team)
                                .Include(p => p.team)
                                .Include(p => p.match)
                                .Include(m => m.match.round)
                                .ToList();


            return list.Select(w => EFtoDTO(w)).ToList();
        }

        public IList<objects_with_open_rounds> GetTeamsWhoNeedToMakePicks(int userID)
        {
            return db.objects_with_open_rounds.Where(o => o.user_id == userID).ToList();
        }

        public IList<PickDTO> GetListByTeam(int teamID)
        {
            IList<pick> list = db.picks
                                .Where(p => p.league_team_id == teamID)
                                .Include(p => p.league_team)
                                .Include(p => p.team)
                                .Include(p => p.match)
                                .Include(m => m.match.round)
                                .ToList();

            return list.Select(w => EFtoDTO(w)).ToList();
        }

        public PickDTO Get(int id)
        {
            pick myPick = db.picks
                                    .Where(p => p.id == id)
                                    .Include(p => p.league_team)
                                    .Include(p => p.team)
                                    .Include(p => p.match)
                                    .Include(m => m.match.round)
                                    .FirstOrDefault();

            if (myPick == null)
                throw new System.ArgumentException("A pick with this id does not exist: " + id, " pick.id");

            return EFtoDTO(myPick);
        }

        public void Save(PickDTO dto)
        {
            pick existingpick = db.picks.Where(l => l.id == dto.id).FirstOrDefault();

            existingpick = DTOtoEF(dto, existingpick);
            existingpick.modified_on = DateTime.Today;

            db.SaveChanges();
        }

        public int Create(PickDTO dto)
        {
            pick newpick = new pick();

            newpick = DTOtoEF(dto, newpick);
            newpick.created_on = DateTime.Today;

            db.picks.Add(newpick);
            db.SaveChanges();

            return newpick.id;
        }

        public void Delete(int id)
        {
            db.picks.Remove(db.picks.Where(l => l.id == id).FirstOrDefault());
            db.SaveChanges();
        }

    }
}
