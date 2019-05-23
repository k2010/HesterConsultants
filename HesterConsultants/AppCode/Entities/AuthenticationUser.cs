using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Web.Security;
using System.Diagnostics;

namespace HesterConsultants.AppCode.Entities
{
    /// <summary>
    /// Currently, this class serves only as a partial wrapper for MembershipUser.
    /// It has methods for use with ListViews.
    /// </summary>
    public class AuthenticationUser
    {
        public string UserName { get; set; }
        //public Guid ProviderUserKey { get; set; }
        public MembershipUser MembershipUser { get; set; }

        // methods

        public static MembershipUserCollection AllMembershipUsers()
        {
            return Membership.GetAllUsers();
        }

        public static List<AuthenticationUser> AllAuthUsers()
        {
            List<AuthenticationUser> allUsers = new List<AuthenticationUser>();

            foreach (MembershipUser mUser in Membership.GetAllUsers())
            {
                AuthenticationUser aUser = new AuthenticationUser();
                aUser.UserName = mUser.UserName;
                aUser.MembershipUser = mUser;
                //aUser.ProviderUserKey = (Guid)mUser.ProviderUserKey;

                //Debug.WriteLine("aUser.UserName: " + aUser.UserName);

                allUsers.Add(aUser);
            }

            return allUsers;
        }

        //public static List<AuthenticationUser> AllAuthUsers()
        //{
        //    DataTable dtAuthUsers = ClientData.Current.AllAuthUsersDataTable();
        //    List<AuthenticationUser> allAuthUsers = new List<AuthenticationUser>();

        //    foreach (DataRow drAuthUser in dtAuthUsers.Rows)
        //    {
        //        AuthenticationUser authUser = new AuthenticationUser();
        //        authUser.UserId = (Guid)drAuthUser["UserId"];
        //        authUser.UserName = drAuthUser["UserName"].ToString();

        //        allAuthUsers.Add(authUser);
        //    }

        //    return allAuthUsers;
        //}

        //public static AuthenticationUser AuthUserFromId(Guid id)
        //{
        //    DataRow drAuthUser = ClientData.Current.AuthUserFromIdDataRow(id);
        //    AuthenticationUser authUser = new AuthenticationUser();
        //    authUser.UserId = (Guid)drAuthUser["UserId"];
        //    authUser.UserName = drAuthUser["UserName"].ToString();

        //    return authUser;
        //}

        public static MembershipUser MembershipUserFromId(Guid guid)
        {
            return Membership.GetUser(guid);
        }

        public static void InsertMembershipUser(AuthenticationUser aUser)
        {
            // do nothing; insert the membership user from event handler on page
        }

        public static bool DeleteMembershipUser(AuthenticationUser aUser)
        {
            // delete from the provider store
            bool ret = false;
            MembershipUser mUser = Membership.GetUser(aUser.UserName);
            Debug.WriteLine(mUser.UserName);

            try
            {
                Membership.DeleteUser(mUser.UserName, true);
                ret = true;
            }
            catch (Exception ex)
            {
                ClientData.Current.LogErrorAndSendAlert(ex);
                ret = false;
            }

            return ret;
        }

        // operators
        //public override bool Equals(object obj)
        //{
        //    try
        //    {
        //        return this.UserId == ((AuthenticationUser)obj).UserId;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public override int GetHashCode()
        //{
        //    return this.UserId.GetHashCode();
        //}
    }
}