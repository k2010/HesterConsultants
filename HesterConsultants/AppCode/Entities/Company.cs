using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace HesterConsultants.AppCode.Entities
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }

        //public Company()
        //{
        //}

        public Company(int companyId, string name)
        {
            this.CompanyId = companyId;
            this.Name = name;
        }

        public Company(string name)
        {
            this.Name = name;
        }

        public static List<Company> AllCompanies()
        {
            DataTable dtAllCompanies = ClientData.Current.AllCompaniesDataTable();
            List<Company> allCompanies = new List<Company>();

            foreach (DataRow drCompany in dtAllCompanies.Rows)
            {
                Company company = new Company
                (
                    (int)drCompany["CompanyId"],
                    drCompany["CompanyName"].ToString()
                );

                allCompanies.Add(company);
            }

            return allCompanies;
        }

        // operators
        public override bool Equals(object obj)
        {
            try
            {
                return this.CompanyId == ((Company)obj).CompanyId;
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.CompanyId.GetHashCode();
        }
    }
}