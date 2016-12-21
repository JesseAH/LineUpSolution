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
        ErrorDAL myErrorDAL = new ErrorDAL();
        ErrorDTO err = new ErrorDTO();

        public PickController()
        {
            err.controller = "Pick";
            err.source = "lineup-webapp";
        }

        public JsonResult Index()
        {
            try
            {
                return this.Json(myDAL.GetList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                err.method = "Index";
                if (User != null)
                    err.user_id = myUserDAL.GetUserID(User.Identity.Name);
                myErrorDAL.ReportError(err, ex.Message.ToString(), "Error getting list");
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
                err.method = "Teams";
                if (User != null)
                    err.user_id = myUserDAL.GetUserID(User.Identity.Name);
                myErrorDAL.ReportError(err, ex.Message.ToString(), "Error getting teams that are open for picking");
                return null;
            }
        }

        [HttpPost]
        public JsonResult Post(IList<PickDTO> dtos)
        {
            try
            {
                int myID = myUserDAL.GetUserID(User.Identity.Name);

                foreach (PickDTO pick in dtos)
                {
                    if (myDAL.IsMyTeam(pick.league_team_id, myID))
                    {
                        myDAL.DeleteDuplicatePicks(pick.league_team_id, pick.match_id);
                        myDAL.Create(pick);
                    }
                    else
                    {
                        return this.Json(false);
                    }
                }
                return this.Json(true);
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
    }
}
