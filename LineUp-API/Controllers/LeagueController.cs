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
        /// List of Leagues
        /// </summary>
        /// <remarks>List of Leagues</remarks>
        /// <returns></returns>
        // GET api/League
        [ResponseType(typeof(IList<LeagueDTO>))]
        public IHttpActionResult Get(bool getTeams = true, bool getTeamsCalculations = true, bool getRounds = true)
        {
            try
            {
                return this.Ok(myDAL.GetList(getTeams, getTeamsCalculations, getRounds));
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Get League
        /// </summary>
        /// <remarks>Get League</remarks>
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
        /// User's List of Leagues
        /// </summary>
        /// <remarks>User's List of Leagues</remarks>
        /// <returns></returns>
        [HttpGet]
        [System.Web.Http.Route("User")]
        [ResponseType(typeof(IList<LeagueDTO>))]
        public IHttpActionResult UserID(bool getTeams = true, bool getTeamsCalculations = true, bool getRounds = true)
        {
            try
            {
                return this.Ok(myDAL.GetListByUser(myUserDAL.GetUserID(User.Identity.Name), getTeams, getTeamsCalculations, getRounds));
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message.ToString());
            }
        }

        ///// <summary>
        ///// User's List of Leagues + Teams
        ///// </summary>
        ///// <remarks>User's List of Leagues + Teams</remarks>
        ///// <returns></returns>
        //[HttpGet]
        //[System.Web.Http.Route("User/Teams")]
        //[ResponseType(typeof(IList<LeagueDTO>))]
        //public IHttpActionResult UserTeams()
        //{
        //    try
        //    {
        //        return this.Ok(myDAL.GetListByUser(myUserDAL.GetUserID(User.Identity.Name), true, true, false));
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.BadRequest(ex.Message.ToString());
        //    }
        //}

        ///// <summary>
        ///// User's List of Leagues + Teams + Calculations
        ///// </summary>
        ///// <remarks>User's List of Leagues + Teams + Calculations</remarks>
        ///// <returns></returns>
        //[HttpGet]
        //[System.Web.Http.Route("User/Teams/Calculations")]
        //[ResponseType(typeof(IList<LeagueDTO>))]
        //public IHttpActionResult UserTeamsCalculations()
        //{
        //    try
        //    {
        //        return this.Ok(myDAL.GetListByUser(myUserDAL.GetUserID(User.Identity.Name), true, true, true));
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.BadRequest(ex.Message.ToString());
        //    }
        //}

        /// <summary>
        /// List of Leagues + Teams
        /// </summary>
        /// <remarks>List of Leagues + Teams</remarks>
        /// <returns></returns>
        [HttpGet]
        [System.Web.Http.Route("Teams")]
        [ResponseType(typeof(IList<LeagueDTO>))]
        public IHttpActionResult Teams(bool getTeamsCalculations = true, bool getRounds = true)
        {
            try
            {
                return this.Ok(myDAL.GetList(true, getTeamsCalculations, getRounds));
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message.ToString());
            }
        }

        ///// <summary>
        ///// List of Leagues + Teams + Calculations
        ///// </summary>
        ///// <remarks>List of Leagues + Teams + Calculations</remarks>
        ///// <returns></returns>
        //[HttpGet]
        //[System.Web.Http.Route("Teams/Calculations")]
        //[ResponseType(typeof(IList<LeagueDTO>))]
        //public IHttpActionResult TeamCalculations()
        //{
        //    try
        //    {
        //        return this.Ok(myDAL.GetList(true, true, false));
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.BadRequest(ex.Message.ToString());
        //    }
        //}

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