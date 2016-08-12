using LineUpLibrary.DALs;
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
        UserDAL myUserDAL = new UserDAL();

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

    }
}
