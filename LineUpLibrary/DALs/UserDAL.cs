using LineUpLibrary.DTOs;
using LineUpLibrary.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineUpLibrary.DALs
{
    public class UserDAL
    {
        private Entities db = new Entities();

        public userDTO EFtoDTO(user ef)
        {
            userDTO dto = new userDTO();

            dto.id = ef.id;
            dto.first_name = ef.first_name;
            dto.last_name = ef.last_name;
            dto.username = ef.username;
            dto.email = ef.email;

            return dto;
        }

        public user DTOtoEF(userDTO dto)
        {
            user ef = new user();

            ef.id = dto.id;
            ef.first_name = dto.first_name;
            ef.last_name = dto.last_name;
            ef.username = dto.username;
            ef.email = dto.email;

            return ef;
        }


        public IEnumerable<userDTO> GetList()
        {

            var x = db.users.ToList();
            return db.users.Select(u => EFtoDTO(u)).AsEnumerable();
        }

        public IList<user> GetListOfLeague(int leagueID)
        {
            IList<user> users = db.league_team.Where(lt => lt.league_id == leagueID).Include(lt => lt.user).Select(lt => lt.user).ToList();
            return users;
        }

        public userDTO Get(int id)
        {

            var user = db.users.Where(u => u.id == id).FirstOrDefault();
            return EFtoDTO(user);
        }

        public userDTO Get(string identityEmail)
        {

            var user = db.users.Where(u => u.email == identityEmail).FirstOrDefault();
            return EFtoDTO(user);
        }

        public int GetUserID(string identityUsername)
        {
            var id = db.users.Where(u => u.email == identityUsername).First().id;
            return id;
        }

        public user Create(RegisterBindingModel model)
        {
            user newUser = new user()
            {
                email = model.Email,
                first_name = model.First_Name,
                last_name = model.Last_Name,
                username = model.Username,
                created_on = DateTime.Now.ToUniversalTime()
            };

            db.users.Add(newUser);

            db.SaveChanges();

            return newUser;
        }

        public int Create(userDTO dto)
        {
            user newUser = DTOtoEF(dto);
            newUser.created_on = DateTime.Now.ToUniversalTime();
            db.users.Add(newUser);
            db.SaveChanges();

            return newUser.id;
        }
    }
}
