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
    [Authorize]
    [RoutePrefix("api/Pick")]
    public class PickController : ApiController
    {
        PickDAL myDAL = new PickDAL();
        UserDAL myUserDAL = new UserDAL();

        /// <summary>
        /// Get a pick by its id
        /// </summary>
        /// <remarks>Get a pick by its id</remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/Pick/5
        [ResponseType(typeof(PickDTO))]
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
        /// Get a list of all picks for a certain team
        /// </summary>
        /// <remarks>Get a list of all picks for a certain team</remarks>
        /// <param name="team_id"></param>
        /// <returns></returns>
        //// GET api/Pick/Team/{team_id}
        [HttpGet]
        [Route("Team/{team_id}")]
        [ResponseType(typeof(IList<PickDTO>))]
        public IHttpActionResult Team(int team_id)
        {
            try
            {
                return this.Ok(myDAL.GetListByTeam(team_id));

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Create a new pick
        /// </summary>
        /// <remarks>Create a new pick</remarks>
        /// <param name="dto"></param>
        /// <returns></returns>
        // POST api/Pick
        public IHttpActionResult Post([FromBody]PickDTO dto)
        {
            try
            {
                //if(!myDAL.Pick_Open(dto.match_id))
                //    return this.BadRequest("This match is no longer open to make picks.");

                int id = myDAL.Create(dto);
                return Content(HttpStatusCode.Created, id);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Edit an existing pick
        /// </summary>
        /// <remarks>Edit an existing pick</remarks>
        /// <param name="dto"></param>
        /// <returns></returns>
        // PUT api/Pick
        public IHttpActionResult Put([FromBody]PickDTO dto)
        {
            try
            {
                //if (!myDAL.Pick_Open(dto.match_id))
                //    return this.BadRequest("This match is no longer open to make picks.");

                myDAL.Save(dto);
                return this.Ok();
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Delete a pick by its id
        /// </summary>
        /// <remarks>Delete a pick by its id</remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE api/Pick/5
        public IHttpActionResult Delete(int id)
        {
            try
            {
                myDAL.Delete(id);
                return this.Ok();
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message.ToString());
            }
        }
    }
}
