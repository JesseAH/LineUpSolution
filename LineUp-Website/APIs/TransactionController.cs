using LineUpLibrary.DALs;
using LineUpLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LineUp_Website.APIs
{
    public class TransactionController : Controller
    {
        TransactionDAL myDAL = new TransactionDAL();
        UserDAL myUserDAL = new UserDAL();
        ErrorDAL myErrorDAL = new ErrorDAL();
        ErrorDTO err = new ErrorDTO();

        public ActionResult Account()
        {
            try
            {
                var payments = myDAL.DTOGetPayments(myUserDAL.GetUserID(User.Identity.Name));
                var results = myDAL.DTOGetResults(myUserDAL.GetUserID(User.Identity.Name));

                var selrialized = this.Json(new
                {
                    payments = payments,
                    total_winnings = results.Sum(r => r.winnings),
                    unpaid_winnings = results.Where(r => r.payment_sent != true).ToList().Sum(r => r.winnings),
                    payouts = string.Empty
                }, JsonRequestBehavior.AllowGet);
                return selrialized;
            }
            catch (Exception ex)
            {
                err.method = "Account";
                if (User != null)
                    err.user_id = myUserDAL.GetUserID(User.Identity.Name);
                myErrorDAL.ReportError(err, ex.Message, "Error getting account information");
                return null;
            }
        }

    }
}