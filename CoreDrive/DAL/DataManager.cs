using CoreDrive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreDrive.DAL
{
    public class DataManager
    {
        public AccountModel ValidateUser(AccountModel account)
        {
            if (account.TrustedLogin)
            {
                String username = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                if (account.Email.Equals("admin@admin.com") && account.Password.Equals("Admin123"))
                {
                    account.AccountId = Guid.NewGuid();
                    account.Name = "Admin";
                }
                else
                {
                    account = null;
                }
            }
            else if (account.Email.Equals("admin@coredrive.com") && account.Password.Equals("JellyFish"))
            {
                account.AccountId = Guid.NewGuid();
                account.Name = "Administrator";
            }
            else
            {
                account = null;
            }

            return account;
        }
    }
}