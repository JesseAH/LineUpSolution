using LineUp_API.Models;
using LineUpLibrary.DALs;
using LineUpLibrary.DTOs;
using Newtonsoft.Json;
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
    /// Leagues
    /// </summary>
    [Authorize]
    [RoutePrefix("api/League")]
    public class LeagueController : ApiController
    {
        LeagueDAL myDAL = new LeagueDAL();
        UserDAL myUserDAL = new UserDAL();

        /// <summary>
        /// Get list of all leagues
        /// </summary>
        /// <remarks>Get list of all leagues</remarks>
        /// <returns></returns>
        // GET api/League
        [ResponseType(typeof(IList<LeagueDTO>))]
        public IHttpActionResult Get()
        {
            try
            {
                return this.Ok(myDAL.GetList());
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Get a league by its id
        /// </summary>
        /// <remarks>Get a league by its id</remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/League/5
        [ResponseType(typeof(LeagueDTO))]
        public IHttpActionResult Get(int id)
        {
            try
            {
                return this.Ok(myDAL.Get(id));
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Get a list of all leagues that the current user is in
        /// </summary>
        /// <remarks>Get a list of all leagues that the current user is in</remarks>
        /// <returns></returns>
        [HttpGet]
        [System.Web.Http.Route("User")]
        [ResponseType(typeof(IList<LeagueDTO>))]
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
        /// Create a new league
        /// </summary>
        /// <remarks>Create a new league</remarks>
        /// <param name="dto"></param>
        /// <returns></returns>
        // POST api/<controller>
        [ResponseType(typeof(LeagueDTO))]
        public IHttpActionResult Post([FromBody]LeagueDTO dto)
        {
            try
            {
                return Content(HttpStatusCode.Created, myDAL.Create(dto));
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Edit an existing league
        /// </summary>
        /// <remarks>Edit an existing league</remarks>
        /// <param name="dto"></param>
        /// <returns></returns>
        // PUT api/<controller>
        public IHttpActionResult Put([FromBody]LeagueDTO dto)
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
        /// Add a new team to the a league
        /// </summary>
        /// <remarks>Add a new team to the a league</remarks>
        /// <param name="req"></param>
        /// <returns></returns>
        //// POST api/League/Join
        [Route("Join")]
        public IHttpActionResult Join(JoinLeagueRequest req)
        {
            try
            {
                if (myDAL.leagueAuthorization(req.league_id, req.password) == false)
                    return new StatusCodeResult(HttpStatusCode.Forbidden, Request);

                var id = myDAL.AddLeagueTeam(req.league_team_name, req.league_id, myUserDAL.GetUserID(User.Identity.Name));
                return new StatusCodeResult(HttpStatusCode.Created, Request);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message.ToString());
            }
        }

    }
}