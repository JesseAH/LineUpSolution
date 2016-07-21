using LineUpLibrary.DALs;
using LineUpLibrary.DTOs;
using LineUpLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace LineUp_API.Controllers
{
    [Authorize]
    [RoutePrefix("api/Game_Type")]
    public class Game_TypeController : ApiController
    {
        UserDAL myUserDAL = new UserDAL();
        Game_TypeDAL myGameDAL = new Game_TypeDAL();

        /// <summary>
        /// Get a game_type by its id
        /// </summary>
        /// <remarks>Get a game_type by its id</remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/League_Team/5
        [ResponseType(typeof(Game_TypeDTO))]
        public IHttpActionResult Get(int id)
        {
            try
            {
                return this.Ok(myGameDAL.Get(id));
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message.ToString());
            }
        }


        /// <summary>
        /// Get a list of all game_types
        /// </summary>
        /// <remarks>Get a list of all game_types</remarks>
        /// <returns></returns>
        // GET api/League_Team
        [ResponseType(typeof(IList<Game_TypeDTO>))]
        public IHttpActionResult Get()
        {
            try
            {
                return this.Ok(myGameDAL.GetList());
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message.ToString());
            }
        }

    }
}
