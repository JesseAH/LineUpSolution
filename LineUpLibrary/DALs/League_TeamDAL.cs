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

        /// <summary>
        /// Convert EF to DTO
        /// </summary>
        private League_TeamDTO EFtoDTO(league_team ef, bool getCalculations = false, bool getRounds = false)
        {
            League_TeamDTO dto = new League_TeamDTO();
            IList<round> gameRounds = db.rounds.Where(r => r.game_type_id == ef.league.game_type_id).AsNoTracking().ToList();
            result resultsRecord = new result();

            dto.id = ef.id;
            dto.league_name = ef.league == null ? null : ef.league.name;
            dto.league_id = ef.league_id;
            dto.name = ef.name;
            dto.user_id = ef.user_id;
            dto.user_name = ef == null ? null : ef.user.username;


            if (ef.league != null)
            {
                dto.leagues_league_team_count = db.league_team.Where(lt => lt.league_id == ef.league_id).Count();
                dto.league_is_completed = ef.league.is_completed;

                //check to see if this game has been completed and if there is a result record for this team
                if (dto.league_is_completed == true)
                    if (ef.results != null && ef.results.Count() > 0)
                    {
                        resultsRecord = ef.results.FirstOrDefault();
                        dto.total_winnings = (decimal)resultsRecord.winnings;
                        dto.league_ranking = resultsRecord.rank;
                    }

            }

            if (getCalculations)
            {
                IList<round_summary> roundSums = db.round_summary.Where(l => l.league_id == ef.league_id).AsNoTracking().ToList();
                IList<round_summary> myRoundSums = roundSums.Where(r => r.league_team_id == ef.id).ToList();

                dto.league_team_points_sum = myRoundSums == null ? 0 : myRoundSums.Sum(r => r.round_sum);
                dto.league_points_per_pick = myRoundSums == null ? 0 : myRoundSums.Sum(r => r.correct_pick_count);                
                dto.league_total_pot = ef.league == null ? null : dto.leagues_league_team_count * ef.league.price;

                //if the game has been completed, winnings will have already been filled in
                if (dto.total_winnings == null)
                    dto.total_winnings = myRoundSums == null ? 0 : myRoundSums.Sum(r => r.winnings_sum);

                if (getRounds)
                {

                    foreach (round rnd in gameRounds)
                    {
                        RoundDTO newRnd = new RoundDTO();
                        round_summary roundSum = myRoundSums.Where(rs => rs.round_id == rnd.id).FirstOrDefault();

                        newRnd.id = rnd.id;
                        newRnd.name = rnd.name;
                        newRnd.round_number = rnd.round_number;
                        newRnd.short_name = rnd.short_name;
                        newRnd.start_date = rnd.start_date == null ? (DateTime?)null : new DateTime(rnd.start_date.Value.Year, rnd.start_date.Value.Month, rnd.start_date.Value.Day, rnd.start_date.Value.Hour, rnd.start_date.Value.Minute, 0, DateTimeKind.Utc);
                        newRnd.end_date = rnd.end_date == null ? (DateTime?)null : new DateTime(rnd.end_date.Value.Year, rnd.end_date.Value.Month, rnd.end_date.Value.Day, rnd.end_date.Value.Hour, rnd.end_date.Value.Minute, 0, DateTimeKind.Utc);

                        if (roundSum != null)
                        {
                            newRnd.round_points_sum = roundSum.round_sum;
                            newRnd.round_winnings = roundSum.winnings_sum;
                            newRnd.is_winner = roundSum.is_winner == 0 ? false : true;
                        }
                        else
                        {
                            newRnd.round_points_sum = 0;
                            newRnd.round_winnings = 0;
                            newRnd.is_winner = false;
                        }

                        dto.rounds.Add(newRnd);
                    }
                }
            }




            return dto;

        }


        /// <summary>
        /// Convert DTO to EF
        /// </summary>
        private league_team DTOtoEF(League_TeamDTO dto, league_team ef)
        {
            ef.name = dto.name;
            ef.user_id = dto.user_id;
            ef.league_id = dto.league_id;

            return ef;
        }

        /// <summary>
        /// Get List
        /// </summary>
        public IList<League_TeamDTO> GetList()
        {
            return db.league_team
                .Select(l => EFtoDTO(l, true, true))
                .OrderBy(ul => ul.league_is_completed)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Get List By User
        /// </summary>
        public IList<League_TeamDTO> GetListByUser(int userID, bool getCalculations, bool getRounds)
        {
            IList<league_team> teams = db.league_team
                .Where(l => l.user_id == userID)
                .AsNoTracking()
                .ToList();

            return teams.Select(l => EFtoDTO(l, getCalculations, getRounds)).OrderBy(ul => ul.league_is_completed).ToList();
        }

        /// <summary>
        /// Get League_Team by ID
        /// </summary>
        public League_TeamDTO Get(int id, bool getCalculations, bool getRounds)
        {
            league_team myLeague_Team = db.league_team
                                        .Where(l => l.id == id)
                                        .Include(l => l.league)
                                        //.Include(l => l.picks)
                                        //.Include(l => l.picks.Select(p => p.match))
                                        //.Include(l => l.picks.Select(p => p.match.round))
                                        .AsNoTracking()
                                        .FirstOrDefault();

            if (myLeague_Team == null)
                throw new System.ArgumentException("A league_team with this id does not exist: " + id, " league_team.id");

            return EFtoDTO(myLeague_Team, getCalculations, getRounds);
        }

        /// <summary>
        /// Save
        /// </summary>
        public void Save(League_TeamDTO dto)
        {
            league_team existingweek = db.league_team.Where(l => l.id == dto.id).FirstOrDefault();

            existingweek = DTOtoEF(dto, existingweek);
            existingweek.modified_on = DateTime.Today.ToUniversalTime();

            db.SaveChanges();
        }

        /// <summary>
        /// Create
        /// </summary>
        public void Create(League_TeamDTO dto)
        {
            league_team newteam = new league_team();

            newteam = DTOtoEF(dto, newteam);
            newteam.created_on = DateTime.Now.ToUniversalTime();
            newteam.modified_on = DateTime.Now.ToUniversalTime();

            db.league_team.Add(newteam);
            db.SaveChanges();
        }

        /// <summary>
        /// Delete
        /// </summary>
        public void Delete(int id)
        {
            db.league_team.Remove(db.league_team.Where(l => l.id == id).FirstOrDefault());
            db.SaveChanges();
        }
    }
}
