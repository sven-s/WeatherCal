using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace WeatherCal.FeedMgmt
{
    public static class ExtensionMethods
    {
        public static async System.Threading.Tasks.Task<IEnumerable<DynamicTableEntity>> ExecuteQueryAsync(this CloudTable table, TableQuery query)
        {
            TableContinuationToken token = null;
            var retVal = new List<DynamicTableEntity>();
            do
            {
                var results = await table.ExecuteQuerySegmentedAsync(query, token);
                retVal.AddRange(results.Results);
                token = results.ContinuationToken;
            } while (token != null);

            return retVal;
        }

        public static async System.Threading.Tasks.Task<IEnumerable<T>> ExecuteQueryAsync<T>(this CloudTable table, TableQuery<T> query) where T : ITableEntity, new()
        {
            TableContinuationToken token = null;
            var retVal = new List<T>();
            do
            {
                var results = await table.ExecuteQuerySegmentedAsync(query, token);
                retVal.AddRange(results.Results);
                token = results.ContinuationToken;
            } while (token != null);

            return retVal;
        }
    }
}