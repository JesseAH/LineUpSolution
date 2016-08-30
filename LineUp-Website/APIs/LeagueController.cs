using LineUp_Website.Models;
using LineUpLibrary.DALs;
using LineUpLibrary.DTOs;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Web.Mvc;

namespace LineUp_Website.APIs
{
    /// <summary>
    /// Leagues
    /// </summary>
    [Authorize]
    public class LeagueController : Controller
    {
        LeagueDAL myDAL = new LeagueDAL();
        UserDAL myUserDAL = new UserDAL();
        Game_TypeDAL myGameTypeDAL = new Game_TypeDAL();

        /// <summary>
        /// Get list of all leagues
        /// </summary>
        /// <remarks>Get list of all leagues</remarks>
        /// <returns></returns>
        // GET api/League
        public JsonResult Index()
        {
            try
            {
                return this.Json(myDAL.GetList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult Details(int id)
        {
            try
            {
                var league = myDAL.Get(id);
                var selrialized = this.Json(league, JsonRequestBehavior.AllowGet);
                return selrialized;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public JsonResult GetLookupOptions()
        {
            try
            {
                var lookupOptions = new
                {
                    GameTypes = myGameTypeDAL.DTOGetAsLookupList(),
                    Leagues = myDAL.GetList()
                };

                return this.Json(lookupOptions, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult Post(LeagueDTO dto)
        {
            try
            {
                LeagueDTO newLeague = myDAL.Create(dto);
                return this.Json(newLeague, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult Join(JoinLeagueRequest req)
        {
            try
            {
                if (myDAL.leagueAuthorization(req.league_id, req.password) == false)
                    return this.Json(false, JsonRequestBehavior.AllowGet);

                var id = myDAL.AddLeagueTeam(req.league_team_name, req.league_id, myUserDAL.GetUserID(User.Identity.Name));

                return this.Json(id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }

}
