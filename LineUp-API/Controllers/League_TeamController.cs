using LineUp_API.Models;
using LineUpLibrary.DALs;
using LineUpLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace LineUp_API.Controllers
{
    /// <summary>
    /// The Teams that make up a league.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/League_Team")]
    public class League_TeamController : ApiController
    {
        League_TeamDAL myDAL = new League_TeamDAL();
        UserDAL myUserDAL = new UserDAL();
        ErrorDAL myErrorDAL = new ErrorDAL();
        ErrorDTO err = new ErrorDTO();

        public League_TeamController()
        {
            err.controller = "League_Team";
            err.source = "lineup-api";
        }


        /// <summary>
        /// Get League Team
        /// </summary>
        /// <remarks>Get League Team</remarks>
        /// <returns></returns>
        // GET api/League_Team/5
        [HttpGet]
        [Route("{id}")]
        [ResponseType(typeof(League_TeamDTO))]
        public IHttpActionResult Get(int id, bool getCalculations = true, bool getRounds = true)
        {
            try
            {
                return this.Ok(myDAL.Get(id, getCalculations, getRounds));
            }
            catch (Exception ex)
            {
                err.method = "Get(" + id + "," + getCalculations + "," + getRounds + ")";
                if (User != null)
                    err.user_id = myUserDAL.GetUserID(User.Identity.Name);
                myErrorDAL.ReportError(err, ex.Message.ToString(), "Error getting league team record");
                return this.BadRequest(ex.Message.ToString());
            }
        }

        ///// <summary>
        ///// Get League Team + Calculations
        ///// </summary>
        ///// <remarks>Get League Team + Calculations</remarks>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("{id}/Calculations")]
        //[ResponseType(typeof(League_TeamDTO))]
        //public IHttpActionResult Calculations(int id)
        //{
        //    try
        //    {
        //        return this.Ok(myDAL.Get(id, true, false));
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.BadRequest(ex.Message.ToString());
        //    }
        //}

        ///// <summary>
        ///// Get League Team + Calculations + Rounds
        ///// </summary>
        ///// <remarks>Get League Team + Calculations + Rounds</remarks>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("{id}/Calculations/Rounds")]
        //[ResponseType(typeof(League_TeamDTO))]
        //public IHttpActionResult Rounds(int id)
        //{
        //    try
        //    {
        //        return this.Ok(myDAL.Get(id, true, true));
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.BadRequest(ex.Message.ToString());
        //    }
        //}

        /// <summary>
        /// Create a new team
        /// </summary>
        /// <remarks>Create a new team</remarks>
        /// <param name="dto"></param>
        /// <returns></returns>
        // POST api/League_Team
        public IHttpActionResult Post([FromBody]League_TeamDTO dto)
        {
            try
            {
                myDAL.Create(dto);
                return new StatusCodeResult(HttpStatusCode.Created, Request);
            }
            catch (Exception ex)
            {
                err.method = "Post";
                if (User != null)
                    err.user_id = myUserDAL.GetUserID(User.Identity.Name);
                myErrorDAL.ReportError(err, ex.Message.ToString(), "Error creating new record");
                return this.BadRequest(ex.Message.ToString());
            }
        }

        /// <summary>
        /// User's List of League Teams
        /// </summary>
        /// <remarks>User's List of League Teams</remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("User")]
        [ResponseType(typeof(IList<League_TeamDTO>))]
        public IHttpActionResult UserID(bool getCalculations = true, bool getRounds = true)
        {
            try
            {
                return this.Ok(myDAL.GetListByUser(myUserDAL.GetUserID(User.Identity.Name), getCalculations, getRounds));

            }
            catch (Exception ex)
            {
                err.method = "UserID(" + getCalculations + "," + getRounds + ")";
                if (User != null)
                    err.user_id = myUserDAL.GetUserID(User.Identity.Name);
                myErrorDAL.ReportError(err, ex.Message.ToString(), "Error getting users list");
                return this.BadRequest(ex.Message.ToString());
            }

        }

        ///// <summary>
        ///// User's List of League Teams + Calculations
        ///// </summary>
        ///// <remarks>User's List of League Teams + Calculations</remarks>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("User/Calculations")]
        //[ResponseType(typeof(IList<League_TeamDTO>))]
        //public IHttpActionResult UserCalculations()
        //{
        //    try
        //    {
        //        return this.Ok(myDAL.GetListByUser(myUserDAL.GetUserID(User.Identity.Name),true,false));

        //    }
        //    catch (Exception ex)
        //    {
        //        return this.BadRequest(ex.Message.ToString());
        //    }
        //}

        ///// <summary>
        ///// User's List of League Teams + Calculations + Rounds
        ///// </summary>
        ///// <remarks>User's List of League Teams + Calculations + Rounds</remarks>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("User/Calculations/Rounds")]
        //[ResponseType(typeof(IList<League_TeamDTO>))]
        //public IHttpActionResult UserRounds()
        //{
        //    try
        //    {
        //        return this.Ok(myDAL.GetListByUser(myUserDAL.GetUserID(User.Identity.Name),true,true));

        //    }
        //    catch (Exception ex)
        //    {
        //        return this.BadRequest(ex.Message.ToString());
        //    }
        //}

        /// <summary>
        /// Edit an existing team
        /// </summary>
        /// <remarks>CEdit an existing team</remarks>
        /// <param name="dto"></param>
        /// <returns></returns>
        // PUT api/League_Team
        public IHttpActionResult Put([FromBody]League_TeamDTO dto)
        {
            try
            {
                myDAL.Save(dto);
                return this.Ok();
            }
            catch (Exception ex)
            {
                err.method = "Put";
                if (User != null)
                    err.user_id = myUserDAL.GetUserID(User.Identity.Name);
                myErrorDAL.ReportError(err, ex.Message.ToString(), "Error saving league team");
                return this.BadRequest(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Delete a team by its id
        /// </summary>
        /// <remarks>Delete a team by its id</remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE api/League_Team/5
        //public IHttpActionResult Delete(int id)
        //{
        //    try
        //    {
        //        myDAL.Delete(id);
        //        return this.Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.BadRequest(ex.Message.ToString());
        //    }
        //}
    }
}
