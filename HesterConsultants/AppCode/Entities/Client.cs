using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using HesterConsultants.clients;

namespace HesterConsultants.AppCode.Entities
{
    public class Client
    {
        public int ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public Company Company { get; set; }
        internal int CompanyId { get; set; }
        public Address Address { get; set; }
        internal int AddressId { get; set; }
        public AuthenticationUser AuthUser { get; set; }
        internal Guid AuthUserId { get; set; }
        public string Email { get; set; } // get from UserName col in aspnet_Users
        public string TimeZoneId { get; set; }
        public Dictionary<string, string> OtherData { get; set; } // to do

        public Client()
        {
        }

        public Client(int clientId, string firstName, string lastName, string phone, Guid authUserId, string email, int companyId, int addressId, string timeZoneId)
        {
            this.ClientId = clientId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Phone = phone;
            this.Email = email;
            this.AuthUserId = authUserId;
            this.CompanyId = companyId;
            this.AddressId = addressId;
            this.TimeZoneId = timeZoneId;
        }

        //public Client(int clientId, string firstName, string lastName, string phone, Guid authUserId, string email, Company company, Address address, string timeZoneId)
        //{
        //    this.ClientId = clientId;
        //    this.FirstName = firstName;
        //    this.LastName = lastName;
        //    this.Phone = phone;
        //    this.AuthUserId = authUserId;
        //    this.Email = email;
        //    this.Company = company;
        //    this.CompanyId = company.CompanyId;
        //    this.Address = address;
        //    this.AddressId = address.AddressId;
        //    this.TimeZoneId = timeZoneId;
        //}

        public Client(string firstName, string lastName, string phone, Guid authUserId, string email, Company company, Address address, string timeZoneId)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Phone = phone;
            this.AuthUserId = authUserId;
            this.Email = email;
            this.Company = company;
            this.CompanyId = company.CompanyId;
            this.Address = address;
            this.AddressId = address.AddressId;
            this.TimeZoneId = timeZoneId;
        }

        public static List<Client> AllClients()
        {
            DataTable dtAllClients = ClientData.Current.AllClientsDataTable();
            List<Client> allClients = new List<Client>();

            foreach (DataRow drClient in dtAllClients.Rows)
            {
                Client client = new Client
                (
                    (int)drClient["ClientId"],
                    drClient["FirstName"].ToString(),
                    drClient["LastName"].ToString(),
                    drClient["Phone"].ToString(),
                    (Guid)drClient["AuthUserId"],
                    drClient["Email"].ToString(), // from join in db
                    (int)drClient["CompanyId"],
                    (int)drClient["AddressId"],
                    drClient["TimeZoneId"].ToString()
                );

                // get email

                allClients.Add(client);
            }

            return allClients;
        }

        /// <summary>
        /// Gets list of BalanceDue objects
        /// </summary>
        /// <returns></returns>
        //public List<BalanceDue> GetBalances()
        //{
        //    List<Invoice> unpaidInvoices = new Invoices().UnpaidInvoicesForClient(this);
        //    List<BalanceDue> balances = new List<BalanceDue>();

        //    DateTime dateForClient = ClientUtils.DateForClient(this, DateTime.UtcNow);

        //    // order invoices
        //    unpaidInvoices = unpaidInvoices.OrderByDescending(i => i.InvoiceDate).ToList();
            
        //    int k = 0;
        //    foreach (Invoice invoice in unpaidInvoices)
        //    {
        //        BalanceDue bd = new BalanceDue();
        //        bd.Invoice = invoice;
        //        //bd.InvoiceId = invoice.InvoiceId;
        //        bd.Amount = invoice.AmountDue;
        //        bd.DaysPast = (int)dateForClient.Subtract(invoice.DateDue).TotalDays;
        //        bd.InvoicesPast = k++;

        //        balances.Add(bd);
        //    }

        //    return balances;
        //}

        //private static Dictionary<string, string> OtherDataForClient(Client client)
        //{
        //}

        public Client ShallowCopy()
        {
            return (Client)this.MemberwiseClone();
        }

        // operators
        public override bool Equals(object obj)
        {
            try
            {
                return this.ClientId == ((Client)obj).ClientId;
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.ClientId.GetHashCode();
        }
    }
}