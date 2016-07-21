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
    /// Rounds belong to a Game Type (formerly known as weeks)
    /// </summary>
    [Authorize]
    [RoutePrefix("api/Round")]
    public class RoundController : ApiController
    {

        RoundDAL myDAL = new RoundDAL();
        UserDAL myUserDAL = new UserDAL();

        /// <summary>
        /// Get a Round by its id
        /// </summary>
        /// <remarks>Get a Round by its id</remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/Round/5
        [ResponseType(typeof(RoundDTO))]
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
        /// Get a list of all Rounds for a certain team
        /// </summary>
        /// <remarks>Get a list Rounds by a certain team</remarks>
        /// <param name="team_id"></param>
        /// <returns></returns>
        //// GET api/Pick/Team/{team_id}
        [HttpGet]
        [Route("Team/{team_id}")]
        [ResponseType(typeof(IList<RoundDTO>))]
        public IHttpActionResult Team(int team_id)
        {
            try
            {
                return this.Ok(myDAL.GetListByLeague(team_id));

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Get a list of all Rounds for a certain league
        /// </summary>
        /// <remarks>Get a list Rounds by a certain league</remarks>
        /// <param name="league_id"></param>
        /// <returns></returns>
        //// GET api/Pick/League/{league_id}
        [HttpGet]
        [Route("League/{league_id}")]
        [ResponseType(typeof(IList<RoundDTO>))]
        public IHttpActionResult League(int league_id)
        {
            try
            {
                return this.Ok(myDAL.GetListByLeague(league_id));

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Create a new Round
        /// </summary>
        /// <remarks>Create a new Round</remarks>
        /// <param name="dto"></param>
        /// <returns></returns>
        // POST api/Round
        public IHttpActionResult Post([FromBody]RoundDTO dto)
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
        /// Edit an existing Round
        /// </summary>
        /// <remarks>Edit an existing Round</remarks>
        /// <param name="dto"></param>
        /// <returns></returns>
        // PUT api/Round
        public IHttpActionResult Put([FromBody]RoundDTO dto)
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

    }
}
