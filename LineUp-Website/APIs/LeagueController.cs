using LineUp_Website.Models;
using LineUpLibrary.DALs;
using LineUpLibrary.DTOs;
using LineUpLibrary.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Braintree;

namespace LineUp_Website.APIs
{
    /// <summary>
    /// Leagues
    /// </summary>
    [Authorize]
    public class LeagueController : Controller
    {
        LeagueDAL myDAL = new LeagueDAL();
        UserDAL myUserDAL = new UserDAL();
        Game_TypeDAL myGameTypeDAL = new Game_TypeDAL();
        AuthMessageSender myMessageSender = new AuthMessageSender();
        ErrorDAL myErrorDAL = new ErrorDAL();
        ErrorDTO err = new ErrorDTO();
        public IBraintreeConfig config = new BraintreeConfig();

        public static readonly TransactionStatus[] transactionSuccessStatuses = {
                                                                                    TransactionStatus.AUTHORIZED,
                                                                                    TransactionStatus.AUTHORIZING,
                                                                                    TransactionStatus.SETTLED,
                                                                                    TransactionStatus.SETTLING,
                                                                                    TransactionStatus.SETTLEMENT_CONFIRMED,
                                                                                    TransactionStatus.SETTLEMENT_PENDING,
                                                                                    TransactionStatus.SUBMITTED_FOR_SETTLEMENT
                                                                                };

        public LeagueController()
        {
            err.controller = "League";
            err.source = "lineup-webapp";
        }

