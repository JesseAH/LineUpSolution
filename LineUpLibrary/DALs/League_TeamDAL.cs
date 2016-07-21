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

        private League_TeamDTO EFtoDTO(league_team ef)
        {
            League_TeamDTO dto = new League_TeamDTO();
            league_calculations lc = db.league_calculations.Where(l => l.league_team_id == ef.id).FirstOrDefault();

            dto.id = ef.id;
            dto.name = ef.name;
            dto.user_id = ef.user_id;
            dto.user_name = ef.user == null ? null : ef.user.username;
            dto.league_id = ef.league_id;
            dto.league_name = ef.league == null ? null : ef.league.name;
            dto.league_team_points_sum = lc == null ? null : lc.league_team_sum;

            dto.rounds = (from r in db.rounds.Where(r => r.game_type_id == ef.league.game_type_id)
                          from rc in db.round_calculations.Where(rc => rc.round_id == r.id && rc.league_team_id == dto.id).DefaultIfEmpty()
                          select new RoundDTO()
                          {
                              id = r.id,
                              name = r.name,
                              round_number = r.round_number,
                              round_points_sum = rc == null ? null : rc.round_sum,
                              start_date = r.start_date,
                              end_date = r.end_date,
                              short_name = r.short_name
                              //picks = db.picks.Where(p => p.league_team_id == ef.id && r.matches.Where(m => m.round_id == r.id).ToList().Any(c => c.id == p.match_id)).Select(newP => new PickDTO() { }).ToList()
                          }).ToList();

            return dto;

        }

        private User_League_TeamDTO EFtoDTOForUser(league_team ef)
        {
            User_League_TeamDTO dto = new User_League_TeamDTO();
            league_calculations lc = db.league_calculations.Where(l => l.league_team_id == ef.id).FirstOrDefault();
            Random rnd = new Random();

            dto.id = ef.id;
            dto.name = ef.name;
            dto.user_id = ef.user_id;
            dto.user_name = ef.user == null ? null : ef.user.username;
            dto.league_id = ef.league_id;
            dto.league_name = ef.league == null ? null : ef.league.name;
            dto.league_team_points_sum = lc == null ? null : lc.league_team_sum;
            dto.is_completed = lc == null ? false : lc.is_completed;

            dto.rounds = (from r in db.rounds.Where(r => r.game_type_id == ef.league.game_type_id)
                          from rc in db.round_calculations.Where(rc => rc.round_id == r.id && rc.league_team_id == dto.id).DefaultIfEmpty()
                          select new RoundDTO()
                          {
                              id = r.id,
                              name = r.name,
                              round_number = r.round_number,
                              round_points_sum = rc == null ? null : rc.round_sum,
                              start_date = r.start_date,
                              end_date = r.end_date,
                              short_name = r.short_name,
                              picks = db.picks.Where(p => p.league_team_id == ef.id && r.matches.Where(m => m.round_id == r.id).ToList().Any(c => c.id == p.match_id)).Select(newP => new PickDTO()
                              {
                                  id = newP.id,
                                  picked_team_name = "picked_team_name",
                                  confidence_value = 5,
                                  is_winner = true
                              })
                              .ToList()
                          }).ToList();

            return dto;

        }


        private league_team DTOtoEF(League_TeamDTO dto, league_team ef)
        {
            ef.name = ef.name;
            ef.user_id = ef.user_id;
            ef.league_id = ef.league_id;

            return ef;
        }

        public IList<League_TeamDTO> GetList()
        {
            return db.league_team.Select(l => EFtoDTO(l)).ToList();
        }

        public User_League_TeamDTO Get(int id)
        {
            league_team lt = db.league_team
                            .Where(l => l.id == id)
                            .Include(l => l.picks)
                            .Include(l => l.picks.Select(p => p.match))
                            .Include(l => l.picks.Select(p => p.match.round))
                            .FirstOrDefault();

            return EFtoDTOForUser(lt);
        }

        public IList<User_League_TeamDTO> GetListByUser(int userID)
        {
            return db.league_team.Where(l => l.user_id == userID).ToList().Select(l => EFtoDTOForUser(l)).ToList();
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

            return EFtoDTO(myLeague_Team);
        }

        public void Save(League_TeamDTO dto)
        {
            league_team existingweek = db.league_team.Where(l => l.id == dto.id).FirstOrDefault();

            existingweek = DTOtoEF(dto, existingweek);
            existingweek.modified_on = DateTime.Today;

            db.SaveChanges();
        }

        public void Create(League_TeamDTO dto)
        {
            league_team newteam = new league_team();

            newteam = DTOtoEF(dto, newteam);
            newteam.created_on = DateTime.Today;

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
