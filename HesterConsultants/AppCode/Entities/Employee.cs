using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Web.Security;

namespace HesterConsultants.AppCode.Entities
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public Address Address { get; set; }
        public int AddressId { get; set; }
        public string Phone { get; set; }
        public DateTime HireDate { get; set; }
        DateTime TerminationDate { get; set; }
        public AuthenticationUser AuthUser { get; set; }
        internal Guid AuthUserId { get; set; }
        public string Email { get; set; } // get from UserName col in aspnet_Users
        public string TimeZoneId { get; set; }

        private static List<Employee> allEmployees;

        // constructors
        public Employee()
        {
        }

        public Employee(string firstName, string lastName, string title, Address address, string phone, DateTime hireDate, Guid authUserId, string timeZoneId)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Title = title;
            this.Address = address;
            this.AddressId = address.AddressId;
            this.Phone = phone;
            this.HireDate = hireDate;
            this.AuthUserId = authUserId;
            this.TimeZoneId = timeZoneId;
        }

        // methods
        public static List<Employee> AllEmployees()
        {
            if (allEmployees == null)
                RefreshAllEmployees();

            return allEmployees;
        }

        private static void RefreshAllEmployees()
        {
            DataTable dtAllEmployees = ClientData.Current.AllEmployeesDataTable();
            allEmployees = new List<Employee>();

            foreach (DataRow drEmployee in dtAllEmployees.Rows)
            {
                Employee ee = new Employee();
                ee.SetFieldsFromDataRow(drEmployee);

                allEmployees.Add(ee);
            }
        }

        public static Employee EmployeeFromId(int id)
        {
            if (allEmployees == null)
                RefreshAllEmployees();

            return allEmployees.FirstOrDefault(e => e.EmployeeId == id);
        }

        public static Employee EmployeeFromAuthUserId(Guid authUserId)
        {
            if (allEmployees == null)
                RefreshAllEmployees();

            return allEmployees.FirstOrDefault(e => e.AuthUserId == authUserId);
        }

        private void SetFieldsFromDataRow(DataRow drEmployee)
        {
            this.EmployeeId = Convert.ToInt32(drEmployee["EmployeeId"]);
            this.FirstName = drEmployee["FirstName"].ToString();
            this.LastName = drEmployee["LastName"].ToString();
            this.Title = drEmployee["Title"].ToString();
            this.AddressId = (int)drEmployee["AddressId"];
            this.Phone = drEmployee["Phone"].ToString();
            this.HireDate = Convert.ToDateTime(drEmployee["HireDate"]);
            this.TerminationDate = (drEmployee["TerminationDate"] == System.DBNull.Value) ?
                DateTime.MinValue : Convert.ToDateTime(drEmployee["TerminationDate"]);
            this.AuthUserId = (Guid)drEmployee["AuthUserId"];
            this.TimeZoneId = drEmployee["TimeZoneId"].ToString();

            //this.Email = AuthenticationUser.AuthUserFromId(this.AuthUserId).UserName;
            // email from Membership
            MembershipUser mUser = Membership.GetUser(this.AuthUserId);
            this.Email = mUser.UserName;
        }

        public static bool InsertEmployee(Employee employee)
        {
            bool ret = false;

            int employeeId = ClientData.Current.InsertEmployee(employee.FirstName, employee.LastName, employee.Title, employee.Address.AddressId, employee.Phone, employee.HireDate, employee.AuthUserId, employee.TimeZoneId);

            if (employeeId != 0)
            {
                ret = true;
                RefreshAllEmployees();
            }

            return ret;
        }

        // operators
        public override bool Equals(object obj)
        {
            try
            {
                return this.EmployeeId == ((Employee)obj).EmployeeId;
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.EmployeeId.GetHashCode();
        }
    }
}