        /// <summary>
        /// Get list of all leagues
        /// </summary>
        /// <remarks>Get list of all leagues</remarks>
        /// <returns></returns>
        // GET api/League
        public JsonResult Index()
        {
            try
            {
                return this.Json(myDAL.GetList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                err.method = "Index";
                if(User != null)
                    err.user_id = myUserDAL.GetUserID(User.Identity.Name);
                myErrorDAL.ReportError(err,ex.Message.ToString(),"Error getting list");
                return null;
            }
        }

        public ActionResult Details(int id)
        {
            try
            {
                var league = myDAL.Get(id);
                if (myUserDAL.GetUserID(User.Identity.Name) == league.manager_id)
                    league.is_manager = true;

                var selrialized = this.Json(league, JsonRequestBehavior.AllowGet);
                return selrialized;
            }
            catch (Exception ex)
            {
                err.method = "Details";
                if (User != null)
                    err.user_id = myUserDAL.GetUserID(User.Identity.Name);
                myErrorDAL.ReportError(err, ex.Message, "Error getting details of league " + id);
                return null;
            }
        }

        public ActionResult BasicDetails(int id)
        {
            try
            {
                var league = myDAL.Get(id,false,false,false);
                if (myUserDAL.GetUserID(User.Identity.Name) == league.manager_id)
                    league.is_manager = true;

                var selrialized = this.Json(league, JsonRequestBehavior.AllowGet);
                return selrialized;
            }
            catch (Exception ex)
            {
                err.method = "Details";
                if (User != null)
                    err.user_id = myUserDAL.GetUserID(User.Identity.Name);
                myErrorDAL.ReportError(err, ex.Message, "Error getting details of league " + id);
                return null;
            }
        }

        public JsonResult GetLookupOptions()
        {
            try
            {
                var lookupOptions = new
                {
                    GameTypes = myGameTypeDAL.DTOGetAsLookupList(),
                    Leagues = myDAL.GetList()
                };

                return this.Json(lookupOptions, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                err.method = "GetLookupOptions";
                if (User != null)
                    err.user_id = myUserDAL.GetUserID(User.Identity.Name);
                myErrorDAL.ReportError(err, ex.Message.ToString(), "Error getting lookup options");
                return null;
            }
        }

        [HttpPost]
        public JsonResult Post(LeagueDTO dto)
        {
            try
            {
                LeagueDTO newLeague = myDAL.Create(dto, myUserDAL.GetUserID(User.Identity.Name));
                return this.Json(newLeague, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                err.method = "Post";
                if (User != null)
                    err.user_id = myUserDAL.GetUserID(User.Identity.Name);
                myErrorDAL.ReportError(err, ex.Message.ToString(), "Error saving");
                return null;
            }
        }

        [HttpPost]
        public JsonResult Join(JoinLeagueRequest req)
        {
            try
            {
                if (myDAL.leagueAuthorization(req.league_id, req.password) == false)
                    return this.Json(false, JsonRequestBehavior.AllowGet);

                var id = myDAL.AddLeagueTeam(req.league_team_name, req.league_id, myUserDAL.GetUserID(User.Identity.Name));

                return this.Json(id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                err.method = "Join";
                if (User != null)
                    err.user_id = myUserDAL.GetUserID(User.Identity.Name);
                myErrorDAL.ReportError(err, ex.Message.ToString(), "Error joining");
                return null;
            }
        }

        [HttpPost]
        public ActionResult JoinAndPay()
        {
            try
            {
                var gateway = config.GetGateway();
                var clientToken = gateway.ClientToken.generate();

                Decimal amount;

                try
                {
                    amount = Convert.ToDecimal(10);
                }
                catch (FormatException e)
                {
                    TempData["Flash"] = "Error: 81503: Amount is an invalid format.";
                    return null;
                }

                //var nonce = Request["payment_method_nonce"];
                var nonce = "fake-valid-nonce";

                var request = new TransactionRequest
                {
                    Amount = amount,
                    PaymentMethodNonce = nonce,
                    Options = new TransactionOptionsRequest
                    {
                        SubmitForSettlement = true
                    }
                };


                Result<Transaction> result = gateway.Transaction.Sale(request);
                if (result.IsSuccess())
                {
                    Transaction transaction = result.Target;

                    //TODO: store transaction info
                    //TODO: add team to league

                    //if (myDAL.leagueAuthorization(req.league_id, req.password) == false)
                    //    return this.Json(false, JsonRequestBehavior.AllowGet);

                    //var id = myDAL.AddLeagueTeam(req.league_team_name, req.league_id, myUserDAL.GetUserID(User.Identity.Name));


                    return Redirect("http://localhost:56999/user/team/list"); 
                }
                else if (result.Transaction != null)
                {
                    //return RedirectToAction("Show", new { id = result.Transaction.Id });
                }
                else
                {
                    string errorMessages = "";
                    foreach (ValidationError error in result.Errors.DeepAll())
                    {
                        errorMessages += "Error: " + (int)error.Code + " - " + error.Message + "\n";
                    }
                    TempData["Flash"] = errorMessages;
                    //return RedirectToAction("New");
                }

                return null;
            }
            catch (Exception ex)
            {
                err.method = "Join";
                if (User != null)
                    err.user_id = myUserDAL.GetUserID(User.Identity.Name);
                myErrorDAL.ReportError(err, ex.Message.ToString(), "Error joining");
                return null;
            }
        }

        [HttpPost]
        public async Task<JsonResult> Invite(InviteRequest req)
        {
            try
            {
                string[] emailList = req.emails.Split(',').Select(sValue => sValue.Trim()).ToArray();
                LeagueDTO league = myDAL.Get(req.league_id);

                // Loop emailList
                foreach (string e in emailList)
                {
                    await myMessageSender.SendEmailAsync(e, "LineUp: Invitation to join new league", "You are invited to join " + league.name + ": http://lineupconfidence.com/user/league/select");
                }

                return this.Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                err.method = "Invite";
                if (User != null)
                    err.user_id = myUserDAL.GetUserID(User.Identity.Name);
                myErrorDAL.ReportError(err, ex.Message.ToString(), "Error sending out invite emails");
                return this.Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Email(EmailRequest req)
        {
            try
            {
                LeagueDTO league = myDAL.Get(req.league_id);
                IList<user> users = myUserDAL.GetListOfLeague(req.league_id);

                // Loop users list
                foreach (user u in users)
                {
                    await myMessageSender.SendEmailAsync(u.email, req.subject, req.body);
                }

                return this.Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                err.method = "Email";
                if (User != null)
                    err.user_id = myUserDAL.GetUserID(User.Identity.Name);
                myErrorDAL.ReportError(err, ex.Message.ToString(), "Error sending out emails");
                return this.Json(false, JsonRequestBehavior.AllowGet);
            }
        }

    }

}
