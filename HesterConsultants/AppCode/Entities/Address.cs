using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text;

namespace HesterConsultants.AppCode.Entities
{
    public class Address
    {
        public int AddressId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public Address(int addressId, string addr1, string addr2, string city, string state, string postalCode, string country)
        {
            this.AddressId = addressId;
            this.Address1 = addr1;
            this.Address2 = addr2;
            this.City = city;
            this.State = state;
            this.PostalCode = postalCode;
            this.Country = country;
        }

        public Address(string addr1, string addr2, string city, string state, string postalCode, string country)
        {
            this.Address1 = addr1;
            this.Address2 = addr2;
            this.City = city;
            this.State = state;
            this.PostalCode = postalCode;
            this.Country = country;
        }

        public static List<Address> AllAddresses()
        {
            DataTable dtAllAddresses = ClientData.Current.AllAddressesDataTable();
            List<Address> allAddresses = new List<Address>();

            foreach (DataRow drAddress in dtAllAddresses.Rows)
            {
                //Address address = new Address();
                Address address = new Address
                (
                    (int)drAddress["AddressId"],
                    drAddress["Address1"].ToString(),
                    drAddress["Address2"].ToString(),
                    drAddress["City"].ToString(),
                    drAddress["State"].ToString(),
                    drAddress["PostalCode"].ToString(),
                    drAddress["Country"].ToString()
                );

                allAddresses.Add(address);
            }

            return allAddresses;
        }

        public string ConcatenatedFields()
        {
            return String.Concat(
                new string[] { this.Address1, this.Address2, this.City, this.State, this.PostalCode, this.Country });
        }

        public string AddressBlock(string lineBreak)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.Address1 + lineBreak);
            if (!String.IsNullOrEmpty(this.Address2))
                sb.Append(this.Address2 + lineBreak);
            sb.Append(this.City + lineBreak);
            sb.Append(this.State + " " + this.PostalCode + lineBreak);
            if (this.Country != null)
                sb.Append(this.Country);

            return sb.ToString();
        }

        // operators
        public override bool Equals(object obj)
        {
            try
            {
                return this.AddressId == ((Address)obj).AddressId;
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.AddressId.GetHashCode();
        }
   }
}