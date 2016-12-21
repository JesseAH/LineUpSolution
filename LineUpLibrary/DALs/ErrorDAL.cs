using LineUpLibrary.DTOs;
using LineUpLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineUpLibrary.DALs
{
    public class ErrorDAL
    {
        private Entities db = new Entities();


        public void ReportError(ErrorDTO dto, string ex, string message)
        {
            Error err = new Error();

            err.date = DateTime.Now.ToUniversalTime();
            err.controller = dto.controller;
            err.method = dto.method;
            err.inner_exception = ex;
            err.message = message;
            err.source = dto.source;
            err.user_id = dto.user_id;

            Create(err);
        }

        public void Create(Error error)
        {
            db.Errors.Add(error);
            db.SaveChanges();
        }
    }
}
