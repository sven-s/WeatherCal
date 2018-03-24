using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using WeatherCal.UserMgmt.Entities;

namespace WeatherCal.UserMgmt
{
    public class Registration
    {
        public const string UserTableName = "Users";
        public const string FeedTableName = "Feeds";
        public const string SubscriptionTableName = "Subcriptions";

        public const string PartitionName = "P1";

        public const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=your_account;AccountKey=your_account_key";

        private CloudStorageAccount _cloudStorageAccount;
        private CloudTableClient _tableClient;

        private CloudTable _userTable;
        private CloudTable _feedTable;
        private CloudTable _subcriptionTable;

        

        

        public Registration(string connectionstring = "")
        {
            if (string.IsNullOrEmpty(connectionstring))
                connectionstring = ConnectionString;
            _cloudStorageAccount = CloudStorageAccount.Parse(connectionstring);
            InitializeTables();
        }

        private void InitializeTables()
        {
            _tableClient = _cloudStorageAccount.CreateCloudTableClient();
            _userTable = _tableClient.GetTableReference(UserTableName);
            _feedTable = _tableClient.GetTableReference(SubscriptionTableName);
            _subcriptionTable = _tableClient.GetTableReference(SubscriptionTableName);

            _userTable.CreateIfNotExistsAsync();
            _feedTable.CreateIfNotExistsAsync();
            _subcriptionTable.CreateIfNotExistsAsync();
        }

        //public User AddUser(string userName)
        //{
        //    var user = new User {UserName = userName};
        //    return null;
        //}

        public async Task<Feed> AddSubscriptionToFeed(Subscription subscription, Guid? feedGuid)
        {
            var feed = new Feed();

            if (feedGuid.HasValue)
            {
                var tableOperation = TableOperation.Retrieve<Feed>(PartitionName, feedGuid.Value.ToString());
                var retrievedResult = await _feedTable.ExecuteAsync(tableOperation);
                if (retrievedResult.Result != null)
                {
                    if (retrievedResult.Result is Feed tempFeed)
                        feed = tempFeed;
                }
            }
            else{
                feed.Subscriptions.Add(subscription);

                var insertOperation = TableOperation.Insert(feed);

                var insertResult = await _feedTable.ExecuteAsync(insertOperation);

                if (insertResult != null)
                {
                    //var 
                }
            }

            



            return feed;
        }

        public void DeleteSubscription(Guid subscriptionGuid)
        {
            //if last subscription of the feed, delete feed also
        }

        public void DeleteSubscription(Subscription subscription)
        {
            
        }

        public List<Feed> GetFeeds()
        {
            return null;
        }

        public List<Subscription> GetSubscriptions(Guid feedGuid)
        {
            return null;
        }
        public List<Subscription> GetSubscriptions(Feed feed)
        {
            return GetSubscriptions(feed.Id);
        }

        public List<Subscription> GetSubscriptions()
        {
            return null;
        }


    }
}
