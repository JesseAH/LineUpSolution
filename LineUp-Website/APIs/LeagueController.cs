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
        TransactionDAL myTransactionDAL = new TransactionDAL();
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
                if (User != null)
                    err.user_id = myUserDAL.GetUserID(User.Identity.Name);
                myErrorDAL.ReportError(err, ex.Message.ToString(), "Error getting list");
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
                var league = myDAL.Get(id, false, false, false);
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

        public ActionResult JoinDetails(int id)
        {
            try
            {
                var league = myDAL.Get(id, false, false, false);

                //Braintree client token
                //Your server is responsible for generating the client token, which contains all of the necessary configuration information to set up the client SDKs.
                //When your server provides a client token to your client, it authenticates the application to communicate directly to Braintree.
                //Your client is responsible for obtaining the client token from your server and initializing the client SDK.If this succeeds, the client will generate a payment method nonce.
                var gateway = config.GetGateway();
                var clientToken = gateway.ClientToken.generate();

                var result = new
                {
                    league = league,
                    clientToken = clientToken
                };

                var selrialized = this.Json(result, JsonRequestBehavior.AllowGet);

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
                myErrorDAL.ReportError(err, ex.Message.ToString(), "Error creating");
                return null;
            }
        }

        [HttpPut]
        public JsonResult Put(LeagueDTO dto)
        {
            try
            {
                myDAL.Save(dto);
                return this.Json(dto, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                err.method = "Put";
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

                //Get values from form
                string teamName = Request["name"];
                string password = Request["password"];
                int leagueId = Convert.ToInt32(Request["id"]);
                decimal amount = Convert.ToDecimal(Request["amount"]);

                if (myDAL.leagueAuthorization(leagueId, password) == false)
                    return RedirectToAction("IncorrectPassword", "Manage");


                //Braintree nonce
                //The payment method nonce is a string returned by the client SDK to represent a payment method. 
                //This string is a reference to the customer payment method details that were provided in your payment form and should be sent to your server 
                //where it can be used with the server SDKs to create a new transaction request.
                var nonce = Request["payment_method_nonce"];

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

                    //Add team to league
                    int teamid = myDAL.AddLeagueTeam(teamName, leagueId, myUserDAL.GetUserID(User.Identity.Name));

                    //Store transaction info
                    PaymentDTO dto = new PaymentDTO();
                    dto.braintree_id = transaction.Id;
                    dto.user_id = myUserDAL.GetUserID(User.Identity.Name);
                    dto.league_team_id = teamid;
                    dto.payment_total = amount;
                    dto.braintree_payment_date = transaction.CreatedAt.ToString();
                    myTransactionDAL.DTOPaymentSave(dto);

                    //return Redirect("http://localhost:56999/user/team/details/" + teamid);
                    return Redirect("http://lineup-website-test.azurewebsites.net/user/team/details/" + teamid);
                    //return Redirect("http://lineupconfidence.com/user/team/details/" + teamid);
                }
                else if (result.Transaction != null)
                {
                    err.method = "JoinAndPay";
                    if (User != null)
                        err.user_id = myUserDAL.GetUserID(User.Identity.Name);
                    myErrorDAL.ReportError(err, result.Message, "Error Completing Transaction");
                    return RedirectToAction("TransactionDeclined", "Manage", new { message = result.Message });
                }
                else
                {
                    string errorMessages = "";
                    foreach (ValidationError error in result.Errors.DeepAll())
                    {
                        errorMessages += "Error: " + (int)error.Code + " - " + error.Message + "\n";
                    }

                    err.method = "JoinAndPay";
                    if (User != null)
                        err.user_id = myUserDAL.GetUserID(User.Identity.Name);
                    myErrorDAL.ReportError(err, errorMessages, "Error Completing Transaction");

                    return RedirectToAction("TransactionDeclined", "Manage", new { message = errorMessages });
                }
            }
            catch (Exception ex)
            {
                err.method = "JoinAndPay";
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
