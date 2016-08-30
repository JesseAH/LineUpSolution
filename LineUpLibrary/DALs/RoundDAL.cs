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
    public class RoundDAL
    {

        Entities db = new Entities();

        public RoundDTO EFtoDTOForTeam(round ef, int teamID)
        {
            RoundDTO dto = new RoundDTO();
            round_summary rs = db.round_summary.Where(r => r.round_id == ef.id).Where(r => r.league_team_id == teamID).FirstOrDefault();


            dto.id = ef.id;
            dto.name = ef.name;
            dto.game_type_id = ef.game_type_id;
            dto.start_date = ef.start_date == null ? (DateTime?)null : new DateTime(ef.start_date.Value.Year, ef.start_date.Value.Month, ef.start_date.Value.Day, ef.start_date.Value.Hour, ef.start_date.Value.Minute, 0, DateTimeKind.Utc);
            dto.end_date = ef.end_date == null ? (DateTime?)null : new DateTime(ef.end_date.Value.Year, ef.end_date.Value.Month, ef.end_date.Value.Day, ef.end_date.Value.Hour, ef.end_date.Value.Minute, 0, DateTimeKind.Utc);
            dto.lock_date = ef.lock_date == null ? (DateTime?)null : new DateTime(ef.lock_date.Value.Year, ef.lock_date.Value.Month, ef.lock_date.Value.Day, ef.lock_date.Value.Hour, ef.lock_date.Value.Minute, 0, DateTimeKind.Utc);
            dto.short_name = ef.short_name;
            dto.round_points_sum = rs == null ? 0 : rs.round_sum;
            dto.is_winner = rs == null ? false : rs.is_winner == 0 ? false : true;
            dto.round_winnings = rs == null ? 0 : rs.winnings_sum;
            dto.is_locked = ef.lock_date == null ? false : ef.lock_date.Value.ToUniversalTime() < DateTime.Now.ToUniversalTime();

            foreach (match mat in ef.matches)
            {
                MatchDTO m_dto = new MatchDTO();
                m_dto.id = mat.id;
                m_dto.round_id = mat.round_id;
                m_dto.description = mat.description;
                m_dto.team1_id = mat.team1_id;
                m_dto.team2_id = mat.team2_id;
                m_dto.match_type_id = mat.match_type_id;
                m_dto.winning_team_id = mat.winning_team_id;
                m_dto.team1_start_date = mat.team1_start_date == null ? (DateTime?)null : new DateTime(mat.team1_start_date.Value.Year, mat.team1_start_date.Value.Month, mat.team1_start_date.Value.Day, mat.team1_start_date.Value.Hour, mat.team1_start_date.Value.Minute, 0, DateTimeKind.Utc);
                m_dto.team2_start_date = mat.team2_start_date == null ? (DateTime?)null : new DateTime(mat.team2_start_date.Value.Year, mat.team2_start_date.Value.Month, mat.team2_start_date.Value.Day, mat.team2_start_date.Value.Hour, mat.team2_start_date.Value.Minute, 0, DateTimeKind.Utc);
                m_dto.match_outcome = mat.match_outcome;
                m_dto.team1_name = mat.team == null ? null : mat.team.name;
                m_dto.team2_name = mat.team1 == null ? null : mat.team1.name;
                m_dto.winning_team_name = mat.team2 == null ? null : mat.team2.name;
                m_dto.picks = mat.picks == null ? null : mat.picks.Where(p => p.league_team_id == teamID).Select(p => new PickDTO()
                {
                    id = p.id,
                    league_team_id = p.league_team_id,
                    match_id = p.match_id,
                    picked_team_id = p.picked_team_id,
                    confidence_value = p.confidence_value,
                    picked_team_name = p.team == null ? null : p.team.name,
                    is_winner = m_dto.winning_team_id == null ? null : p.picked_team_id == m_dto.winning_team_id ? (bool?)true : (bool?)false
                }).ToList();

                dto.matches.Add(m_dto);
            }


            if (dto.matches.Count() > 0)
                dto.max_pick_count = Math.Ceiling((double)dto.matches.Count() / 5);

            dto.game_type_name = ef.game_type == null ? null : ef.game_type.name;

            return dto;
        }

        private RoundDTO EFtoDTO(round ef, int? leagueID = null, int? leagueTeamID = null)
        {
            RoundDTO dto = new RoundDTO();
            round_summary rs = db.round_summary.Where(r => r.round_id == ef.id).FirstOrDefault();
            

            dto.id = ef.id;
            dto.name = ef.name;
            dto.game_type_id = ef.game_type_id;
            dto.start_date = ef.start_date == null ? (DateTime?)null : new DateTime(ef.start_date.Value.Year, ef.start_date.Value.Month, ef.start_date.Value.Day, ef.start_date.Value.Hour, ef.start_date.Value.Minute, 0, DateTimeKind.Utc);
            dto.end_date = ef.end_date == null ? (DateTime?)null : new DateTime(ef.end_date.Value.Year, ef.end_date.Value.Month, ef.end_date.Value.Day, ef.end_date.Value.Hour, ef.end_date.Value.Minute, 0, DateTimeKind.Utc);
            dto.lock_date = ef.lock_date == null ? (DateTime?)null : new DateTime(ef.lock_date.Value.Year, ef.lock_date.Value.Month, ef.lock_date.Value.Day, ef.lock_date.Value.Hour, ef.lock_date.Value.Minute, 0, DateTimeKind.Utc);
            dto.short_name = ef.short_name;
            dto.round_points_sum = rs == null ? 0  : rs.round_sum;
            dto.is_winner = rs.is_winner == 0 ? false : true;
            dto.round_winnings = rs.winnings_sum;

            foreach (match mat in ef.matches)
            {
                MatchDTO m_dto = new MatchDTO();
                m_dto.id = mat.id;
                m_dto.round_id = mat.round_id;
                m_dto.description = mat.description;
                m_dto.team1_id = mat.team1_id;
                m_dto.team2_id = mat.team2_id;
                m_dto.match_type_id = mat.match_type_id;
                m_dto.winning_team_id = mat.winning_team_id;
                m_dto.team1_start_date = mat.team1_start_date == null ? (DateTime?)null : mat.team1_start_date.Value.ToUniversalTime();
                m_dto.team2_start_date = mat.team2_start_date == null ? (DateTime?)null : mat.team2_start_date.Value.ToUniversalTime();
                m_dto.match_outcome = mat.match_outcome;
                m_dto.team1_name = mat.team == null ? null : mat.team.name;
                m_dto.team2_name = mat.team1 == null ? null : mat.team1.name;
                m_dto.winning_team_name = mat.team2 == null ? null : mat.team2.name;

                //Get all the picks for the given league
                if (leagueID != null)
                {
                    m_dto.picks = mat.picks.Where(p => p.league_team.league_id == leagueID).ToList() == null ? null : mat.picks.Where(p => p.league_team.league_id == leagueID).ToList().Select(p => new PickDTO()
                    {
                        id = p.id,
                        league_team_id = p.league_team_id,
                        match_id = p.match_id,
                        picked_team_id = p.picked_team_id,
                        confidence_value = p.confidence_value,
                        picked_team_name = p.team == null ? null : p.team.name,
                        is_winner = p.picked_team_id == m_dto.winning_team_id
                    }).ToList();
                }

                //Get all the picks for the given team
                if (leagueTeamID != null)
                {
                    m_dto.picks = mat.picks.Where(p => p.league_team_id == leagueTeamID).Select(p => new PickDTO()
                    {
                        id = p.id,
                        league_team_id = p.league_team_id,
                        match_id = p.match_id,
                        picked_team_id = p.picked_team_id,
                        confidence_value = p.confidence_value,
                        picked_team_name = p.team == null ? null : p.team.name,
                        is_winner = p.picked_team_id == m_dto.winning_team_id
                    }).ToList();
                }

                dto.matches.Add(m_dto);
            }

            if (dto.matches.Count() > 0)
                dto.max_pick_count = Math.Ceiling((double)dto.matches.Count() / 5);

            dto.game_type_name = ef.game_type == null ? null : ef.game_type.name;

            return dto;
        }

        private round DTOtoEF(RoundDTO dto, round ef)
        {
            ef.name = dto.name;
            ef.game_type_id = dto.game_type_id;
            ef.start_date = dto.start_date;
            ef.short_name = dto.short_name;
            ef.start_date = dto.start_date == null ? (DateTime?)null : dto.start_date.Value.ToUniversalTime();
            ef.end_date = dto.end_date == null ? (DateTime?)null : dto.end_date.Value.ToUniversalTime();
            ef.lock_date = dto.lock_date == null ? (DateTime?)null : dto.lock_date.Value.ToUniversalTime();
            ef.round_number = dto.round_number;

            return ef;
        }

        public IList<RoundDTO> GetList()
        {
            IList<round> list = db.rounds
                                .Include(w => w.game_type)
                                .Include(w => w.matches)
                                .Include(m => m.matches.Select(t => t.team))
                                .Include(m => m.matches.Select(t => t.team1))
                                .Include(m => m.matches.Select(t => t.team2))
                                .Include(m => m.matches.Select(t => t.match_type))
                                .Include(m => m.matches.Select(t => t.picks))
                                .Include(m => m.matches.Select(t => t.picks.Select(p => p.team)))
                                .ToList();

            return list.Select(w => EFtoDTO(w, null)).ToList();
        }

        public RoundDTO Get(int roundID, int? leagueID = null, int? leagueTeamID = null)
        {
            round myRound = db.rounds
                                .Where(p => p.id == roundID)
                                .Include(w => w.game_type)
                                .Include(w => w.matches)
                                .Include(m => m.matches.Select(t => t.team))
                                .Include(m => m.matches.Select(t => t.team1))
                                .Include(m => m.matches.Select(t => t.team2))
                                .Include(m => m.matches.Select(t => t.match_type))
                                .Include(m => m.matches.Select(t => t.picks))
                                .Include(m => m.matches.Select(t => t.picks.Select(p => p.team)))
                                .FirstOrDefault();

            if (myRound == null)
                throw new System.ArgumentException("A round with this id does not exist: " + roundID, "pick.id");

            return EFtoDTO(myRound, leagueID, leagueTeamID);
        }

        public void Save(RoundDTO dto)
        {
            round existinground = db.rounds.Where(l => l.id == dto.id).FirstOrDefault();

            existinground = DTOtoEF(dto, existinground);
            existinground.modified_on = DateTime.Now.ToUniversalTime();

            db.SaveChanges();
        }

        public IList<RoundDTO> GetListByLeague(int leagueID)
        {
            int? typeID = db.leagues.Where(l => l.id == leagueID).FirstOrDefault().game_type_id;
            IList<round> list = db.rounds
                            .Where(p => p.game_type_id == typeID)
                            .Include(w => w.game_type)
                            .Include(w => w.matches)
                            .Include(m => m.matches.Select(t => t.team))
                            .Include(m => m.matches.Select(t => t.team1))
                            .Include(m => m.matches.Select(t => t.team2))
                            .Include(m => m.matches.Select(t => t.match_type))
                            .Include(m => m.matches.Select(t => t.picks))
                            .Include(m => m.matches.Select(t => t.picks.Select(p => p.team)))
                            .ToList();

            return list.Select(w => EFtoDTO(w, leagueID)).ToList();
        }

        public IList<RoundDTO> GetListByTeam(int teamID)
        {
            int? leagueID = db.league_team.Where(t => t.id == teamID).FirstOrDefault().league_id;
            return GetListByLeague((int)leagueID);
        }

        public IList<RoundDTO> GetListByUsersTeam(int teamID)
        {
            int? leagueID = db.league_team.Where(t => t.id == teamID).FirstOrDefault().league_id;
            int? typeID = db.leagues.Where(l => l.id == leagueID).FirstOrDefault().game_type_id;
            IList<round> list = db.rounds
                .Where(p => p.game_type_id == typeID)
                .Include(w => w.game_type)
                .Include(w => w.matches)
                .Include(m => m.matches.Select(t => t.team))
                .Include(m => m.matches.Select(t => t.team1))
                .Include(m => m.matches.Select(t => t.team2))
                .Include(m => m.matches.Select(t => t.match_type))
                .Include(m => m.matches.Select(t => t.picks))
                .Include(m => m.matches.Select(t => t.picks.Select(p => p.team)))
                .OrderByDescending(m => m.round_number)
                .ToList();

            return list.Select(w => EFtoDTOForTeam(w, teamID)).ToList();
        }

        public void Create(RoundDTO dto)
        {
            round newRound = new round();

            newRound = DTOtoEF(dto, newRound);
            newRound.created_on = DateTime.Now.ToUniversalTime();
            newRound.modified_on = DateTime.Now.ToUniversalTime();

            db.rounds.Add(newRound);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            db.rounds.Remove(db.rounds.Where(l => l.id == id).FirstOrDefault());
            db.SaveChanges();
        }

    }
}
