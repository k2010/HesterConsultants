using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using HesterConsultants.Properties;
using System.Text;
using System.Net.Mail;

namespace HesterConsultants.AppCode
{
    /// <summary>
    /// Database access class (SQL Server)
    /// </summary>
    public sealed class ClientData
    {
        // to do - make a conn string for clients with limited permissions
        // to do - encrypt conn string in web.config
        private string clientConnectionString;
        //private string adminConnectionString;

        //// db constants
        // move to config file
        //public static int JobStatusCompletedId = 6;

        private ClientData()
        {
			this.clientConnectionString = null; // 2014 // HesterConsultants.Properties.Settings.Default.ConnString_Clients;
        }

        private class SingletonInstantiator
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static SingletonInstantiator()
            {
            }

            internal static ClientData instance = new ClientData();
        }

        /// <summary>
        /// Returns the sole instance of ClientData.
        /// </summary>
        public static ClientData Current
        {
            get
            {
                return SingletonInstantiator.instance;
            }
        }

        #region clients

        public DataTable AllClientsDataTable()
        {
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT C.*, U.UserName AS Email FROM Clients AS C INNER JOIN aspnet_Users AS U "
                + "ON C.AuthUserId = U.UserId;";

            SqlDataAdapter daClients = new SqlDataAdapter(cmd);
            DataTable dtClients = new DataTable();

            try
            {
                daClients.Fill(dtClients);
            }
            catch (Exception ex)
            {
                LogErrorAndSendAlert(ex);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return dtClients;
        }

        public int InsertClient(string firstName, string lastName, string phone, Guid authUserId, DateTime startDate, int companyId, int addressId, string timeZoneId)
        {
            int clientId = 0;

            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "InsertClient";

            cmd.Parameters.AddWithValue("@FirstName", firstName);
            cmd.Parameters.AddWithValue("@LastName", lastName);
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.Parameters.AddWithValue("@AuthUserId", authUserId);
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@CompanyId", companyId);
            cmd.Parameters.AddWithValue("@AddressId", addressId);
            cmd.Parameters.AddWithValue("@TimeZoneId", timeZoneId);

            SqlParameter parm = new SqlParameter();
            parm.ParameterName = "@ClientId";
            parm.SqlDbType = SqlDbType.Int; 
            parm.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parm);

            conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            cmd.Transaction = trans;

            try
            {
                cmd.ExecuteNonQuery();
                clientId = (int)cmd.Parameters["@ClientId"].Value;
                trans.Commit();
                conn.Close();
            }
            catch (Exception ex1)
            {
                LogErrorAndSendAlert(ex1);
                try
                {
                    trans.Rollback();
                }
                catch (Exception ex2)
                {
                    LogErrorAndSendAlert(ex2);
                }
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return clientId;
        }

        public bool UpdateClient(string firstName, string lastName, string phone, Guid authUserId, int companyId, int addressId, string timeZoneId, int clientId)
        {
            bool ret = false;
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "UpdateClient";

            cmd.Parameters.AddWithValue("@FirstName", firstName);
            cmd.Parameters.AddWithValue("@LastName", lastName);
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.Parameters.AddWithValue("@AuthUserId", authUserId);
            cmd.Parameters.AddWithValue("@CompanyId", companyId);
            cmd.Parameters.AddWithValue("@AddressId", addressId);
            cmd.Parameters.AddWithValue("@TimeZoneId", timeZoneId);
            cmd.Parameters.AddWithValue("@ClientId", clientId);

            conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            cmd.Transaction = trans;

            try
            {
                cmd.ExecuteNonQuery();
                trans.Commit();
                ret = true;
                conn.Close();
            }
            catch (Exception ex1)
            {
                LogErrorAndSendAlert(ex1);
                try
                {
                    trans.Rollback();
                }
                catch (Exception ex2)
                {
                    LogErrorAndSendAlert(ex2);
                }
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return ret;
       }

        #endregion

        #region authusers

        public DataTable AllAuthUsersDataTable()
        {
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM AuthenticationUsers;";

            SqlDataAdapter daAuthUsers = new SqlDataAdapter(cmd);
            DataTable dtAuthUsers = new DataTable();

            try
            {
                daAuthUsers.Fill(dtAuthUsers);
            }
            catch (Exception ex)
            {
                LogErrorAndSendAlert(ex);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return dtAuthUsers;
        }

        public DataRow AuthUserFromIdDataRow(Guid id)
        {
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM aspnet_Users WHERE UserId = @AuthUserId;";
            cmd.Parameters.AddWithValue("@AuthUserId", id);
            
            SqlDataAdapter daAuthUsers = new SqlDataAdapter(cmd);
            DataTable dtAuthUsers = new DataTable();

            conn.Open();
            try
            {
                daAuthUsers.Fill(dtAuthUsers);
                if (dtAuthUsers.Rows.Count > 0)
                    return dtAuthUsers.Rows[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                LogErrorAndSendAlert(ex);
                return null;
            }

            finally
            {
                if (conn.State != ConnectionState.Open)
                    conn.Close();
            }
        }

        #endregion

        #region companies

        public DataTable AllCompaniesDataTable()
        {
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Companies;";

            SqlDataAdapter daCompanies = new SqlDataAdapter(cmd);
            DataTable dtCompanies = new DataTable();

            try
            {
                daCompanies.Fill(dtCompanies);
            }
            catch (Exception ex)
            {
                LogErrorAndSendAlert(ex);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return dtCompanies;
        }

        public int InsertCompany(string name)
        {
            int companyId = 0;

            if (string.IsNullOrEmpty(name))
            {
                throw new InvalidOperationException("Company Name is required.");
                // to do - log it
            }

            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "InsertCompany";

            cmd.Parameters.AddWithValue("@CompanyName", name);

            SqlParameter parm = new SqlParameter();
            parm.ParameterName = "@CompanyId";
            parm.DbType = DbType.Int32;
            parm.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parm);

            conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            cmd.Transaction = trans;

            try
            {
                cmd.ExecuteNonQuery();
                companyId = (int)cmd.Parameters["@CompanyId"].Value;
                trans.Commit();
                conn.Close();
            }
            catch (Exception ex1)
            {
                LogErrorAndSendAlert(ex1);
                try
                {
                    trans.Rollback();
                }
                catch (Exception ex2)
                {
                    LogErrorAndSendAlert(ex2);
                }
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return companyId;
        }

        public bool UpdateCompany(string name, int companyId)
        {
            bool ret = false;
            if (string.IsNullOrEmpty(name))
            {
                throw new InvalidOperationException("Company Name is required.");
                // to do - log it
            }

            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "InsertCompany";

            cmd.Parameters.AddWithValue("@CompanyName", name);
            cmd.Parameters.AddWithValue("@CompanyId", companyId);

            conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            cmd.Transaction = trans;

            try
            {
                cmd.ExecuteNonQuery();
                trans.Commit();
                ret = true;
                conn.Close();
            }
            catch (Exception ex1)
            {
                LogErrorAndSendAlert(ex1);
                try
                {
                    trans.Rollback();
                }
                catch (Exception ex2)
                {
                    LogErrorAndSendAlert(ex2);
                }
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return ret;
        }
        #endregion  

        #region addresses

        public DataTable AllAddressesDataTable()
        {
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Addresses;";

            SqlDataAdapter daAddresses = new SqlDataAdapter(cmd);
            DataTable dtAddresses = new DataTable();

            try
            {
                daAddresses.Fill(dtAddresses);
            }
            catch (Exception ex)
            {
                LogErrorAndSendAlert(ex);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return dtAddresses;
        }

        public int InsertAddress(string addr1, string addr2, string city, string state, string postalCode, string country)
        {
            int addressId = 0;

            if (String.IsNullOrEmpty(addr1)
                    ||
                    String.IsNullOrEmpty(city)
                    ||
                    String.IsNullOrEmpty(state)
                    ||
                    String.IsNullOrEmpty(postalCode))
            {
                //return addressId; // 0
                throw new InvalidOperationException("Missing one or more required fields: Address1, City, State/Province and Zip/Postal Code are all required.");
                // to do - log it
            }

            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "InsertAddress";

            cmd.Parameters.AddWithValue("@Address1", addr1);

            SqlParameter parm = new SqlParameter();
            parm.ParameterName = "@Address2";
            parm.SqlDbType = SqlDbType.NVarChar;
            parm.Size = 100;
            if (!String.IsNullOrEmpty(addr2))
                parm.Value = addr2;
            else
                parm.Value = DBNull.Value;
            cmd.Parameters.Add(parm);

            cmd.Parameters.AddWithValue("@City", city);
            cmd.Parameters.AddWithValue("@State", state);
            cmd.Parameters.AddWithValue("@PostalCode", postalCode);

            parm = new SqlParameter();
            parm.ParameterName = "@Country";
            parm.SqlDbType = SqlDbType.NVarChar;
            parm.Size = 100;
            if (!String.IsNullOrEmpty(country))
                parm.Value = country;
            else
                parm.Value = DBNull.Value;
            cmd.Parameters.Add(parm);

            parm = new SqlParameter();
            parm.ParameterName = "@AddressId";
            parm.DbType = DbType.Int32;
            parm.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parm);

            conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            cmd.Transaction = trans;

            try
            {
                cmd.ExecuteNonQuery();
                addressId = (int)cmd.Parameters["@AddressId"].Value;
                trans.Commit();
                conn.Close();
            }
            catch (Exception ex1)
            {
                LogErrorAndSendAlert(ex1);
                try
                {
                    trans.Rollback();
                }
                catch (Exception ex2)
                {
                    LogErrorAndSendAlert(ex2);
                }
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return addressId;
        }

        public bool UpdateAddress(string addr1, string addr2, string city, string state, string postalCode, string country, int addressId)
        {
            bool ret = false;

            if (String.IsNullOrEmpty(addr1)
                    ||
                    String.IsNullOrEmpty(city)
                    ||
                    String.IsNullOrEmpty(state)
                    ||
                    String.IsNullOrEmpty(postalCode))
            {
                //return addressId; // 0
                throw new InvalidOperationException("Missing one or more required fields: Address1, City, State/Province and Zip/Postal Code are all required.");
                // to do - log it
            }

            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "UpdateAddress";

            cmd.Parameters.AddWithValue("@Address1", addr1);

            SqlParameter parm = new SqlParameter();
            parm.ParameterName = "@Address2";
            parm.SqlDbType = SqlDbType.NVarChar;
            parm.Size = 100;
            if (!String.IsNullOrEmpty(addr2))
                parm.Value = addr2;
            else
                parm.Value = DBNull.Value;
            cmd.Parameters.Add(parm);

            cmd.Parameters.AddWithValue("@City", city);
            cmd.Parameters.AddWithValue("@State", state);
            cmd.Parameters.AddWithValue("@PostalCode", postalCode);

            parm = new SqlParameter();
            parm.ParameterName = "@Country";
            parm.SqlDbType = SqlDbType.NVarChar;
            parm.Size = 100;
            if (!String.IsNullOrEmpty(country))
                parm.Value = country;
            else
                parm.Value = DBNull.Value;
            cmd.Parameters.Add(parm);

            cmd.Parameters.AddWithValue("@AddressId", addressId);

            conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            cmd.Transaction = trans;

            try
            {
                cmd.ExecuteNonQuery();
                trans.Commit();
                ret = true;
                conn.Close();
            }
            catch (Exception ex1)
            {
                LogErrorAndSendAlert(ex1);
                try
                {
                    trans.Rollback();
                }
                catch (Exception ex2)
                {
                    LogErrorAndSendAlert(ex2);
                }
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return ret;
        }

        #endregion

        #region newusers

        public Guid InsertNewClientToken(Guid authUserId)
        {
            Guid token = new Guid();

            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "InsertNewClientToken";

            cmd.Parameters.AddWithValue("@AuthUserId", authUserId);
            
            conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            cmd.Transaction = trans;

            try
            {
                token = (Guid)cmd.ExecuteScalar();
                trans.Commit();
                conn.Close();
            }
            catch (Exception ex1)
            {
                LogErrorAndSendAlert(ex1);
                try
                {
                    trans.Rollback();
                }
                catch (Exception ex2)
                {
                    LogErrorAndSendAlert(ex2);
                }
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return token;
        }

        public string GetNewClientEmailFromToken(Guid token)
        {
            string email = String.Empty;

            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetNewClientEmailFromToken";

            cmd.Parameters.AddWithValue("@Token", token);

            conn.Open();
            try
            {
                email = cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                LogErrorAndSendAlert(ex);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return email;
        }

        public bool VerifyNewClientToken(Guid authUserId, Guid token)
        {
            // if no token, return false
            if (token == null)
                return false;

            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "VerifyNewClientToken";

            cmd.Parameters.AddWithValue("@AuthUserId", authUserId);
            cmd.Parameters.AddWithValue("@Token", token);

            int ret = 0;

            conn.Open();

            try
            {
                ret = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                // could have exception converting to uniqueidentifier
                LogErrorAndSendAlert(ex);
                ret = 0;
                // log
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return ret > 0;
        }

        public bool VerifyNewClientIsAuthenticated(Guid authUserId)
        {
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CheckNewUserIsAuthenticated";

            cmd.Parameters.AddWithValue("@AuthUserId", authUserId);

            bool ret = false;

            conn.Open();

            try
            {
                Debug.WriteLine("ClientData.VerifyNewClientIsAuthenticated(guid): " + cmd.ExecuteScalar().ToString());
                ret = (bool)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                // could have exception converting to uniqueidentifier
                LogErrorAndSendAlert(ex);
                ret = false;
                // log
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return ret;
        }

        #endregion

        #region jobs

        public DataTable RecentJobsDataTable(DateTime dt)
        {
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "OpenAndRecentJobs";
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Jobs WHERE (IsArchived = 0) AND (DateCompleted IS NULL OR DATEDIFF(day, DateCompleted, @Date) <= " + Settings.Default.RecentJobsNumberOfDays + ");";

            cmd.Parameters.AddWithValue("@Date", dt);

            SqlDataAdapter daRecentJobs = new SqlDataAdapter(cmd);
            DataTable dtRecentJobs = new DataTable();

            try
            {
                Debug.WriteLine("ClientData.RecentJobsDataTable() before fill");
                daRecentJobs.Fill(dtRecentJobs);
                Debug.WriteLine("ClientData.RecentJobsDataTable() filled");
            }
            catch (Exception ex)
            {
                LogErrorAndSendAlert(ex);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return dtRecentJobs;
        }

        public DataTable JobsFromInvoiceIdDataTable(int invoiceId)
        {
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Jobs WHERE InvoiceId = @InvoiceId;";

            cmd.Parameters.AddWithValue("@InvoiceId", invoiceId);

            SqlDataAdapter daJobsForInvoice = new SqlDataAdapter(cmd);
            DataTable dtJobsForInvoice = new DataTable();

            try
            {
                daJobsForInvoice.Fill(dtJobsForInvoice);
            }
            catch (Exception ex)
            {
                LogErrorAndSendAlert(ex);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return dtJobsForInvoice;
        }

        public DataTable JobsFromSearchTermDataTable(int clientId, IList<string> terms, bool isAdmin)
        {
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            if (terms.Count == 0)
                return null;

            List<int> jobIds = new List<int>();

            // see if user is searching for any job numbers
            foreach (string term in terms)
            {
                // if the search term is a positive int,
                // put it in jobIds (but also keep it in string search terms)
                int num = 0;
                bool isNum = Int32.TryParse(term, out num);
                if (num > 0)
                    jobIds.Add(num);
            }

            StringBuilder sbSql = new StringBuilder();

            int k = 0;
            bool hasJobIds = jobIds.Count > 0;

            if (hasJobIds)
            {
                // SearchType is to distinguish jobId searches (more relevant)
                // from text searches
                sbSql.Append("SELECT *, 0 AS SearchType FROM Jobs AS J");
                sbSql.Append(" LEFT JOIN JobFiles AS JF ON J.JobId = JF.JobId");
                sbSql.Append(" WHERE (");

                if (!isAdmin)
                {
                    sbSql.Append("J.ClientId = @ClientId) AND (");
                    sbSql.Append("JF.JobFileId IS NULL OR JF.IsReturnFile = 1 OR JF.IsSubmittedFile = 1) AND (");
                }

                foreach (int jobId in jobIds)
                {
                    if (k > 0)
                        sbSql.Append(" OR ");

                    sbSql.Append("J.JobId = @JobId" + k++.ToString());
                }

                sbSql.Append(")");

                // do a UNION if any ints
                // if we're doing a jobid, we're doing both kinds
                // because we'll search jobid and instructions for a jobid
                sbSql.Append(" UNION ");
            }

            // string searches
            sbSql.Append("SELECT *, 1 AS SearchType FROM Jobs AS J");
            sbSql.Append(" LEFT JOIN JobFiles AS JF ON J.JobId = JF.JobId");
            sbSql.Append(" WHERE (");

            if (!isAdmin)
            {
                sbSql.Append("J.ClientId = @ClientId) AND (");
                sbSql.Append("JF.JobFileId IS NULL OR JF.IsReturnFile = 1 OR JF.IsSubmittedFile = 1) AND (");
            }

            // filter out ones we already found by jobid
            if (hasJobIds)
            {
                k = 0;
                foreach (int jobId in jobIds)
                {
                    if (k > 0)
                        sbSql.Append(" AND ");

                    sbSql.Append("J.JobId <> @JobId" + k++.ToString());
                }
                sbSql.Append(") AND (");
            }

            // search all terms in instructions
            k = 0;
            foreach (string term in terms)
            {
                if (k > 0)
                    sbSql.Append(" OR ");

                sbSql.Append("J.Instructions LIKE '%' + @Term" + k.ToString() + " + '%'");
                sbSql.Append(" OR JF.JobFileName LIKE '%' + @Term" + k++.ToString() + " + '%'");
                //sbSql.Append(" OR 
            }

            sbSql.Append(")");

            sbSql.Append(" ORDER BY SearchType;"); // matching job # first
            Debug.WriteLine(sbSql.ToString());

            cmd.CommandText = sbSql.ToString();

            // parameters
            if (!isAdmin)
                cmd.Parameters.AddWithValue("@ClientId", clientId);

            k = 0;
            foreach (int jobId in jobIds)
                cmd.Parameters.AddWithValue("@JobId" + k++.ToString(), jobId);

            k = 0;
            foreach (string term in terms)
                cmd.Parameters.AddWithValue("@Term" + k++.ToString(), term);

            SqlDataAdapter daJobs = new SqlDataAdapter(cmd);
            DataTable dtJobs = new DataTable();

            try
            {
                daJobs.Fill(dtJobs);
            }
            catch (Exception ex)
            {
                LogErrorAndSendAlert(ex);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return dtJobs;
        }

        //public DataTable RecentJobsDataTable(int clientId)
        //{
        //    SqlConnection conn = new SqlConnection(CLIENT_CONNECTION_STRING);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = conn;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "OpenAndRecentJobsForClient";

        //    cmd.Parameters.AddWithValue("@ClientId", clientId);

        //    SqlDataAdapter daRecentJobs = new SqlDataAdapter(cmd);
        //    DataTable dtRecentJobs = new DataTable();

        //    daRecentJobs.Fill(dtRecentJobs);
        //    return dtRecentJobs;
        //}

        public DataRow JobFromIdDataRow(int jobId)
        {
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Jobs WHERE JobId = @JobId;";
            cmd.Parameters.AddWithValue("@JobId", jobId);

            SqlDataAdapter daJobFromId = new SqlDataAdapter(cmd);
            DataTable dtJobFromId = new DataTable();

            try
            {
                daJobFromId.Fill(dtJobFromId);
                if (dtJobFromId.Rows.Count > 0)
                    return dtJobFromId.Rows[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                LogErrorAndSendAlert(ex);
                return null;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
        }

        public DataTable AllJobsForClientDataTable(int clientId)
        {
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Jobs WHERE ClientId = @ClientId;";

            cmd.Parameters.AddWithValue("@ClientId", clientId);

            SqlDataAdapter daJobsFromClientId = new SqlDataAdapter(cmd);
            DataTable dtJobsFromClientId = new DataTable();

            try
            {
                daJobsFromClientId.Fill(dtJobsFromClientId);
            }
            catch (Exception ex)
            {
                LogErrorAndSendAlert(ex);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
 
            return dtJobsFromClientId;
       }

        public DataTable UninvoicedJobsForClientDataTable(int clientId)
        {
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Jobs WHERE ClientId = @ClientId AND InvoiceId IS NULL;";

            cmd.Parameters.AddWithValue("@ClientId", clientId);

            SqlDataAdapter daJobsFromClientId = new SqlDataAdapter(cmd);
            DataTable dtJobsFromClientId = new DataTable();

            try
            {
                daJobsFromClientId.Fill(dtJobsFromClientId);
            }
            catch (Exception ex)
            {
                LogErrorAndSendAlert(ex);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return dtJobsFromClientId;
        }

        public int CountOfJobsForClient(int clientId)
        {
            int ret = 0;

            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT COUNT(*) FROM Jobs WHERE ClientId = @ClientId;";

            cmd.Parameters.AddWithValue("@ClientId", clientId);

            conn.Open();

            try
            {
                ret = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                LogErrorAndSendAlert(ex);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return ret;
        }

        public int InsertJob(int clientId, string billingReference, int jobTypeId, string toApplication, bool formatted, bool proofed, DateTime dateSubmitted, DateTime dateDue, string instructions, decimal estimate, decimal finalCharge, decimal taxes)
        {
            int jobId = 0;

            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "InsertJob"; 

            cmd.Parameters.AddWithValue("@ClientId", clientId);
            cmd.Parameters.AddWithValue("@BillingReference", billingReference);
            cmd.Parameters.AddWithValue("@JobTypeId", jobTypeId);
            cmd.Parameters.AddWithValue("@ToApplication", toApplication);
            cmd.Parameters.AddWithValue("@Formatted", formatted);
            cmd.Parameters.AddWithValue("@Proofread", proofed);
            cmd.Parameters.AddWithValue("@DateSubmitted", dateSubmitted);
            cmd.Parameters.AddWithValue("@DateDue", dateDue);
            cmd.Parameters.AddWithValue("@Instructions", instructions);
            cmd.Parameters.AddWithValue("@Estimate", estimate);
            cmd.Parameters.AddWithValue("@FinalCharge", finalCharge);
            cmd.Parameters.AddWithValue("@Taxes", taxes);

            SqlParameter parm = new SqlParameter();
            parm.ParameterName = "@JobId";
            parm.SqlDbType = SqlDbType.Int;
            parm.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parm);

            conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            cmd.Transaction = trans;

            try
            {
                cmd.ExecuteNonQuery();
                jobId = (int)cmd.Parameters["@JobId"].Value;
                trans.Commit();
                conn.Close();
            }
            catch (Exception ex1)
            {
                LogErrorAndSendAlert(ex1);
                try
                {
                    trans.Rollback();
                }
                catch (Exception ex2)
                {
                    LogErrorAndSendAlert(ex2);
                }
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return jobId;
        }

        public bool UpdateJob(string billingReference, int jobTypeId, string toApplication, bool formatted, bool proofed, DateTime dateDue, object objDateCompleted, string instructions, decimal estimate, decimal finalCharge, decimal taxes, string deliveryNotes, bool isArchived, int jobId)
        {
            bool ret = false;

            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "UpdateJob";

            cmd.Parameters.AddWithValue("@BillingReference", billingReference);
            cmd.Parameters.AddWithValue("@JobTypeId", jobTypeId);
            cmd.Parameters.AddWithValue("@ToApplication", toApplication);
            cmd.Parameters.AddWithValue("@Formatted", formatted);
            cmd.Parameters.AddWithValue("@Proofread", proofed);
            //cmd.Parameters.AddWithValue("@DateSubmitted", dateSubmitted);
            cmd.Parameters.AddWithValue("@DateDue", dateDue);

            SqlParameter parm = new SqlParameter();
            parm.ParameterName = "@DateCompleted";
            if (objDateCompleted != null)
                parm.Value = (DateTime)objDateCompleted;
            else
                parm.Value = System.DBNull.Value;
            cmd.Parameters.Add(parm);

            cmd.Parameters.AddWithValue("@Instructions", instructions);
            cmd.Parameters.AddWithValue("@Estimate", estimate);
            cmd.Parameters.AddWithValue("@FinalCharge", finalCharge);
            cmd.Parameters.AddWithValue("@Taxes", taxes);
            cmd.Parameters.AddWithValue("@DeliveryNotes", deliveryNotes);
            cmd.Parameters.AddWithValue("@IsArchived", isArchived);
            cmd.Parameters.AddWithValue("@JobId", jobId);

            conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            cmd.Transaction = trans;

            try
            {
                cmd.ExecuteNonQuery();
                trans.Commit();
                ret = true;
                conn.Close();
            }
            catch (Exception ex1)
            {
                LogErrorAndSendAlert(ex1);
                try
                {
                    trans.Rollback();
                }
                catch (Exception ex2)
                {
                    LogErrorAndSendAlert(ex2);
                }
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return ret;
        }

        public bool UpdateJobSetInvoice(int jobId, int invoiceId)
        {
            bool ret = false;

            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "UPDATE Jobs SET InvoiceId = @InvoiceId WHERE JobId = @JobId;";

            cmd.Parameters.AddWithValue("@InvoiceId", invoiceId);
            cmd.Parameters.AddWithValue("@JobId", jobId);

            conn.Open();

            try
            {
                cmd.ExecuteNonQuery();
                ret = true;
                conn.Close();
            }
            catch (Exception ex)
            {
                LogErrorAndSendAlert(ex);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return ret;
        }

        public bool UpdateJobPickedUp(bool pickedUp, int jobId)
        {
            bool ret = false;
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "UPDATE Jobs SET PickedUp = @PickedUp WHERE JobId = @JobId;";

            cmd.Parameters.AddWithValue("@PickedUp", pickedUp);
            cmd.Parameters.AddWithValue("@JobId", jobId);

            conn.Open();

            try
            {
                cmd.ExecuteNonQuery();
                ret = true;
                conn.Close();
            }
            catch (Exception ex)
            {
                LogErrorAndSendAlert(ex);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return ret;
        }

        public void InsertDummyJobs(int numberOfJobs, int numberOfDocumentsPerJob)
        {
            //bool ret = false;
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            for (int k = 0; k < numberOfJobs; k++)
            {
                DateTime utcNow = DateTime.UtcNow;

                int jobId = InsertJob(Settings.Default.DefaultClientId, "n/a", Settings.Default.JobTypeConsultingId, "n/a", false, false, utcNow, utcNow.AddDays(1.0), "n/a", 0m, 0m, 0m);
                for (int j = 0; j < numberOfDocumentsPerJob; j++)
                {
                    int documentId = InsertJobFile(jobId, "na", "na", false, true);
                }

                // completed & archive
                cmd.CommandText = "INSERT INTO JobStatusChanges(JobId, JobStatusId, DateOfChange) VALUES(@JobId" + k.ToString() + ", " + Settings.Default.JobStatusCompletedId.ToString() + ", @DateCompleted" + k.ToString() + "); "
                + "UPDATE Jobs SET DateCompleted = @DateCompleted" + k.ToString() + ", IsArchived = 1 WHERE JobId = @JobId" + k.ToString() + ";"; 

                cmd.Parameters.AddWithValue("@DateCompleted" + k.ToString(), utcNow);
                cmd.Parameters.AddWithValue("@JobId" + k.ToString(), jobId);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            //return ret;
        }

        //public bool UpdateJobStatus(int jobId, int jobStatusId)
        //{
        //    bool ret = false;
        //    SqlConnection conn = new SqlConnection(clientConnectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = conn;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "UpdateJobStatus"; // batch -> also insert into status changes

        //    cmd.Parameters.AddWithValue("@JobStatusId", jobStatusId);
        //    cmd.Parameters.AddWithValue("@JobId", jobId);

        //    conn.Open();
        //    SqlTransaction trans = conn.BeginTransaction();
        //    cmd.Transaction = trans;

        //    try
        //    {
        //        cmd.ExecuteNonQuery();
        //        trans.Commit();
        //        ret = true;
        //        conn.Close();
        //    }
        //    catch (Exception ex1)
        //    {
        //        LogErrorAndSendAlert(ex1);
        //        try
        //        {
        //            trans.Rollback();
        //        }
        //        catch (Exception ex2)
        //        {
        //            LogErrorAndSendAlert(ex2);
        //        }
        //    }
        //    finally
        //    {
        //        if (conn.State != ConnectionState.Closed)
        //            conn.Close();
        //    }

        //    return ret;
        //}

        //public bool UpdateJobDateDue(int jobId, DateTime newDateDue)
        //{
        //    bool ret = false;
        //    SqlConnection conn = new SqlConnection(clientConnectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = conn;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "UpdateJobDueDate";

        //    cmd.Parameters.AddWithValue("@NewDateDue", newDateDue);
        //    cmd.Parameters.AddWithValue("@JobId", jobId);

        //    conn.Open();
        //    SqlTransaction trans = conn.BeginTransaction();
        //    cmd.Transaction = trans;

        //    try
        //    {
        //        cmd.ExecuteNonQuery();
        //        trans.Commit();
        //        ret = true;
        //        conn.Close();
        //    }
        //    catch (Exception ex1)
        //    {
        //        InsertError(ex1);
        //        try
        //        {
        //            trans.Rollback();
        //        }
        //        catch (Exception ex2)
        //        {
        //            InsertError(ex2);
        //        }
        //    }
        //    finally
        //    {
        //        if (conn.State != ConnectionState.Closed)
        //            conn.Close();
        //    }

        //    return ret;
        //}

        //public bool UpdateJobEstimate(decimal estimate, decimal taxes, int jobId)
        //{
        //    bool ret = false;
        //    SqlConnection conn = new SqlConnection(clientConnectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = conn;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "UpdateJobEstimate";

        //    cmd.Parameters.AddWithValue("@Estimate", estimate);
        //    cmd.Parameters.AddWithValue("@Taxes", taxes);
        //    cmd.Parameters.AddWithValue("@JobId", jobId);

        //    conn.Open();
        //    SqlTransaction trans = conn.BeginTransaction();
        //    cmd.Transaction = trans;

        //    try
        //    {
        //        cmd.ExecuteNonQuery();
        //        trans.Commit();
        //        ret = true;
        //        conn.Close();
        //    }
        //    catch (Exception ex1)
        //    {
        //        InsertError(ex1);
        //        try
        //        {
        //            trans.Rollback();
        //        }
        //        catch (Exception ex2)
        //        {
        //            InsertError(ex2);
        //        }
        //    }
        //    finally
        //    {
        //        if (conn.State != ConnectionState.Closed)
        //            conn.Close();
        //    }

        //    return ret;
        //}

        //public bool UpdateJobFinalCharge(decimal finalCharge, decimal taxes, int jobId)
        //{
        //    bool ret = false;
        //    SqlConnection conn = new SqlConnection(clientConnectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = conn;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "UpdateJobFinalCharge";

        //    cmd.Parameters.AddWithValue("@FinalCharge", finalCharge);
        //    cmd.Parameters.AddWithValue("@Taxes", taxes);
        //    cmd.Parameters.AddWithValue("@JobId", jobId);

        //    conn.Open();
        //    SqlTransaction trans = conn.BeginTransaction();
        //    cmd.Transaction = trans;

        //    try
        //    {
        //        cmd.ExecuteNonQuery();
        //        trans.Commit();
        //        ret = true;
        //        conn.Close();
        //    }
        //    catch (Exception ex1)
        //    {
        //        InsertError(ex1);
        //        try
        //        {
        //            trans.Rollback();
        //        }
        //        catch (Exception ex2)
        //        {
        //            InsertError(ex2);
        //        }
        //    }
        //    finally
        //    {
        //        if (conn.State != ConnectionState.Closed)
        //            conn.Close();
        //    }

        //    return ret;
        //}

        #endregion

        #region jobfiles

        public int InsertJobFile(int jobId, string filename, string path, bool isReturnFile, bool isSubmittedFile)
        {
            int jobFileId = 0;

            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "InsertJobFile";

            cmd.Parameters.AddWithValue("@JobId", jobId);
            cmd.Parameters.AddWithValue("@JobFileName", filename);
            cmd.Parameters.AddWithValue("@JobFilePath", path);
            cmd.Parameters.AddWithValue("@IsReturnFile", isReturnFile);
            cmd.Parameters.AddWithValue("@IsSubmittedFile", isSubmittedFile);

            SqlParameter parm = new SqlParameter();
            parm.ParameterName = "@JobFileId";
            parm.SqlDbType = SqlDbType.Int;
            parm.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parm);

            conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            cmd.Transaction = trans;

            try
            {
                cmd.ExecuteNonQuery();
                jobFileId = (int)cmd.Parameters["@JobFileId"].Value;
                trans.Commit();
                conn.Close();
            }
            catch (Exception ex1)
            {
                LogErrorAndSendAlert(ex1);
                try
                {
                    trans.Rollback();
                }
                catch (Exception ex2)
                {
                    LogErrorAndSendAlert(ex2);
                }
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return jobFileId;
        }

        public DataTable JobFilesForJobDataTable(int jobId)
        {
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM JobFiles WHERE JobId = @JobId;";
            cmd.Parameters.AddWithValue("@JobId", jobId);

            SqlDataAdapter daJobFiles = new SqlDataAdapter(cmd);
            DataTable dtJobFiles = new DataTable();

            try
            {
                daJobFiles.Fill(dtJobFiles);
            }
            catch (Exception ex)
            {
                LogErrorAndSendAlert(ex);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return dtJobFiles;
        }

        public DataRow JobFileFromIdDataRow(int jobFileId)
        {
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM JobFiles WHERE JobFileId = @JobFileId;";
            cmd.Parameters.AddWithValue("@JobFileId", jobFileId);

            SqlDataAdapter daJobFileFromId = new SqlDataAdapter(cmd);
            DataTable dtJobFileFromId = new DataTable();

            try
            {
                daJobFileFromId.Fill(dtJobFileFromId);
                if (dtJobFileFromId.Rows.Count > 0)
                    return dtJobFileFromId.Rows[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                LogErrorAndSendAlert(ex);
                return null;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
        }

        public bool DeleteJobFile(int jobFileId)
        {
            bool ret = false;

            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "DELETE FROM JobFiles WHERE JobFileId = @JobFileId;";

            cmd.Parameters.AddWithValue("@JobFileId", jobFileId);

            conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
                ret = true;
            }
            catch (Exception ex)
            {
                LogErrorAndSendAlert(ex);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return ret;

        }

        #endregion

        #region jobsegments

        public DataTable JobSegmentsForJobDataTable(int jobId)
        {
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM JobSegments WHERE JobId = @JobId;";

            cmd.Parameters.AddWithValue("@JobId", jobId);

            SqlDataAdapter daSegmentsFromJobId = new SqlDataAdapter(cmd);
            DataTable dtSegmentsFromJobId = new DataTable();

            try
            {
                daSegmentsFromJobId.Fill(dtSegmentsFromJobId);
            }
            catch (Exception ex)
            {
                LogErrorAndSendAlert(ex);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return dtSegmentsFromJobId;
        }

        public int InsertJobSegment(int jobId, int employeeId, DateTime startDate, string notes)
        {
            int jobSegmentId = 0;

            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO JobSegments(JobId, EmployeeId, StartDate, Notes) VALUES(@JobId, @EmployeeId, @StartDate, @Notes); SET @JobSegmentId = SCOPE_IDENTITY();";

            cmd.Parameters.AddWithValue("@JobId", jobId);
            cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@Notes", notes);

            SqlParameter parm = new SqlParameter();
            parm.ParameterName = "@JobSegmentId";
            parm.SqlDbType = SqlDbType.Int;
            parm.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parm);

            conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            cmd.Transaction = trans;

            try
            {
                cmd.ExecuteNonQuery();
                jobSegmentId = (int)cmd.Parameters["@JobSegmentId"].Value;
                trans.Commit();
                conn.Close();
            }
            catch (Exception ex1)
            {
                LogErrorAndSendAlert(ex1);
                try
                {
                    trans.Rollback();
                }
                catch (Exception ex2)
                {
                    LogErrorAndSendAlert(ex2);
                }
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return jobSegmentId;
        }

        public bool UpdateJobSegment(DateTime startDate, DateTime endDate, int minutesWorked, string notes, int jobSegmentId)
        {
            bool ret = false;
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "UPDATE JobSegments SET StartDate = @StartDate, EndDate = @EndDate, MinutesWorked = @MinutesWorked, Notes = @Notes WHERE JobSegmentId = @JobSegmentId;";

            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);
            cmd.Parameters.AddWithValue("@MinutesWorked", minutesWorked);
            cmd.Parameters.AddWithValue("@Notes", notes);
            cmd.Parameters.AddWithValue("@JobSegmentId", jobSegmentId);

            conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            cmd.Transaction = trans;

            try
            {
                cmd.ExecuteNonQuery();
                trans.Commit();
                ret = true;
                conn.Close();
            }
            catch (Exception ex1)
            {
                LogErrorAndSendAlert(ex1);
                try
                {
                    trans.Rollback();
                }
                catch (Exception ex2)
                {
                    LogErrorAndSendAlert(ex2);
                }
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return ret;
        }

        #endregion

        #region jobstatuses

        public DataTable AllJobStatusesDataTable()
        {
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM JobStatuses;";

            SqlDataAdapter daStatuses = new SqlDataAdapter(cmd);
            DataTable dtStatuses = new DataTable();

            try
            {
                daStatuses.Fill(dtStatuses);
            }
            catch (Exception ex)
            {
                LogErrorAndSendAlert(ex);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return dtStatuses;
        }

        #endregion

        #region jobstatuschanges

        public DataTable JobStatusChangesForJobDataTable(int jobId)
        {
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM JobStatusChanges WHERE JobId = @JobId;";
            cmd.Parameters.AddWithValue("@JobId", jobId);

            SqlDataAdapter daJobStatusChanges = new SqlDataAdapter(cmd);
            DataTable dtJobStatusChanges = new DataTable();

            try
            {
                daJobStatusChanges.Fill(dtJobStatusChanges);
            }
            catch (Exception ex)
            {
                LogErrorAndSendAlert(ex);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return dtJobStatusChanges;
        }

        public int InsertJobStatusChange(int jobId, int jobStatusId, DateTime dt)
        {
            int jobStatusChangeId = 0;

            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO JobStatusChanges(JobId, JobStatusId, DateOfChange) VALUES(@JobId, @JobStatusId, @DateOfChange); SET @JobStatusChangeId = SCOPE_IDENTITY();";

            cmd.Parameters.AddWithValue("@JobId", jobId);
            cmd.Parameters.AddWithValue("@JobStatusId", jobStatusId);
            cmd.Parameters.AddWithValue("@DateOfChange", dt);

            SqlParameter parm = new SqlParameter();
            parm.ParameterName = "@JobStatusChangeId";
            parm.SqlDbType = SqlDbType.Int;
            parm.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parm);

            conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            cmd.Transaction = trans;

            try
            {
                cmd.ExecuteNonQuery();
                jobStatusChangeId = (int)cmd.Parameters["@JobStatusChangeId"].Value;
                trans.Commit();
                conn.Close();
            }
            catch (Exception ex1)
            {
                LogErrorAndSendAlert(ex1);
                try
                {
                    trans.Rollback();
                }
                catch (Exception ex2)
                {
                    LogErrorAndSendAlert(ex2);
                }
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return jobStatusChangeId;
        }

        #endregion

        #region jobtypes

        public DataTable AllJobTypesDataTable()
        {
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM JobTypes;";

            SqlDataAdapter daJobTypes = new SqlDataAdapter(cmd);
            DataTable dtJobTypes = new DataTable();

            try
            {
                daJobTypes.Fill(dtJobTypes);
            }
            catch (Exception ex)
            {
                LogErrorAndSendAlert(ex);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return dtJobTypes;
        }

        #endregion

        #region employees

        public DataTable AllEmployeesDataTable()
        {
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Employees;";

            SqlDataAdapter daEmployees = new SqlDataAdapter(cmd);
            DataTable dtEmployees = new DataTable();

            try
            {
                daEmployees.Fill(dtEmployees);
            }
            catch (Exception ex)
            {
                LogErrorAndSendAlert(ex);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return dtEmployees;
        }

        public DataRow EmployeeFromIdDataRow(int employeeId)
        {
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Employees WHERE EmployeeId = @EmployeeId;";
            cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

            SqlDataAdapter daEmployeeFromId = new SqlDataAdapter(cmd);
            DataTable dtEmployeeFromId = new DataTable();

            try
            {
                daEmployeeFromId.Fill(dtEmployeeFromId);
                if (dtEmployeeFromId.Rows.Count > 0)
                    return dtEmployeeFromId.Rows[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                LogErrorAndSendAlert(ex);
                return null;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
        }

        public int InsertEmployee(string firstName, string lastName, string title, int addressId, string phone, DateTime hireDate, Guid authUserId, string timeZoneId)
        {
            int employeeId = 0;

            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO Employees (FirstName, LastName, Title, AddressId, Phone, HireDate, AuthUserId, TimeZoneId) VALUES (@FirstName, @LastName, @Title, @AddressId, @Phone, @HireDate, @AuthUserId, @TimeZoneId); SET @EmployeeId = SCOPE_IDENTITY();";

            cmd.Parameters.AddWithValue("@FirstName", firstName);
            cmd.Parameters.AddWithValue("@LastName", lastName);
            cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@AddressId", addressId);
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.Parameters.AddWithValue("@HireDate", hireDate);
            cmd.Parameters.AddWithValue("@AuthUserId", authUserId);
            cmd.Parameters.AddWithValue("@TimeZoneId", timeZoneId);

            SqlParameter parm = new SqlParameter();
            parm.ParameterName = "@EmployeeId";
            parm.SqlDbType = SqlDbType.Int;
            parm.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(parm);

            conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            cmd.Transaction = trans;

            try
            {
                cmd.ExecuteNonQuery();
                employeeId = (int)cmd.Parameters["@EmployeeId"].Value;
                trans.Commit();
                conn.Close();
            }
            catch (Exception ex1)
            {
                LogErrorAndSendAlert(ex1);
                try
                {
                    trans.Rollback();
                }
                catch (Exception ex2)
                {
                    LogErrorAndSendAlert(ex2);
                }
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return employeeId;
        }

        #endregion

        #region messages

        public int InsertMessageThread(string threadName, DateTime dateCreatedUtc)
        {
            int threadId = 0;

            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            string sql = "INSERT INTO MessageThreads(ThreadName, DateCreated) VALUES(@ThreadName, @DateCreated); SET @MessageThreadId = SCOPE_IDENTITY();";

            cmd.CommandText = sql;

            cmd.Parameters.AddWithValue("@ThreadName", threadName);
            cmd.Parameters.AddWithValue("@DateCreated", dateCreatedUtc);

            SqlParameter parm = new SqlParameter();
            parm.ParameterName = "@MessageThreadId";
            parm.DbType = DbType.Int32;
            parm.Direction = ParameterDirection.Output;

            cmd.Parameters.Add(parm);

            conn.Open();

            try
            {
                cmd.ExecuteNonQuery();
                threadId = (int)cmd.Parameters["@MessageThreadId"].Value;
                conn.Close();
            }
            catch (Exception ex1)
            {
                LogErrorAndSendAlert(ex1);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return threadId;
        }

        public int InsertMessage(string senderName, string senderEmail, DateTime dateUtc, string subject, string message, int threadId, int inReplyToMsgId = 0)
        {
            int messageId = 0;

            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            string sql = "INSERT INTO Messages(ThreadId, InReplyToMessageId, SenderName, SenderEmail, DateSent, Subject, Message) VALUES(@ThreadId, @InReplyToMessageId, @SenderName, @SenderEmail, @DateSent, @Subject, @Message); SET @MessageId = SCOPE_IDENTITY();";

            cmd.CommandText = sql;

            cmd.Parameters.AddWithValue("@ThreadId", threadId);

            SqlParameter parm = new SqlParameter();
            parm.ParameterName = "@InReplyToMessageId";
            parm.DbType = DbType.Int32;

            if (inReplyToMsgId != 0)
                parm.Value = inReplyToMsgId;
            else
                parm.Value = System.DBNull.Value;

            cmd.Parameters.Add(parm);

            cmd.Parameters.AddWithValue("@SenderName", senderName);
            cmd.Parameters.AddWithValue("@SenderEmail", senderEmail);
            cmd.Parameters.AddWithValue("@DateSent", dateUtc);
            cmd.Parameters.AddWithValue("@Subject", subject);
            cmd.Parameters.AddWithValue("@Message", message);

            parm = new SqlParameter();
            parm.ParameterName = "@MessageId";
            parm.DbType = DbType.Int32;
            parm.Direction = ParameterDirection.Output;

            cmd.Parameters.Add(parm);
            
            conn.Open();

            try
            {
                cmd.ExecuteNonQuery();
                messageId = (int)cmd.Parameters["@MessageId"].Value;
                conn.Close();
            }
            catch (Exception ex1)
            {
                LogErrorAndSendAlert(ex1);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return messageId;
        }

        public int InsertMessageAttachment(string filename, int messageId)
        {
            int attachmentId = 0;

            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            string sql = "INSERT INTO MessageAttachments(MessageId, Filename) VALUES(@MessageId, @Filename); SET @MessageAttachmentId = SCOPE_IDENTITY();";

            cmd.CommandText = sql;

            cmd.Parameters.AddWithValue("@MessageId", messageId);
            cmd.Parameters.AddWithValue("@Filename", filename);

            SqlParameter parm = new SqlParameter();
            parm.ParameterName = "@MessageAttachmentId";
            parm.DbType = DbType.Int32;
            parm.Direction = ParameterDirection.Output;

            cmd.Parameters.Add(parm);

            conn.Open();

            try
            {
                cmd.ExecuteNonQuery();
                attachmentId = (int)cmd.Parameters["@MessageAttachmentId"].Value;
                conn.Close();
            }
            catch (Exception ex1)
            {
                LogErrorAndSendAlert(ex1);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return attachmentId;
        }

        #endregion

        #region invoices

        public DataTable InvoicesFromClientDataIdTable(int clientId, bool unpaidOnly, bool currentOnly)
        {
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            if (currentOnly)
                cmd.CommandText = "SELECT TOP(1) * FROM Invoices WHERE ClientId = @ClientId AND DATEDIFF(day, InvoiceDate, @Date) <= " + Settings.Default.CurrentInvoiceNumberOfDays + " ORDER BY InvoiceDate DESC;";

            else if (unpaidOnly)
                cmd.CommandText = "SELECT * FROM Invoices WHERE ClientId = @ClientId AND IsPaid = 0;";

            else
                cmd.CommandText = "SELECT * FROM Invoices WHERE ClientId = @ClientId;";

            cmd.Parameters.AddWithValue("@ClientId", clientId);
            cmd.Parameters.AddWithValue("@Date", DateTime.UtcNow);

            SqlDataAdapter daInvoicesFromClientId = new SqlDataAdapter(cmd);
            DataTable dtInvoicesFromClientId = new DataTable();

            try
            {
                daInvoicesFromClientId.Fill(dtInvoicesFromClientId);
            }
            catch (Exception ex)
            {
                LogErrorAndSendAlert(ex);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return dtInvoicesFromClientId; 
        }

        internal int InsertInvoice(int clientId, DateTime invoiceDate, DateTime dateDue, decimal discountAmount, decimal discountRate, decimal amountDue)
        {
            int invoiceId = 0;

            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            string sql = "INSERT INTO Invoices(ClientId, InvoiceDate, DateDue, DiscountRate, DiscountAmount, AmountDue, AmountPaid, HasBeenSent, IsPaid, PaymentPending) VALUES(@ClientId, @InvoiceDate, @DateDue, @DiscountRate, @DiscountAmount, @AmountDue, @AmountPaid, @HasBeenSent, @IsPaid, @PaymentPending); SET @InvoiceId = SCOPE_IDENTITY();";

            cmd.CommandText = sql;

            cmd.Parameters.AddWithValue("@ClientId", clientId);
            cmd.Parameters.AddWithValue("@InvoiceDate", invoiceDate);
            cmd.Parameters.AddWithValue("@DateDue", dateDue);
            cmd.Parameters.AddWithValue("@DiscountRate", discountRate);
            cmd.Parameters.AddWithValue("@DiscountAmount", discountAmount);
            cmd.Parameters.AddWithValue("@AmountDue", amountDue);
            cmd.Parameters.AddWithValue("@AmountPaid", 0m);
            cmd.Parameters.AddWithValue("@HasBeenSent", 0);
            cmd.Parameters.AddWithValue("@IsPaid", 0);
            cmd.Parameters.AddWithValue("@PaymentPending", 0);

            SqlParameter parm = new SqlParameter();
            parm.ParameterName = "@InvoiceId";
            parm.DbType = DbType.Int32;
            parm.Direction = ParameterDirection.Output;

            cmd.Parameters.Add(parm);

            conn.Open();

            try
            {
                cmd.ExecuteNonQuery();
                invoiceId = (int)cmd.Parameters["@InvoiceId"].Value;
                conn.Close();
            }
            catch (Exception ex1)
            {
                LogErrorAndSendAlert(ex1);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return invoiceId;
        }

        public bool UpdateInvoice(int clientId, DateTime invoiceDate, DateTime dateDue, decimal discountRate, decimal discountAmount, decimal amountDue, decimal amountPaid, bool hasBeenSent, bool paymentPending, bool isPaid, int invoiceId)
        {
            bool ret = false;

            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            string sql = "UPDATE Invoices " 
                + "SET InvoiceDate = @InvoiceDate, DateDue = @DateDue, DiscountRate = @DiscountRate, DiscountAmount = @DiscountAmount, AmountDue = @AmountDue, AmountPaid = @AmountPaid, HasBeenSent = @HasBeenSent, IsPaid = @IsPaid, PaymentPending = @PaymentPending, " 
                + "WHERE InvoiceId = @InvoiceId;";

            cmd.CommandText = sql;

            cmd.Parameters.AddWithValue("@InvoiceDate", invoiceDate);
            cmd.Parameters.AddWithValue("@DateDue", dateDue);
            cmd.Parameters.AddWithValue("@DiscountRate", discountRate);
            cmd.Parameters.AddWithValue("@DiscountAmount", discountAmount);
            cmd.Parameters.AddWithValue("@AmountDue", amountDue);
            cmd.Parameters.AddWithValue("@AmountPaid", amountPaid);
            cmd.Parameters.AddWithValue("@HasBeenSent", hasBeenSent);
            cmd.Parameters.AddWithValue("@IsPaid", isPaid);
            cmd.Parameters.AddWithValue("@PaymentPending", paymentPending);
            cmd.Parameters.AddWithValue("@InvoiceId", invoiceId);

            conn.Open();

            try
            {
                cmd.ExecuteNonQuery();
                ret = true;
                conn.Close();
            }
            catch (Exception ex1)
            {
                LogErrorAndSendAlert(ex1);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return ret;
        }

        #endregion

        #region errors

        public void LogErrorAndSendAlert(Exception ex, string extraInfo = null, bool sendAlert = true)
        {
            SqlConnection conn = new SqlConnection(clientConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO Errors(ErrorMethod, ErrorMessage, ErrorDate) "
                + "VALUES(@ErrorMethod, @ErrorMessage, @ErrorDate);";

            cmd.Parameters.AddWithValue("@ErrorMethod", ex.TargetSite.ToString());
            cmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
            cmd.Parameters.AddWithValue("@ErrorDate", DateTime.UtcNow);

            if (extraInfo != null)
                cmd.Parameters.AddWithValue("@ExtraInfo", extraInfo);
            else
                cmd.Parameters.AddWithValue("@ExtraInfo", String.Empty);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex1)
            {
                // retry
                try
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex2)
                {
                    Debug.WriteLine(ex1.Message + Environment.NewLine + ex2.Message);
                }
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            if (sendAlert && !(ex is SmtpException)) // don't get into infinite loop if it's a mail exception
                new Emailer(Settings.Default.SmtpHost, Settings.Default.EmailStyleTag).SendErrorAlert(ex, extraInfo);
        }

        #endregion

        #region utilities

        #endregion

    }
}