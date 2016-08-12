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

                LogEntry entry = new LogEntry
                {
                    LogDate = new DateTime(2009, 2, 15, 0, 0, 0, DateTimeKind.Utc),
                    Details = "Application started."
                };

                // default as of Json.NET 4.5
                string isoJson = JsonConvert.SerializeObject(entry);



                var x = myDAL.Get(id);
                var y = this.Json(x, JsonRequestBehavior.AllowGet);
                var z = JsonConvert.SerializeObject(x);
                return y;
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
                    Leagues = myDAL.GetSimpleList()
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

    public class LogEntry
    {
        public string Details { get; set; }
        public DateTime LogDate { get; set; }
    }
}
