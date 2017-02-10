using LineUpLibrary.DALs;
using LineUpLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LineUp_Website.APIs
{
    [Authorize]
    public class RoundController : Controller
    {
        RoundDAL myDAL = new RoundDAL();
        TeamDAL myTeamDAL = new TeamDAL();
        UserDAL myUserDAL = new UserDAL();
        ErrorDAL myErrorDAL = new ErrorDAL();
        MatchDAL myMatchDAL = new MatchDAL();
        ErrorDTO err = new ErrorDTO();

        // GET: Round/Details/5/10
        [Route("Round/Details/{roundID:int}/{leagueID:int}")]
        public JsonResult Details(int roundID, int leagueID)
        {
            try
            {
                return this.Json(myDAL.Get(roundID, null, leagueID), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public JsonResult Details(int id)
        {
            try
            {
                if (myDAL.RoundSecurityCheck(myUserDAL.GetUserID(User.Identity.Name), 0, id) == false)
                    return this.Json("Incorrect Permisions", JsonRequestBehavior.AllowGet);

                return this.Json(myDAL.Get(id, null, null), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public JsonResult Team(int id)
        {
            try
            {
                return this.Json(myDAL.GetListByUsersTeam(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // POST api/<controller>
        [HttpPost]
        public JsonResult Post(RoundDTO dto)
        {
            try
            {
                if (myDAL.RoundSecurityCheck(myUserDAL.GetUserID(User.Identity.Name), dto.game_type_id, dto.id) == false)
                    return this.Json("Incorrect Permisions", JsonRequestBehavior.AllowGet);

                int roundId = myDAL.Save(dto);

                myMatchDAL.PostMatches(dto.matches, roundId);

                return null;
            }
            catch (Exception ex)
            {
                err.method = "Post";
                if (User != null)
                    err.user_id = myUserDAL.GetUserID(User.Identity.Name);
                myErrorDAL.ReportError(err, ex.Message.ToString(), "Error saving pick");
                return null;
            }

        }

        public ActionResult Lookups(int id)
        {
            try
            {
                var lookupOptions = new
                {
                    Teams = myTeamDAL.GetByGame(id)
                };

                return this.Json(lookupOptions, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                err.method = "GetLookupOptions";
                if (User != null)
                    err.user_id = myUserDAL.GetUserID(User.Identity.Name);
                myErrorDAL.ReportError(err, ex.Message.ToString(), "Error getting lookup options");
                return null;
            }
        }

    }
}
