using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using HesterConsultants.admin;

namespace HesterConsultants.AppCode.Entities
{
    public class Invoices
    {
        //private Invoices()
        //{
        //}

        //private class SingletonInstantiator
        //{
        //    // Explicit static constructor to tell C# compiler
        //    // not to mark type as beforefieldinit
        //    static SingletonInstantiator()
        //    {
        //    }

        //    internal static Invoices instance = new Invoices();
        //}

        //public static Invoices Current
        //{
        //    get
        //    {
        //        return SingletonInstantiator.instance;
        //    }
        //}

        public IList<Invoice> AllInvoicesForClient(Client client)
        {
            DataTable dtInvoices = ClientData.Current.InvoicesFromClientDataIdTable(client.ClientId, false, false);
            IList<Invoice> invoices = new List<Invoice>();

            foreach (DataRow drInvoice in dtInvoices.Rows)
            {
                Invoice invoice = new Invoice();
                invoice.SetFieldsFromDataRow(drInvoice);

                invoice.Client = client;

                invoices.Add(invoice);
            }

            return invoices;
        }

        public IList<Invoice> UnpaidInvoicesForClient(Client client)
        {
            DataTable dtInvoices = ClientData.Current.InvoicesFromClientDataIdTable(client.ClientId, true, false);
            IList<Invoice> invoices = new List<Invoice>();

            foreach (DataRow drInvoice in dtInvoices.Rows)
            {
                Invoice invoice = new Invoice();
                invoice.SetFieldsFromDataRow(drInvoice);

                invoice.Client = client;

                invoices.Add(invoice);
            }

            return invoices;
        }

        

        public List<Invoice> CurrentInvoiceForClient(Client client)
        {
            DataTable dtCurrentInvoice = ClientData.Current.InvoicesFromClientDataIdTable(client.ClientId, false, true);
            List<Invoice> currentInvoice = new List<Invoice>();

            if (dtCurrentInvoice.Rows.Count > 0)
            {
                Invoice invoice = new Invoice();
                invoice.SetFieldsFromDataRow(dtCurrentInvoice.Rows[0]);
                invoice.Client = client;
                currentInvoice.Add(invoice);
            }

            return currentInvoice;
        }

        //private decimal PastDueBalance(Client client)
        //{
        //    IList<Invoice> invoicesForClient = this.InvoicesForClient(client);
        //    decimal balance = 0m;

        //    foreach (Invoice invoice in invoicesForClient)
        //        balance += invoice.PastDueBalance;

        //    return balance;
        //}

        public Invoice CreateInvoice(Client client, DateTime invoiceDate, DateTime dateDue, IList<Job> jobs)
        {
            if (jobs.Count == 0)
                return null;

            Invoice invoice = new Invoice();
            invoice.Client = client;
            invoice.ClientId = client.ClientId;
            invoice.InvoiceDate = invoiceDate;
            invoice.DateDue = dateDue;
            invoice.Jobs = jobs;
            invoice.AmountPaid = 0m;
            invoice.DiscountAmount = 0m;
            invoice.DiscountRate = 1.0m;
            invoice.HasBeenSent = false;
            invoice.IsPaid = false;
            invoice.PaymentPending = false;

            decimal total = 0m;
            foreach (Job job in jobs)
                total += job.FinalCharge + job.Taxes; // assign invoice to job at client level

            invoice.AmountDue = total;

            int id = ClientData.Current.InsertInvoice(invoice.ClientId, invoice.InvoiceDate, invoice.DateDue, invoice.DiscountAmount, invoice.DiscountRate, invoice.AmountDue);

            if (id != 0)
            {
                invoice.InvoiceId = id;

                foreach (Job job in jobs)
                    job.SetInvoice(invoice);
            }

            return invoice;
        }

        public Invoice CreateCurrentInvoice(Client client, Employee curAdmin)
        {
            IList<Job> uninvoicedJobs = Job.UninvoicedJobsForClient(client);
            if (uninvoicedJobs.Count == 0)
                return null;

            DateTime nowForAdmin = AdminUtils.EmployeeDate(DateTime.UtcNow, curAdmin);

            DateTime invoiceDate = new DateTime(nowForAdmin.Year, nowForAdmin.Month, 1, 0, 0, 0); // first day of month
            DateTime dueDate = invoiceDate.AddMonths(1);

            return CreateInvoice(client, invoiceDate, dueDate, uninvoicedJobs);
        }

        public Invoice UpdateInvoice(Invoice invoice)
        {
            bool ret = ClientData.Current.UpdateInvoice(invoice.Client.ClientId, invoice.InvoiceDate, invoice.DateDue, invoice.DiscountRate, invoice.DiscountAmount, invoice.AmountDue, invoice.AmountPaid, invoice.HasBeenSent, invoice.PaymentPending, invoice.IsPaid, invoice.InvoiceId);

            if (!ret)
                return null;

            return invoice;
        }
    }
}