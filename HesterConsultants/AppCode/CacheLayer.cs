using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using HesterConsultants.AppCode;
using HesterConsultants.AppCode.Entities;
using System.Configuration;
using System.Diagnostics;
using System.Collections;

namespace HesterConsultants.AppCode
{
    public static class CacheLayer
    {
        private static Cache cache = HttpRuntime.Cache; // HttpRuntime.Cache is a singleton
        private static DateTime recentJobsFilledDate;

        //private CacheLayer()
        //{
        //}

        //private class SingletonInstantiator
        //{
        //    // Explicit static constructor to tell C# compiler
        //    // not to mark type as beforefieldinit
        //    static SingletonInstantiator()
        //    {
        //    }

        //    internal static readonly CacheLayer instance = new CacheLayer();
        //}

        ///// <summary>
        ///// Returns the sole instance of CacheLayer.
        ///// </summary>
        //public static CacheLayer Current
        //{
        //    get
        //    {
        //        return SingletonInstantiator.instance;
        //    }
        //}

        #region clients
        public static List<Client> AllClients()
        {
            //Cache cache = HttpRuntime.Cache;

            // ensure parent objects loaded first
            List<Company> allCompanies = AllCompanies();
            List<Address> allAddresses = AllAddresses();

            List<Client> allClients = cache[Global.CACHE_ALL_CLIENTS] as List<Client>;

            if (allClients == null)
            {
                allClients = Client.AllClients();
                //ClientsRepository clientRepository = new ClientsRepository(clientsConnectionString);
                //allClients = clientRepository.AllClients().ToList();

                // parent objects
                // to do - do this at bo layer
                foreach (Client client in allClients)
                {
                    client.Company = CompanyFromId(client.CompanyId);
                    client.Address = AddressFromId(client.AddressId);
                }

                cache.Insert(Global.CACHE_ALL_CLIENTS, allClients);
            }

            return allClients;
        }

        public static Client ClientFromId(int clientId)
        {
            List<Client> clients = AllClients();

            return clients.FirstOrDefault(c => c.ClientId == clientId);
        }

