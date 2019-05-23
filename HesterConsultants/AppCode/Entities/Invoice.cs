using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace HesterConsultants.AppCode.Entities
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public Client Client { get; set; } 
        public int ClientId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DateDue { get; set; }
        public IList<Job> Jobs { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal AmountDue { get; set; }
        public decimal AmountPaid { get; set; }
        public bool HasBeenSent { get; set; }
        public bool PaymentPending { get; set; }
        public bool IsPaid { get; set; }

        public Invoice()
        {
        }

        public Invoice(Client client, DateTime invoiceDate, IList<Job> jobs)
        {
            this.Client = client;
            this.InvoiceDate = invoiceDate;
            this.Jobs = jobs;
        }

        public void SetFieldsFromDataRow(DataRow drInvoice)
        {
            this.InvoiceId = (int)drInvoice["InvoiceId"];
            //this.Client = client;
            this.ClientId = (int)drInvoice["ClientId"];
            this.InvoiceDate = Convert.ToDateTime(drInvoice["InvoiceDate"]);
            this.DateDue = Convert.ToDateTime(drInvoice["DateDue"]);
            this.DiscountRate = Convert.ToDecimal(drInvoice["DiscountRate"]);
            this.DiscountAmount = Convert.ToDecimal(drInvoice["DiscountAmount"]);
            this.AmountDue = Convert.ToDecimal(drInvoice["AmountDue"]);
            this.AmountPaid = Convert.ToDecimal(drInvoice["AmountPaid"]);
            this.HasBeenSent = (bool)drInvoice["HasBeenSent"];
            this.PaymentPending = (bool)drInvoice["PaymentPending"];
            this.IsPaid = (bool)drInvoice["IsPaid"];

            this.Jobs = Job.JobsForInvoice(this);
        }

        public void MarkPaymentPending()
        {
            this.PaymentPending = true;
        }

        public void MarkPaid()
        {
            this.IsPaid = true;
        }

        public void ApplyAmountPaid(decimal amountPaid)
        {
            this.AmountPaid += amountPaid;
            this.AmountDue -= amountPaid;
        }

        public void ApplyDiscountRate(decimal rate)
        {
            this.DiscountRate = rate;
            this.AmountDue = this.AmountDue * rate;
        }

        public void ApplyDiscountAmount(decimal amount)
        {
            this.DiscountAmount = amount;
            this.AmountDue -= amount;
        }

        public void Send()
        {
        }
    }
}