using LineUpLibrary.DTOs;
using LineUpLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineUpLibrary.DALs
{
    public class LeagueDAL
    {
        Entities db = new Entities();

        private LeagueDTO EFtoDTO(league ef)
        {
            LeagueDTO dto = new LeagueDTO();

            dto.id = ef.id;
            dto.name = ef.name;
            dto.game_type_id = ef.game_type_id;
            dto.game_type_name = ef.game_type == null ? null : ef.game_type.name;
            dto.is_private = ef.is_private;
            dto.description = ef.description;
            dto.max_players = ef.max_players;
            dto.price = ef.price;
            dto.lock_date = ef.lock_date;

            dto.league_teams = (from lt in ef.league_team
                                from ltc in db.league_calculations.Where(lc => lc.league_team_id == lt.id).DefaultIfEmpty()
                                select new League_TeamDTO()
                                {
                                    id = lt.id,
                                    league_name = ef.name,
                                    league_id = lt.league_id,
                                    name = lt.name,
                                    user_id = lt.user_id,
                                    user_name = lt.user == null ? null : lt.user.username,
                                    is_paid_up = lt.is_paid_up,
                                    total_winnings = 100,
                                    league_team_points_sum = ltc.league_team_sum,
                                    rounds = (from r in db.rounds.Where(r => r.game_type_id == ef.game_type_id)
                                              from rc in db.round_calculations.Where(rc => rc.round_id == r.id && rc.league_team_id == lt.id).DefaultIfEmpty()
                                              from rwc in db.round_winnings_calculations.Where(rwc => rwc.league_id == ef.id && rwc.round_id == rc.round_id && rwc.winning_score == rc.round_sum).DefaultIfEmpty()
                                              select new RoundDTO()
                                              {
                                                  id = r.id,
                                                  name = r.name,
                                                  round_number = r.round_number,
                                                  round_points_sum = rc == null ? null : rc.round_sum,
                                                  start_date = r.start_date,
                                                  end_date = r.end_date,
                                                  round_winnings = rwc.round_pot_value
                                              }).ToList()
                                }).ToList();

            return dto;
        }

        private decimal? GetRoundWinnings(int leagueID, int roundID, int roundSum)
        {
            IList<round_winnings_calculations> rwc = db.round_winnings_calculations
                                                        .Where(r => r.league_id == leagueID && r.round_id == roundID && r.winning_score == roundSum)
                                                        .ToList();

            if (rwc.Count() > 0)
                return rwc[0].round_pot_value / rwc.Count();
            else
                return null;
        }

        private league DTOtoEF(LeagueDTO dto, league ef)
        {
            ef.name = dto.name;
            ef.game_type_id = dto.game_type_id;
            ef.is_private = dto.is_private;
            ef.price = dto.price;
            ef.password = dto.password;
            ef.lock_date = dto.lock_date;
            ef.max_players = dto.max_players;

            return ef;
        }

        public IList<LeagueDTO> GetList()
        {

            IList<league> list = db.leagues
                .Where(l => l.is_completed != true)
                .ToList();

            return list.Select(l => EFtoDTO(l)).ToList();

        }

        public LeagueDTO Get(int id, int userID)
        {
            league myLeague = db.leagues.Where(l => l.id == id).FirstOrDefault();

            if (myLeague == null)
                throw new System.ArgumentException("A league with this id does not exist: " + id, " league.id");

            if (myLeague.league_team.Where(t => t.user_id == userID).ToList().Count() == 0)
                throw new System.ArgumentException("This user does not have access to this league: " + userID, " user.id");

            return EFtoDTO(myLeague);
        }

        public IList<LeagueDTO> GetListByUser(int userID)
        {
            return db.league_team.Where(l => l.user_id == userID).ToList().Select(l => EFtoDTO(l.league)).ToList();
        }

        public void AddLeagueTeam(string name, int leagueID, int userID)
        {
            db.league_team.Add(new league_team()
            {
                name = name,
                league_id = leagueID,
                user_id = userID,
                created_on = DateTime.Now
            });

            db.SaveChanges();
        }

        public bool leagueAuthorization(int leagueID, string pass)
        {
            league myLeague = db.leagues.Where(l => l.id == leagueID).FirstOrDefault();

            if (myLeague.is_private == true)
            {

                if (myLeague == null)
                    throw new System.ArgumentException("A league with this id does not exist: " + leagueID, " league.id");

                if (myLeague.password != pass)
                    return false;
            }

            return true;
        }

        public void Save(LeagueDTO dto)
        {
            league existingLeague = db.leagues.Where(l => l.id == dto.id).FirstOrDefault();

            existingLeague = DTOtoEF(dto, existingLeague);
            existingLeague.modified_on = DateTime.Today;

            db.SaveChanges();
        }

        public LeagueDTO Create(LeagueDTO dto)
        {
            league newleague = new league();

            newleague = DTOtoEF(dto, newleague);
            newleague.created_on = DateTime.Today;

            db.leagues.Add(newleague);
            db.SaveChanges();
            return EFtoDTO(newleague);
        }

        public void Delete(int id)
        {
            db.leagues.Remove(db.leagues.Where(l => l.id == id).FirstOrDefault());
            db.SaveChanges();
        }
    }
}