        public static Client ClientFromAuthUserId(Guid authUserId)
        {
            //foreach (Client client in AllClients())
            //    if (client.AuthUserId.ToString().ToLower() == authUserId.ToString().ToLower())
            //        return client;
            //return null;

            return AllClients().FirstOrDefault(c => c.AuthUserId == authUserId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Client must have Company and Address assigned before calling this.
        /// to do - is this necessary? Could we say if (client.company == null) client.company
        /// = companyfromid(companyid) ??
        /// </remarks>
        /// <param name="client"></param>
        public static void InsertClient(Client client)
        {
            // ensure list in cache
            List<Client> allClients = AllClients();

            // insert client to db, get id
            int clientId = ClientData.Current.InsertClient(client.FirstName, client.LastName, client.Phone, client.AuthUserId, DateTime.UtcNow, client.Company.CompanyId, client.Address.AddressId, client.TimeZoneId);

            if (clientId != 0)
            {
                // assign id
                client.ClientId = clientId;

                // add to cached list
                allClients.Add(client);
            }
            else
                throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to add client to database."));
        }

        //public static void SaveClient(Client client)
        //{
        //    // to do - take care of dependent children first 

        //    // ensure list in cache
        //    List<Client> allClients = AllClients();

        //    // insert client to db
        //    ClientsRepository rep = new ClientsRepository(ClientData.CLIENT_CONNECTION_STRING);
        //    rep.SaveClient(client);

        //    // add to cached list
        //    allClients.Add(client);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Client must have Company and Address assigned before calling this.
        /// </remarks>
        /// <param name="client"></param>
        public static void UpdateClient(Client client)
        {
            // ensure list in cache
            List<Client> allClients = AllClients();

            // update db
            bool ret = ClientData.Current.UpdateClient(client.FirstName, client.LastName, client.Phone, client.AuthUserId, client.Company.CompanyId, client.Address.AddressId, client.TimeZoneId, client.ClientId);

            // replace in cached list
            if (ret)
            {
                Debug.WriteLine(allClients.Contains(client).ToString());
                allClients.Remove(client); // .Equals() checks only ClientId
                allClients.Add(client);
            }
            else
                throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to update client in database."));
        }

        #endregion

        #region authusers

        // why do we need to cache authusers?
        //public static List<AuthenticationUser> AllAuthUsers()
        //{
        //    Cache cache = HttpRuntime.Cache;

        //    List<AuthenticationUser> allAuthUsers = cache[Global.CACHE_ALL_AUTHUSERS] as List<AuthenticationUser>;

        //    if (allAuthUsers == null)
        //    {
        //        allAuthUsers = AuthenticationUser.AllAuthUsers();
        //        cache.Insert(Global.CACHE_ALL_AUTHUSERS, allAuthUsers);
        //    }

        //    return allAuthUsers;
        //}

        #endregion

        #region companies

        public static List<Company> AllCompanies()
        {
            //Cache cache = HttpRuntime.Cache;

            List<Company> allCompanies = cache[Global.CACHE_ALL_COMPANIES] as List<Company>;

            if (allCompanies == null)
            {
                allCompanies = Company.AllCompanies();
                //CompaniesRepository rep = new CompaniesRepository(ClientData.CLIENT_CONNECTION_STRING);
                //allCompanies = rep.AllCompanies().ToList();

                cache.Insert(Global.CACHE_ALL_COMPANIES, allCompanies);
            }

            return allCompanies;
        }

        public static Company CompanyFromId(int companyId)
        {
            return AllCompanies().FirstOrDefault(c => c.CompanyId == companyId);
        }

        public static Company CompanyFromName(string companyName)
        {
            return AllCompanies().FirstOrDefault(c => c.Name.ToLower() == companyName.ToLower());
        }

        public static void InsertCompany(Company company)
        {
            // ensure list in cache
            List<Company> allCompanies = AllCompanies();

            // insert to db, get id
            int companyId = ClientData.Current.InsertCompany(company.Name);

            if (companyId != 0)
            {
                // assign id
                company.CompanyId = companyId;

                // insert to cache
                allCompanies.Add(company);
            }
            else
                throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to add company to database."));
        }

        // not currently used.
        //public static void UpdateCompany(Company company)
        //{
        //    // ensure lists (including dependents) in cache
        //    List<Company> allCompanies = AllCompanies();
        //    List<Client> allClients = AllClients();

        //    // update db
        //    ClientData.Current.UpdateCompany(company.Name, company.CompanyId);

        //    // replace in cache
        //    // to do - is this necessary? if we have ref to the obj in cache, no need
        //    allCompanies.Remove(company);
        //    allCompanies.Add(company);

        //    // update child objects
        //    List<Client> companyClients = new List<Client>();
        //    foreach (Client client in allClients)
        //        if (client.Company.Equals(company)) // just compares id
        //            client.Company = company;
        //}

        #endregion

        #region addresses

        public static List<Address> AllAddresses()
        {
            //Cache cache = HttpRuntime.Cache;

            List<Address> allAddresses = cache[Global.CACHE_ALL_ADDRESSES] as List<Address>;

            if (allAddresses == null)
            {
                allAddresses = Address.AllAddresses();
                cache.Insert(Global.CACHE_ALL_ADDRESSES, allAddresses);
            }

            return allAddresses;
        }

        public static Address AddressFromId(int addressId)
        {
            return AllAddresses().FirstOrDefault(a => a.AddressId == addressId);
        }

        public static Address AddressFromStrings(string concatenatedFields)
        {
            string fields = concatenatedFields.ToLower().Trim();

            return AllAddresses().FirstOrDefault(a => a.ConcatenatedFields().ToLower() == fields);
        }

        public static void InsertAddress(Address address)
        {
            // ensure list in cache
            List<Address> allAddresses = AllAddresses();

            // insert to db, get id
            int addressId = ClientData.Current.InsertAddress(address.Address1, address.Address2, address.City, address.State, address.PostalCode, address.Country);

            if (addressId != 0)
            {
                // assign id
                address.AddressId = addressId;

                // insert to cache
                allAddresses.Add(address);
            }
            else
                throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to add address to database."));
        }

        // this is not used. just create a new address instead
        //public static void UpdateAddress(Address address)
        //{
        //    // ensure lists (including dependents) in cache
        //    List<Address> allAddresses = AllAddresses();
        //    List<Client> allClients = AllClients();

        //    // update db
        //    ClientData.Current.UpdateAddress(address.Address1, address.Address2, address.City, address.State, address.PostalCode, address.Country, address.AddressId);

        //    // replace in cache
        //    allAddresses.Remove(address);
        //    allAddresses.Add(address);

        //    // update dependents
        //    foreach (Client client in allClients)
        //        if (client.Address.Equals(address))
        //            client.Address = address;
        //}

        #endregion

        #region jobstatuses

        //public static List<JobStatus> AllJobStatuses()
        //{
        //    //Cache cache = HttpRuntime.Cache;

        //    List<JobStatus> allJobStatuses = cache[Global.CACHE_ALL_JOB_STATUSES] as List<JobStatus>;

        //    if (allJobStatuses == null)
        //    {
        //        allJobStatuses = JobStatus.AllJobStatuses();
        //        cache.Insert(Global.CACHE_ALL_JOB_STATUSES, allJobStatuses);
        //    }

        //    return allJobStatuses;
        //}

        //public static JobStatus JobStatusFromId(int jobStatusId)
        //{
        //    return AllJobStatuses().FirstOrDefault(js => js.JobStatusId == jobStatusId);
        //}

        //public static JobStatus JobStatusFromName(string name)
        //{
        //    return AllJobStatuses().First(js => js.Name.ToLower() == name.ToLower());
        //}

        #endregion

        #region JobStatusChanges

        //public static List<JobStatusChange> RecentJobStatusChanges()
        //{
        //    // parent - jobs
        //    List<Job> recentJobs = RecentJobs();

        //    List<JobStatusChange> recentChanges = cache[Global.CACHE_RECENT_JOB_STATUS_CHANGES] as List<JobStatusChange>;

        //    if (recentChanges == null)
        //    {
        //        recentChanges = JobStatusChange.RecentJobStatusChanges(recentJobsFilledDate);

        //        foreach (JobStatusChange change in recentChanges)
        //        {
        //            Job job = JobFromId(change.JobId);
        //            change.Job = job;
        //            change.JobStatus = JobStatusFromId(change.JobStatusId);
        //        }

        //        cache.Insert(Global.CACHE_RECENT_JOB_STATUS_CHANGES, recentChanges);
        //    }

        //    return recentChanges;
        //}

        #endregion

        #region jobtypes

        //public static List<JobType> AllJobTypes()
        //{
        //    //Cache cache = HttpRuntime.Cache;

        //    List<JobType> allJobTypes = cache[Global.CACHE_ALL_JOB_TYPES] as List<JobType>;

        //    if (allJobTypes == null)
        //    {
        //        allJobTypes = JobType.AllJobTypes();
        //        cache.Insert(Global.CACHE_ALL_JOB_TYPES, allJobTypes);
        //    }

        //    return allJobTypes;
        //}

        //public static JobType JobTypeFromId(int jobTypeId)
        //{
        //    return AllJobTypes().First(jt => jt.JobTypeId == jobTypeId);
        //}

        #endregion

        #region jobfiles

        //public static JobFile JobFileFromId(int id)
        //{
        //    List<Job> cachedJobs = RecentJobs();

        //    // doesn't work because
        //    // .SubmittedFiles etc. may be null.
        //    // to do - consider instantiating JobFile collections
        //    // when instantiating a Job
        //    List<JobFile> cachedJobFiles =
        //        cachedJobs
        //        .SelectMany(j => j.ReturnedFiles)
        //        .Union(cachedJobs.SelectMany(j => j.SubmittedFiles))
        //        .Union(cachedJobs.SelectMany(j => j.WorkingFiles))
        //        .ToList();

        //    foreach (JobFile file in cachedJobFiles)
        //    {
        //        Debug.WriteLine("file: " + file.Name);
        //        if (file.JobFileId == id)
        //            return file;
        //    }

        //    return null;
        //}

        #endregion

        #region recentjobs

        public static List<Job> RecentJobs()
        {
            // parent objs
            List<Client> allClients = AllClients();
            List<JobStatus> allJobStatuses = JobStatus.AllJobStatuses();
            List<JobType> allJobTypes = JobType.AllJobTypes();

            // when this is called,
            // set date for jobstatuschanges
            recentJobsFilledDate = DateTime.UtcNow;
            
            List<Job> recentJobs = cache[Global.CACHE_RECENT_JOBS] as List<Job>;

            // to do - check oldest to refresh

            if (recentJobs == null)
            {
                recentJobs = Job.OpenAndRecentJobs(recentJobsFilledDate).ToList();

                // parent objs
                foreach (Job job in recentJobs)
                {
                    job.Client = ClientFromId(job.ClientId);
                    //job.JobStatus = JobStatusFromId(job.JobStatusId);
                    job.JobType = JobType.JobTypeFromId(job.JobTypeId); // to do - do this at obj level like statuses
                }

                cache.Insert(Global.CACHE_RECENT_JOBS, recentJobs);
           }

            return recentJobs;
        }

        public static List<Job> RecentJobsForAdmin()
        {
            return RecentJobsForAdmin(String.Empty);
        }

        public static List<Job> RecentJobsForAdmin(string sortExpression = "")
        {
            List<Job> jobs = RecentJobs();
            ObjectSortField osf = null;

            if (String.IsNullOrEmpty(sortExpression))
                jobs = jobs
                    .OrderByDescending(j => j.JobStatus.Equals(JobStatus.Submitted))
                    .ThenBy(j => j.DateDue)
                    .ThenByDescending(j => j.JobStatus.Equals(JobStatus.InProgress))
                    .ThenBy(j => j.JobId)
                    .ToList();

            else if (sortExpression.ToLower().StartsWith("jobstatus.name")) // JobStatus.Name
            {
                // custom sort of job statuses for admins
                jobs = jobs
                    .OrderByDescending(j => j.JobStatus.Equals(JobStatus.Submitted))
                    .ThenByDescending(j => j.JobStatus.Equals(JobStatus.PendingApproval))
                    .ThenByDescending(j => j.JobStatus.Equals(JobStatus.InReview))
                    .ThenByDescending(j => j.JobStatus.Equals(JobStatus.InProgress))
                    .ThenByDescending(j => j.JobStatus.Equals(JobStatus.Queued))
                    .ThenByDescending(j => j.JobStatus.Equals(JobStatus.Completed))
                    .ThenByDescending(j => j.JobStatus.Equals(JobStatus.Canceled))
                    .ToList();

                if (sortExpression.ToUpper().EndsWith(" DESC"))
                    jobs.Reverse();
            }

            else
            {
                if (sortExpression.ToUpper().EndsWith(" DESC"))
                    osf = new ObjectSortField(sortExpression.Substring(0, sortExpression.Length - 5), ObjectSortField.SortOrders.Descending);
                else
                    osf = new ObjectSortField(sortExpression, ObjectSortField.SortOrders.Ascending);
                jobs.Sort(new ObjectComparer<Job>(new ObjectSortField[] { osf }));
            }

            return jobs;
        }

        public static List<Job> NewlyCompletedJobsForClient(Client client)
        {
            List<Job> jobs = RecentJobs();

            //foreach (Job job in jobs)
            //{
            //    Debug.WriteLine(job.JobId.ToString());
            //    Debug.WriteLine(job.Client.FirstName);
            //    Debug.WriteLine(job.JobStatus.Name);
            //}

            jobs = jobs.Where(j => j.Client.Equals(client) 
                && j.JobStatus.Equals(JobStatus.Completed) 
                && j.PickedUp == false)
            .ToList();

            return jobs;
        }

        public static List<Job> RecentJobsForClient(Client client, string sortExpression = "")
        {
            Debug.WriteLine("CacheLayer.RecentJobsForClient()");

            List<Job> jobs = RecentJobs();
            ObjectSortField osf = null;

            jobs = jobs.Where(j => j.Client.Equals(client) 
                && !(j.JobStatus.Equals(JobStatus.Completed) && j.PickedUp == false)).ToList();

            if (String.IsNullOrEmpty(sortExpression))
                jobs = jobs
                    .OrderByDescending(j => j.JobStatus.Equals(JobStatus.Completed)) // desc b/c 0 comes before 1
                    .ThenByDescending(j => j.JobStatus.Equals(JobStatus.InProgress))
                    .ThenBy(j => j.DateDue)
                    .ThenBy(j => j.JobId)
                    .ToList();

            else
            {

                if (sortExpression.ToUpper().EndsWith(" DESC"))
                    osf = new ObjectSortField(sortExpression.Substring(0, sortExpression.Length - 5), ObjectSortField.SortOrders.Descending);
                else
                    osf = new ObjectSortField(sortExpression, ObjectSortField.SortOrders.Ascending);
                jobs.Sort(new ObjectComparer<Job>(new ObjectSortField[] {osf}));                                                                                
            }

            foreach (Job job in jobs)
                Debug.WriteLine(" - " + job.JobId.ToString());

            return jobs;
        }

        //private static void DebugJobsList(string header, List<Job> js)
        //{
        //    Debug.WriteLine(header);
        //    foreach (Job j in js)
        //        Debug.WriteLine(j.JobId + " - " 
        //            + j.DateDue.ToString() + " - "
        //            + j.JobStatus.Name + " - "
        //            + "picked up: " + j.PickedUp.ToString());
        //}

        public static List<Job> JobsToPickUp(Client client)
        {
            return RecentJobsForClient(client).Where(j => j.JobStatus.Equals(JobStatus.Completed)
                && j.PickedUp == false)
                .OrderBy(j => j.JobId)
                .ToList();
        }

        public static Job JobFromId(int jobId)
        {
            // try from cache
            Job job = RecentJobs().FirstOrDefault(j => j.JobId == jobId);

            // if not in cache
            if (job == null)
            {
                job = Job.JobFromId(jobId);
                job.Client = ClientFromId(job.ClientId);
                //job.JobType = JobType.JobTypeFromId(job.JobTypeId); // to do - do this at obj level
                //job.JobStatus = JobStatusFromId(job.JobStatusId);
            }

            return job;
        }

        public static void InsertJob(Job job)
        {
            // ensure list in cache
            List<Job> recentJobs = RecentJobs();

            // insert into db
            int jobId = ClientData.Current.InsertJob(job.Client.ClientId, job.BillingReference, job.JobType.JobTypeId, job.ToApplication, job.Formatted, job.Proofread, job.DateSubmitted, job.DateDue, job.Instructions, job.Estimate, job.FinalCharge, job.Taxes);

            if (jobId != 0)
            {
                job.JobId = jobId;

                // put in cached list
                recentJobs.Add(job);
            }
            else
                throw new Exception(SiteUtils.ExceptionMessageForCustomer("Failed to add job to database."));
        }

        #endregion

        #region employees

        //public static List<Employee> AllEmployees()
        //{
        //    //Cache cache = HttpRuntime.Cache;

        //    List<Employee> allEmployees = cache[Global.CACHE_ALL_EMPLOYEES] as List<Employee>;

        //    if (allEmployees == null)
        //    {
        //        allEmployees = Employee.AllEmployees();
        //        cache.Insert(Global.CACHE_ALL_EMPLOYEES, allEmployees);
        //    }

        //    return allEmployees;
        //}

        //public static Employee EmployeeFromAuthUserId(Guid authUserId)
        //{
        //    return AllEmployees().FirstOrDefault(e => e.AuthUserId == authUserId);
        //}


        #endregion

        public static void ClearCache()
        {
            Cache cache = HttpRuntime.Cache;
            foreach (DictionaryEntry entry in cache)
            {
                Debug.WriteLine("cache entry to be removed: " + entry.Key.ToString());
                cache.Remove(entry.Key.ToString());
            }
        }
    }
}