using LineUpLibrary.DALs;
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

    public class Game_TypeDAL
    {
        Entities db = new Entities();
        LeagueDAL myLeagueDAL = new LeagueDAL();

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

        private game_type DTOToEF(game_type ef, Game_TypeDTO dto)
        {
            ef.name = dto.name;
            ef.description = dto.description;
            ef.number_of_rounds = dto.number_of_rounds;
            ef.lock_date = dto.lock_date;
            ef.admin_user_id = dto.adminUserId;

            return ef;
        }

        public Game_TypeDTO Get(int id)
        {
            game_type gt = db.game_type
                .Where(g => g.id == id)
                .AsNoTracking()
                .FirstOrDefault();

            return new Game_TypeDTO()
            {
                id = gt.id,
                name = gt.name,
                description = gt.description,
                lock_date = gt.lock_date,
                number_of_rounds = gt.number_of_rounds,
                completed = gt.completed,
                rounds = gt.rounds == null ? null : gt.rounds.Select(r => new RoundDTO()
                {
                    id = r.id,
                    round_number = r.round_number,
                    name = r.name,
                    short_name = r.short_name,
                    lock_date = r.lock_date,
                    start_date = r.start_date,
                    end_date = r.end_date,
                    matches = r.matches == null ? null : r.matches.Select(m => new MatchDTO()).ToList()
                }).OrderBy(r => r.round_number).ToList(),
                teams = gt.teams == null ? null : gt.teams.Select(t => new TeamDTO()
                {
                    name = t.name,
                    id = t.id,
                    description = t.description,
                    game_type_id = t.game_type_id
                }).OrderBy(t => t.name).ToList()
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
                    description = g.description,
                    completed = g.completed
                }).ToList();
        }

        public IList<Game_TypeDTO> GetListByUser(int userId)
        {
            return db.game_type
                .Where(g => g.admin_user_id == userId)
                .Select(g => new Game_TypeDTO()
                {
                    id = g.id,
                    name = g.name,
                    description = g.description,
                    completed = g.completed
                }).ToList();
        }

        public bool CompletedGame(int id)
        {
            game_type game = db.game_type.Where(g => g.id == id).FirstOrDefault();
            IList<league> gameLeagues = db.leagues.Where(l => l.game_type_id == id).ToList();

            foreach (league ef in gameLeagues)
            {
                //1. Get League info, rankings
                LeagueDTO lg = myLeagueDAL.Get(ef.id, true, true, false);
                ef.is_completed = true;

                IList<League_TeamDTO> allTeams = lg.league_teams.OrderByDescending(t => t.league_team_points_sum).ToList();

                if (allTeams.Count() > 0)
                {

                    IList<League_TeamDTO> winningTeams = new List<League_TeamDTO>();
                    int? winnersTotal = allTeams[0].league_team_points_sum;

                    //2. Calculate winners
                    foreach (League_TeamDTO t in allTeams)
                    {
                        if (t.league_team_points_sum == winnersTotal)
                            winningTeams.Add(t);
                    }

                    int winnerCount = winningTeams.Count();
                    decimal? winnerPercentage = (decimal)(1 - ((double)lg.round_winnings_percentage / (double)100));
                    decimal? winnerPot = lg.total_pot * winnerPercentage;

                    foreach (League_TeamDTO t in winningTeams)
                    {
                        allTeams.Where(at => at.id == t.id).FirstOrDefault().total_winnings = t.total_winnings + (winnerPot / winnerCount);
                    }

                    //3. Insert into results table
                    foreach (League_TeamDTO t in allTeams)
                    {
                        result newResult = new result();
                        newResult.league_id = t.league_id;
                        newResult.league_team_id = t.id;
                        newResult.user_id = t.user_id;
                        newResult.rank = (int)t.league_ranking;

                        League_TeamDTO winner = winningTeams.Where(wt => wt.id == t.id).FirstOrDefault();

                        if (winner != null)
                            newResult.winnings = (double?)winner.total_winnings;
                        else
                            newResult.winnings = (double?)t.total_winnings;

                        db.results.Add(newResult);
                    }
                }
            }

            game.completed = true;
            game.modified_on = DateTime.Now.ToUniversalTime();
            db.SaveChanges();

            return true;
        }



        public int DTOSave(Game_TypeDTO dto)
        {
            game_type ef;

            if (dto.id == 0)
            {
                ef = new game_type();
            }
            else
            {
                ef = db.game_type.Where(g => g.id == dto.id).FirstOrDefault();
            }


            return Save(DTOToEF(ef, dto));

        }

        public int Save(game_type ef)
        {

            ef.modified_on = DateTime.Now.ToUniversalTime();

            if (ef.id == 0)
            {
                ef.created_on = DateTime.Now.ToUniversalTime();
                db.game_type.Add(ef);
            }


            db.SaveChanges();

            return ef.id;
        }



    }
}
