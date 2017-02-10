using LineUpLibrary.DTOs;
using LineUpLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineUpLibrary.DALs
{
    public class MatchDAL
    {
        Entities db = new Entities();

        private match DTOtoEF(MatchDTO dto, match ef)
        {
            ef.id = dto.id;
            ef.description = dto.description;
            ef.team1_id = dto.team1_id;
            ef.team2_id = dto.team2_id;
            ef.winning_team_id = dto.winning_team_id;
            ef.round_id = dto.round_id;

            return ef;
        }

        public IList<MatchDTO> PostMatches(IList<MatchDTO> matches, int roundId)
        {
            //Check to see if we need to delete any matches
            IList<match> dbMatches = db.matches.Where(m => m.round_id == roundId).ToList();


            foreach (match dbm in dbMatches)
            {
                var matchStays = matches.Where(m => m.id == dbm.id).FirstOrDefault();

                if(matchStays == null)
                {
                    //user wants to delete match
                    db.matches.Remove(dbm);

                    //remove all picks related to the above match
                    foreach (var childPick in db.picks.Where(p => p.match_id == dbm.id))
                    {
                        if(childPick.match_id == dbm.id)
                            db.picks.Remove(childPick);
                    }
                        
                }
            }


            //Save all passed in matches
            foreach (MatchDTO m in matches)
            {
                m.round_id = roundId;
                Save(m);
            }

            //Save All Changes
            db.SaveChanges();

            return null;
        }


        public int Save(MatchDTO dto)
        {
            match ef;

            if (dto.id == 0)
            {
                ef = new match();
                ef.created_on = DateTime.Now.ToUniversalTime();
            }
            else
            {
                ef = db.matches.Where(m => m.id == dto.id).FirstOrDefault();
            }


            ef = DTOtoEF(dto, ef);
            ef.modified_on = DateTime.Now.ToUniversalTime();

            if (ef.id == 0)
                db.matches.Add(ef);

            return ef.id;
        }

    }
}
