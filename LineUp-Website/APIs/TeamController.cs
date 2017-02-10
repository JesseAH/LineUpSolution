using LineUp_Website.Models;
using LineUpLibrary.DALs;
using LineUpLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace LineUp_Website.APIs
{
    public class TeamController : Controller
    {
        TeamDAL myDAL = new TeamDAL();
        UserDAL myUserDAL = new UserDAL();
        ErrorDAL myErrorDAL = new ErrorDAL();
        ErrorDTO err = new ErrorDTO();

        // GET api/<controller>/5
        public JsonResult Details(int id)
        {
            try
            {
                return this.Json(myDAL.Get(id), JsonRequestBehavior.AllowGet);
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

        // POST api/<controller>
        [HttpPost]
        public JsonResult Post(TeamDTO dto)
        {
            try
            {
                var retId = myDAL.DTOSave(dto);

                return this.Json(retId);
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

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}