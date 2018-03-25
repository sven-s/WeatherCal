using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using WeatherCal.UserMgmt.Entities;

namespace WeatherCal.UserMgmt
{


    public class Registration : IRegistration
    {
        public const string UserTableName = "Users";
        public const string FeedTableName = "Feeds";
        public const string SubscriptionTableName = "Subcriptions";

        public const string PartitionName = "P1";

        public const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=your_account;AccountKey=your_account_key";

        private CloudStorageAccount _cloudStorageAccount;
        private CloudTableClient _tableClient;

        //private CloudTable _userTable;
        private CloudTable _feedTable;
        //private CloudTable _subcriptionTable;

        

        

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
            //_userTable = _tableClient.GetTableReference(UserTableName);
            _feedTable = _tableClient.GetTableReference(FeedTableName);
            //_subcriptionTable = _tableClient.GetTableReference(SubscriptionTableName);

            //_userTable.CreateIfNotExistsAsync();
            _feedTable.CreateIfNotExistsAsync();
            //_subcriptionTable.CreateIfNotExistsAsync();
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
                feed = await LoadFeed(feedGuid.Value);
                feed.Subscriptions.Add(subscription);
                return await AlterFeed(feed);
            }

            feed.Subscriptions.Add(subscription);
            return await CreateFeed(feed);

        }

        public async void DeleteSubscription(Guid subscriptionGuid)
        {
            //if last subscription of the feed, delete feed also
            //var query = from feed in _feedTable 
            //https://stackoverflow.com/questions/18549555/multiple-filter-conditions-azure-table-storage

            //var query = new TableQuery();   

            var feeds = await GetFeeds();
            var feed = feeds.FirstOrDefault(x => x.Subscriptions.Any(s => s.Id.Equals(subscriptionGuid)));
            feed.Subscriptions.Remove(feed.Subscriptions.First(s => s.Id.Equals(subscriptionGuid)));
            
            await AlterFeed(feed);
        }

        public async void DeleteSubscription(Subscription subscription)
        {
            DeleteSubscription(subscription.Id);
        }

        public async Task<List<Feed>> GetFeeds()
        {
            //https://stackoverflow.com/questions/23940246/how-to-query-all-rows-in-windows-azure-table-storage
            var queryResult =
            await _feedTable.ExecuteQueryAsync(new TableQuery<Feed>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionName)));
            return queryResult.ToList();
        }

        public async Task<List<Subscription>> GetSubscriptions(Guid feedGuid)
        {
            //var x = GetFeeds()..Result.FirstOrDefault(x => x.Id.Equals(feedGuid));
            var feeds = await GetFeeds();

            //return new Task<List<Subscription>>(() => feeds.Result.FirstOrDefault(x => x.Id.Equals(feedGuid)).Subscriptions);
            //return feeds.Result.FirstOrDefault(x => x.Id.Equals(feedGuid)).Subscriptions;
            return feeds.FirstOrDefault(x => x.Id.Equals(feedGuid)).Subscriptions;
        }

        public Task<List<Subscription>> GetSubscriptions(Feed feed)
        {
            return GetSubscriptions(feed.Id);
        }
        
        private async Task<Feed> AlterFeed(Feed feed)
        {
            var updateOperation = TableOperation.Replace(feed);
            var result = await _feedTable.ExecuteAsync(updateOperation);

            if (result.Result != null)
            {
                return result.Result as Feed;
            }
            throw new Exception($"Could not update feed with Id: '{feed?.Id}'");
        }

        private async Task<Feed> CreateFeed(Feed feed)
        {
            var insertOperation = TableOperation.Insert(feed);

            var insertResult = await _feedTable.ExecuteAsync(insertOperation);

            if (insertResult != null)
            {
                var resultFeed = insertResult.Result as Feed;
                return resultFeed;
            }
            throw new Exception($"Could not create feed with Guid: {feed?.Id}");
        }

        private async Task<Feed> LoadFeed(Guid feedGuid)
        {
            var tableOperation = TableOperation.Retrieve<Feed>(PartitionName, feedGuid.ToString());
            var retrievedResult = await _feedTable.ExecuteAsync(tableOperation);
            if (retrievedResult.Result != null)
            {
                if (retrievedResult.Result is Feed tempFeed)
                {
                    return tempFeed;
                }
            }
            throw new Exception($"Could not find a feed with Guid: {feedGuid}");
        }

        private async void DeleteFeed(Guid feedGuid)
        {
            var tableOperation = TableOperation.Retrieve<Feed>(PartitionName, feedGuid.ToString());
            var retrievedResult = await _feedTable.ExecuteAsync(tableOperation);
            if (retrievedResult.Result != null)
            {
                if (retrievedResult.Result is Feed tempFeed)
                {
                    var deleteOperation = TableOperation.Delete(tempFeed);
                    await _feedTable.ExecuteAsync(deleteOperation);
                }
            }
            
        }

    }
}
