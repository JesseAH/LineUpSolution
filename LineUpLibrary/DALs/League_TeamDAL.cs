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
    public class League_TeamDAL
    {
        Entities db = new Entities();

        private League_TeamDTO EFtoDTO(league_team ef, bool getRounds)
        {
            League_TeamDTO dto = new League_TeamDTO();
            league_summary ls = db.league_summary.Where(l => l.league_team_id == ef.id).FirstOrDefault();

            dto.id = ef.id;
            dto.name = ef.name;
            dto.user_id = ef.user_id;
            dto.user_name = ef.user == null ? null : ef.user.username;
            dto.league_id = ef.league_id;
            dto.league_name = ef.league == null ? null : ef.league.name;
            dto.league_team_points_sum = ls == null ? 0 : ls.league_team_sum;
            dto.total_winnings = ls.total_winnings == null ? 0 : ls.total_winnings;
            dto.league_ranking = ls == null ? 0 : ls.league_team_rank;
            dto.leagues_league_team_count = ls == null ? 0 : ls.league_team_count;
            dto.league_is_completed = ls == null ? false : ls.is_completed;

            if (getRounds)
            {
                dto.rounds = (from r in db.rounds.Where(r => r.game_type_id == ef.league.game_type_id)
                              from rc in db.round_calculations.Where(rc => rc.round_id == r.id && rc.league_team_id == dto.id).DefaultIfEmpty()
                              select new RoundDTO()
                              {
                                  id = r.id,
                                  name = r.name,
                                  round_number = r.round_number,
                                  round_points_sum = rc == null ? 0 : rc.round_sum,
                                  start_date = r.start_date,
                                  end_date = r.end_date,
                                  short_name = r.short_name
                              }).ToList();

                foreach (RoundDTO rnd in dto.rounds)
                {
                    rnd.start_date = rnd.start_date == null ? (DateTime?)null : new DateTime(rnd.start_date.Value.Year, rnd.start_date.Value.Month, rnd.start_date.Value.Day, rnd.start_date.Value.Hour, rnd.start_date.Value.Minute, 0, DateTimeKind.Utc);
                    rnd.end_date = rnd.end_date == null ? (DateTime?)null : new DateTime(rnd.end_date.Value.Year, rnd.end_date.Value.Month, rnd.end_date.Value.Day, rnd.end_date.Value.Hour, rnd.end_date.Value.Minute, 0, DateTimeKind.Utc);
                }
            }

            return dto;

        }

        private User_League_TeamDTO EFtoDTOForUser(league_team ef, bool getRounds)
        {
            User_League_TeamDTO dto = new User_League_TeamDTO();
            league l = db.leagues.Where(le => le.id == ef.league_id).FirstOrDefault();
            league_summary ls = db.league_summary.Where(lec => lec.league_team_id == ef.id).FirstOrDefault();
            RoundDAL roundDAL = new RoundDAL();

            dto.id = ef.id;
            dto.name = ef.name;
            dto.user_id = ef.user_id;
            dto.user_name = ef.user == null ? null : ef.user.username;
            dto.league_id = ef.league_id;
            dto.league_name = ef.league == null ? null : ef.league.name;
            dto.league_team_points_sum = ls == null ? 0 : ls.league_team_sum;
            dto.league_is_completed = ls == null ? false : ls.is_completed;
            dto.total_winnings = ls.total_winnings == null ? 0 : ls.total_winnings;
            dto.league_total_pot = l.league_team.Count() * l.price;
            dto.league_ranking = ls == null ? 0 : ls.league_team_rank;
            dto.leagues_league_team_count = ls == null ? 0 : ls.league_team_count;
            dto.league_points_per_pick = ls == null ? null : ls.points_per_pick;
            if (getRounds)
            {
                dto.rounds = (from r in db.rounds.Where(r => r.game_type_id == ef.league.game_type_id)
                              from rs in db.round_summary.Where(rs => rs.round_id == r.id && rs.league_team_id == dto.id).DefaultIfEmpty()
                              select new RoundDTO()
                              {
                                  id = r.id,
                                  name = r.name,
                                  round_number = r.round_number,
                                  round_points_sum = rs == null ? 0 : rs.round_sum,
                                  round_winnings = rs == null ? 0 : rs.winnings_sum,
                                  is_winner = rs.is_winner == 0 ? false : true,
                                  start_date = r.start_date,
                                  end_date = r.end_date,
                                  short_name = r.short_name
                              }).ToList();

                foreach (RoundDTO rnd in dto.rounds)
                {
                    rnd.start_date = rnd.start_date == null ? (DateTime?)null : new DateTime(rnd.start_date.Value.Year, rnd.start_date.Value.Month, rnd.start_date.Value.Day, rnd.start_date.Value.Hour, rnd.start_date.Value.Minute, 0, DateTimeKind.Utc);
                    rnd.end_date = rnd.end_date == null ? (DateTime?)null : new DateTime(rnd.end_date.Value.Year, rnd.end_date.Value.Month, rnd.end_date.Value.Day, rnd.end_date.Value.Hour, rnd.end_date.Value.Minute, 0, DateTimeKind.Utc);
                }
            }

            return dto;

        }


        private league_team DTOtoEF(League_TeamDTO dto, league_team ef)
        {
            ef.name = dto.name;
            ef.user_id = dto.user_id;
            ef.league_id = dto.league_id;

            return ef;
        }

        public IList<League_TeamDTO> GetList()
        {
            return db.league_team.Select(l => EFtoDTO(l, true)).OrderBy(ul => ul.league_is_completed).ToList();
        }

        public User_League_TeamDTO Get(int id)
        {
            league_team lt = db.league_team
                            .Where(l => l.id == id)
                            .Include(l => l.picks)
                            .Include(l => l.picks.Select(p => p.match))
                            .Include(l => l.picks.Select(p => p.match.round))
                            .FirstOrDefault();

            return EFtoDTOForUser(lt, true);
        }

        public IList<User_League_TeamDTO> GetListByUser(int userID)
        {
            return db.league_team.Where(l => l.user_id == userID).ToList().Select(l => EFtoDTOForUser(l, true)).OrderBy(ul => ul.league_is_completed).ToList();
        }

        public League_TeamDTO Get(int id, int userID)
        {
            league_team myLeague_Team = db.league_team
                                        .Where(l => l.id == id)
                                        //.Include(l => l.picks)
                                        //.Include(l => l.picks.Select(p => p.match))
                                        //.Include(l => l.picks.Select(p => p.match.round))
                                        .FirstOrDefault();

            if (myLeague_Team == null)
                throw new System.ArgumentException("A league_team with this id does not exist: " + id, " league_team.id");

            return EFtoDTO(myLeague_Team, true);
        }

        public void Save(League_TeamDTO dto)
        {
            league_team existingweek = db.league_team.Where(l => l.id == dto.id).FirstOrDefault();

            existingweek = DTOtoEF(dto, existingweek);
            existingweek.modified_on = DateTime.Today.ToUniversalTime();

            db.SaveChanges();
        }

        public void Create(League_TeamDTO dto)
        {
            league_team newteam = new league_team();

            newteam = DTOtoEF(dto, newteam);
            newteam.created_on = DateTime.Now.ToUniversalTime();
            newteam.modified_on = DateTime.Now.ToUniversalTime();

            db.league_team.Add(newteam);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            db.league_team.Remove(db.league_team.Where(l => l.id == id).FirstOrDefault());
            db.SaveChanges();
        }
    }
}
