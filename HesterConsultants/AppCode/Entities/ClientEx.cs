using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace HesterConsultants.AppCode.Entities
{
    /// <summary>
    /// Composes Client with calculated fields.
    /// </summary>
    public class ClientEx 
    {
        public Client Client { get; set; }
        public int NumberOfJobs { get; set; }
        public decimal AmountDue { get; set; }

        public ClientEx()
        {
        }

        public ClientEx(Client client)
        {
            this.Client = client;
            this.NumberOfJobs = ClientData.Current.CountOfJobsForClient(client.ClientId);
            this.AmountDue = new Invoices().UnpaidInvoicesForClient(client).Sum(i => i.AmountDue);
        }

        public List<ClientEx> AllClientExes()
        {
            return AllClientExes(String.Empty);
        }

        public List<ClientEx> AllClientExes(string sortExpression = "")
        {
            List<Client> allClients = Client.AllClients();
            List<ClientEx> allClientExes = new List<ClientEx>();
            ObjectSortField osf = null;

            foreach (Client client in allClients)
            {
                ClientEx clientEx = new ClientEx(client);
                allClientExes.Add(clientEx);
            }

            if (!String.IsNullOrEmpty(sortExpression))
            {
                if (sortExpression.ToUpper().EndsWith(" DESC"))
                    osf = new ObjectSortField(sortExpression.Substring(0, sortExpression.Length - 5), ObjectSortField.SortOrders.Descending);
                else
                    osf = new ObjectSortField(sortExpression, ObjectSortField.SortOrders.Ascending);

                allClientExes.Sort(new ObjectComparer<ClientEx>(new ObjectSortField[] { osf }));
            }

            return allClientExes;
        }
    }
}