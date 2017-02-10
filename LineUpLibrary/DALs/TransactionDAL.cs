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
    public class TransactionDAL
    {
        Entities db = new Entities();

        private PaymentDTO PaymentEFtoDTO(PaymentDTO dto, payment ef)
        {
            dto.id = ef.id;
            dto.user_id = ef.user_id;
            dto.league_team_id = ef.league_team_id;
            dto.payment_total = ef.payment_total;
            dto.paypal = ef.paypal;
            dto.braintree_payment_date = ef.braintree_payment_date;
            dto.created_on = ef.created_on;
            dto.league_team_name = ef.league_team == null ? null : ef.league_team.name;

            return dto;
        }

        private ResultDTO ResultEFtoDTO(ResultDTO dto, result ef)
        {
            dto.id = ef.id;
            dto.user_id = ef.user_id;
            dto.league_id = ef.league_id;
            dto.rank = ef.rank;
            dto.winnings = ef.winnings;
            dto.payment_sent = ef.payment_sent;
            dto.modified_on = ef.modified_on;
            dto.modified_by = ef.modified_by;
            dto.league_team_name = ef.league_team == null ? null : ef.league_team.name;
            dto.league_name = ef.league == null ? null : ef.league.name;

            return dto;
        }

        private payment PaymentDTOtoEF(PaymentDTO dto, payment ef)
        {
            ef.id = dto.id;
            ef.user_id = dto.user_id;
            ef.league_team_id = dto.league_team_id;
            ef.payment_total = dto.payment_total;
            ef.braintree_id = dto.braintree_id;
            ef.braintree_payment_date = dto.braintree_payment_date;

            return ef;
        }

        public PaymentDTO DTOPaymentSave(PaymentDTO dto)
        {
            payment ef = PaymentDTOtoEF(dto, new payment());
            ef.created_on = DateTime.UtcNow;

            db.payments.Add(ef);
            db.SaveChanges();

            return PaymentEFtoDTO(new PaymentDTO(), ef);
        }

        public IList<PaymentDTO> DTOGetPayments(int userID)
        {
            IList<payment> payments = db.payments.Where(p => p.user_id == userID).AsNoTracking().ToList();

            return payments.Select(p => PaymentEFtoDTO(new PaymentDTO(), p)).ToList();
        }

        public IList<ResultDTO> DTOGetResults(int userID)
        {
            IList<result> results = db.results.Where(r => r.user_id == userID).AsNoTracking().ToList();
            return results.Select(p => ResultEFtoDTO(new ResultDTO(), p)).ToList();
        }

    }
}
