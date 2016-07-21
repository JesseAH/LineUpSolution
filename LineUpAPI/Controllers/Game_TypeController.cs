using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LineUpLibrary.DALs;
using LineUpLibrary.DTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace LineUpAPI.Controllers
{
    [Route("api/[controller]")]
    public class Game_TypeController : Controller
    {
        UserDAL myUserDAL = new UserDAL();
        Game_TypeDAL myGameDAL = new Game_TypeDAL();

        // GET: api/values
        [HttpGet]
        [Produces(typeof(IList<Game_TypeDTO>))]
        public IActionResult Get()
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

        // GET api/values/5
        [HttpGet("{id}")]
        [Produces(typeof(Game_TypeDTO))]
        public IActionResult Get(int id)
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


    }
}
