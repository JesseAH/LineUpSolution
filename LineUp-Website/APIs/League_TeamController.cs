using LineUpLibrary.DALs;
using LineUp_Website.Models;
using LineUpLibrary.DTOs;
using System;
using System.Net;
using System.Web.Mvc;

namespace LineUp_Website.APIs
{
    public class League_TeamController : Controller
    {
        League_TeamDAL myDAL = new League_TeamDAL();
        UserDAL myUserDAL = new UserDAL();

        // GET: api/League_Team
        public JsonResult Index()
        {
            try
            {
                return this.Json(myDAL.GetListByUser(myUserDAL.GetUserID(User.Identity.Name),true,false), JsonRequestBehavior.AllowGet);
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
                var team = myDAL.Get(id, true, true);

                if (team.user_id == myUserDAL.GetUserID(User.Identity.Name))
                    team.is_logged_in_users_team = true;

                var y = this.Json(team, JsonRequestBehavior.AllowGet);
                return y;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
