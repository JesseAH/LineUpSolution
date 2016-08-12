using LineUp_Website.Models;
using LineUpLibrary.DALs;
using LineUpLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace LineUp_Website.APIs
{
    [Authorize]
    public class PickController : Controller
    {
        PickDAL myDAL = new PickDAL();
        UserDAL myUserDAL = new UserDAL();

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

        public JsonResult Teams()
        {
            try
            {
                return this.Json(myDAL.GetTeamsWhoNeedToMakePicks(myUserDAL.GetUserID(User.Identity.Name)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult Post(IList<PickDTO> dtos)
        {
            try
            {
                foreach (PickDTO pick in dtos)
                {
                    myDAL.DeleteDuplicatePicks(pick.league_team_id, pick.match_id);
                    myDAL.Create(pick);
                }
                return this.Json(true);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
