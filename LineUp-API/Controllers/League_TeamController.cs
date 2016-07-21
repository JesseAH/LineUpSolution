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

        /// <summary>
        /// Get a team by its id
        /// </summary>
        /// <remarks>Get a team by its id</remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/League_Team/5
        [ResponseType(typeof(League_TeamDTO))]
        public IHttpActionResult Get(int id)
        {
            try
            {
                return this.Ok(myDAL.Get(id, myUserDAL.GetUserID(User.Identity.Name)));
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message.ToString());
            }
        }

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
                return this.BadRequest(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Get a list of all teams that the current user is in
        /// </summary>
        /// <remarks>Get a list of all teams that the current user is in</remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("User")]
        [ResponseType(typeof(IList<League_TeamDTO>))]
        public IHttpActionResult UserID()
        {
            try
            {
                return this.Ok(myDAL.GetListByUser(myUserDAL.GetUserID(User.Identity.Name)));

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message.ToString());
            }
        }

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
