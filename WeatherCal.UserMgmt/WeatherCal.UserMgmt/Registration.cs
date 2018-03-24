using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using WeatherCal.UserMgmt.Entities;

namespace WeatherCal.UserMgmt
{
    public class Registration
    {

        //loudStorageAccount storageAccount = CloudStorageAccount.Parse
        //    ("DefaultEndpointsProtocol=https;AccountName=your_account;AccountKey=your_account_key");

        public const string UserTableName = "Users";
        public const string SubscriptionTableName = "Subcriptions";
        //public const string 

        private CloudStorageAccount _cloudStorageAccount;

        public Registration()
        {
            _cloudStorageAccount = CloudStorageAccount.Parse(
                "DefaultEndpointsProtocol=https;AccountName=your_account;AccountKey=your_account_key");
        }

        private void InitializeTables()
        {
            
        }

        public User AddUser()
        {
            return null;
        }
    }
}
