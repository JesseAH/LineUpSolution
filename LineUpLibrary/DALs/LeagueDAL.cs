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

        public IList<LookupSimpleDTO> DTOGetAsLookupList()
        {
            IList<LookupSimpleDTO> theList = db.leagues.Select(l => new LookupSimpleDTO()
            {
                Lookup_ID = l.id,
                Lookup_Name = l.name,
                Description = l.description
            }).OrderBy(gt => gt.Lookup_Name).ToList();

            return theList;
        }

        private LeagueDTO EFtoDTO(league ef, bool getTeams, bool getTeamCalculations, bool getRounds)
        {
            LeagueDTO dto = new LeagueDTO();

            dto.id = ef.id;
            dto.name = ef.name;
            dto.game_type_id = ef.game_type_id;
            dto.game_type_name = ef.game_type == null ? null : ef.game_type.name;
            dto.number_of_rounds = ef.game_type == null ? 0 : ef.game_type.number_of_rounds;
            dto.is_private = ef.is_private;
            dto.is_completed = ef.is_completed;
            dto.description = ef.description;
            dto.max_players = ef.max_players;
            dto.price = ef.price;
            dto.lock_date = ef.lock_date == null ? (DateTime?)null : new DateTime(ef.lock_date.Value.Year, ef.lock_date.Value.Month, ef.lock_date.Value.Day, ef.lock_date.Value.Hour, ef.lock_date.Value.Minute, 0, DateTimeKind.Utc);
            dto.round_winnings_percentage = ef.round_winnings_percentage;

            if (getTeams)
            {
                if (!getTeamCalculations)
                    dto.league_teams = ef.league_team.Select(lt => new League_TeamDTO()
                    {
                        id = lt.id,
                        league_name = ef.name,
                        league_id = lt.league_id,
                        name = lt.name,
                        user_id = lt.user_id,
                    }).ToList();
                else
                {
                    IList<league_summary> summaryDTOs = db.league_summary.Where(ls => ls.league_id == ef.id).ToList();

                    dto.league_teams = (from lt in ef.league_team
                                        from ls in summaryDTOs.Where(ltc => ltc.league_team_id == lt.id).DefaultIfEmpty()
                                        select new League_TeamDTO()
                                        {
                                            id = lt.id,
                                            league_name = ef.name,
                                            league_id = lt.league_id,
                                            name = lt.name,
                                            user_id = lt.user_id,
                                            user_name = lt.user == null ? null : lt.user.username,
                                            is_paid_up = lt.is_paid_up,
                                            league_team_points_sum = ls.league_team_sum == null ? 0 : ls.league_team_sum,
                                            league_points_per_pick = ls.correct_pick_count == null ? 0 : ls.correct_pick_count,
                                            total_winnings = ls.total_winnings == null ? 0 : ls.total_winnings,
                                            league_ranking = ls == null ? 0 : ls.league_team_rank,
                                            leagues_league_team_count = ls == null ? 0 : ls.league_team_count,
                                        }).OrderByDescending(t => t.league_team_points_sum).ToList();

                    if (getRounds)
                    {
                        foreach (League_TeamDTO lt in dto.league_teams)
                        {
                            lt.rounds = (from r in db.rounds.Where(r => r.game_type_id == ef.game_type_id)
                                         from rs in db.round_summary.Where(rs => rs.round_id == r.id && rs.league_team_id == lt.id).DefaultIfEmpty()
                                         select new RoundDTO()
                                         {
                                             id = r.id,
                                             name = r.name,
                                             round_number = r.round_number,
                                             short_name = r.short_name,
                                             round_points_sum = rs == null ? 0 : rs.round_sum,
                                             round_open_sum = rs == null ? 0 : rs.open_points,
                                             lock_date = r.lock_date,
                                             start_date = r.start_date,
                                             end_date = r.end_date,
                                             is_winner = rs.is_winner == 0 ? false : true,
                                             round_winnings = rs.winnings_sum
                                         }).OrderBy(r => r.round_number).ToList();

                            foreach (RoundDTO rnd in lt.rounds)
                            {
                                rnd.start_date = rnd.start_date == null ? (DateTime?)null : new DateTime(rnd.start_date.Value.Year, rnd.start_date.Value.Month, rnd.start_date.Value.Day, rnd.start_date.Value.Hour, rnd.start_date.Value.Minute, 0, DateTimeKind.Utc);
                                rnd.end_date = rnd.end_date == null ? (DateTime?)null : new DateTime(rnd.end_date.Value.Year, rnd.end_date.Value.Month, rnd.end_date.Value.Day, rnd.end_date.Value.Hour, rnd.end_date.Value.Minute, 0, DateTimeKind.Utc);
                            }
                        }
                    }
                }
            }

            return dto;
        }

        private league DTOtoEF(LeagueDTO dto, league ef)
        {
            ef.name = dto.name;
            ef.game_type_id = dto.game_type_id;
            ef.is_private = dto.is_private;
            ef.price = dto.price;
            ef.password = dto.password;
            ef.lock_date = dto.lock_date == null ? (DateTime?)null : dto.lock_date.Value.ToUniversalTime();
            ef.max_players = dto.max_players;
            ef.round_winnings_percentage = dto.round_winnings_percentage;

            return ef;
        }

        public IList<LeagueDTO> GetList(bool getTeams = false, bool getTeamCalculations = false, bool getRounds = false)
        {

            IList<league> list = db.leagues
                .Where(l => l.is_completed != true)
                .Where(l => l.game_type.lock_date == null || l.game_type.lock_date > DateTime.UtcNow)
                .Where(l => l.lock_date == null || l.lock_date > DateTime.UtcNow)
                .OrderBy(l => l.is_private).ThenBy(l => l.name)
                .ToList();

            return list.Select(l => EFtoDTO(l, getTeams, getTeamCalculations, getRounds)).ToList();

        }

        public IList<LeagueDTO> GetListByUser(int userID, bool getTeams = false, bool getTeamsCalculations = false, bool getRounds = false)
        {
            var list = db.league_team
                .Where(l => l.user_id == userID)
                .ToList();

            //return db.league_team
            //    .Where(l => l.user_id == userID)
            //    .Select(l => EFtoDTO(l.league, getTeams, getTeamsCalculations, getRounds))
            //    .ToList();

            return list.Select(l => EFtoDTO(l.league, getTeams, getTeamsCalculations, getRounds)).ToList();
        }

        public LeagueDTO Get(int id)
        {
            if (id == 0)
                return new LeagueDTO();

            league myLeague = db.leagues.Where(l => l.id == id).FirstOrDefault();

            if (myLeague == null)
                throw new System.ArgumentException("A league with this id does not exist: " + id, " league.id");

            return EFtoDTO(myLeague, true, true, true);
        }

        public int AddLeagueTeam(string name, int leagueID, int userID)
        {
            var newTeam = new league_team()
            {
                name = name,
                league_id = leagueID,
                user_id = userID,
                created_on = DateTime.Now.ToUniversalTime()
            };

            db.league_team.Add(newTeam);

            db.SaveChanges();

            return newTeam.id;
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
            existingLeague.modified_on = DateTime.Today.ToUniversalTime();

            db.SaveChanges();
        }

        public LeagueDTO Create(LeagueDTO dto)
        {
            league newleague = new league();

            newleague = DTOtoEF(dto, newleague);
            newleague.created_on = DateTime.Today.ToUniversalTime();
            newleague.modified_on = DateTime.Today.ToUniversalTime();

            db.leagues.Add(newleague);
            db.SaveChanges();
            return EFtoDTO(newleague, false, false, false);
        }

        public void Delete(int id)
        {
            db.leagues.Remove(db.leagues.Where(l => l.id == id).FirstOrDefault());
            db.SaveChanges();
        }
    }
}
