using LineUp_Website.Models;
using LineUpLibrary.DALs;
using LineUpLibrary.DTOs;
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

        public JsonResult Details(int id)
        {
            try
            {
                return this.Json(myDAL.Get(id, myUserDAL.GetUserID(User.Identity.Name)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }



    }
}
