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
            IList<objects_with_open_rounds> list = db.objects_with_open_rounds
                .Where(o => o.user_id == userID)
                .Where(o => o.start_date <= DateTime.UtcNow && o.end_date > DateTime.UtcNow)
                .ToList();
            return list;
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
            existingpick.modified_on = DateTime.Now.ToUniversalTime();

            db.SaveChanges();
        }

        public int Create(PickDTO dto)
        {
            if (!Match_Open(dto.match_id))
                throw new Exception("This round is locked");

            pick newpick = new pick();

            newpick = DTOtoEF(dto, newpick);
            newpick.created_on = DateTime.Now.ToUniversalTime();
            newpick.modified_on = DateTime.Now.ToUniversalTime();

            db.picks.Add(newpick);
            db.SaveChanges();

            return newpick.id;
        }

        public void Delete(int id)
        {
            db.picks.Remove(db.picks.Where(l => l.id == id).FirstOrDefault());
            db.SaveChanges();
        }

        public bool IsMyTeam(int leagueTeamID, int userid)
        {
            league_team team = db.league_team.Where(t => t.id == leagueTeamID).FirstOrDefault();

            if (team == null)
                return false;

            if (team.user_id != userid)
                return false;

            return true;
        }

        public void DeleteDuplicatePicks(int leagueTeamID, int matchID)
        {
            pick oldPick = db.picks.Where(p => p.league_team_id == leagueTeamID && p.match_id == matchID).FirstOrDefault();

            if (oldPick != null)
                db.picks.Remove(oldPick);

            db.SaveChanges();
        }

        //Is this pick's league open for changes
        public bool Match_Open(int matchID)
        {
            match myMatch = db.matches.Where(m => m.id == matchID).Include(r => r.round).FirstOrDefault();

            if (myMatch == null || myMatch.round.lock_date == null)
                return true;

            if (DateTime.Now.ToUniversalTime() <= myMatch.round.lock_date)
                return true;

            return false;
        }

        public bool Round_Open(int roundID)
        {
            objects_with_open_rounds myObj = db.objects_with_open_rounds.Where(o => o.round_id == roundID).First();

            if (myObj == null || myObj.lock_date == null)
                return true;

            if (DateTime.Now.ToUniversalTime() <= myObj.lock_date)
                return true;

            return false;
        }

    }
}
