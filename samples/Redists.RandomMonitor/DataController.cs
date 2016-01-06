using Redists.Configuration;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Redists.RandomMonitor
{
    public class DataController : ApiController
    {
        // GET api/values 
        public Task<DataPoint[]> Get()
        {
            //get last hour
            var client = TimeSeriesFactory.New(RedisServer.GetDatabase(0), "fkts", new TimeSeriesOptions(60 * 1000, 1000, TimeSpan.FromHours(1)));
            return client.AllSinceAsync(DateTime.UtcNow.AddMinutes(-1));
        }

        // GET api/values/5 
        public string Get(int id)
        {
            return "value";
        }
    }
}
