using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Threading.Tasks;

using Newtonsoft.Json;

namespace BikeTest.Controllers
{
    [RoutePrefix("api/bikes")]
    public class BikesController : ApiController
    {
        [Route("GetBikes")]
        public async Task<IEnumerable<BikeModel>> GetBikes()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://trekhiringassignments.blob.core.windows.net/interview/bikes.json");

                var json = await response.Content.ReadAsStringAsync();

                //more time: added to DbContext using EF rather than requery.
                var model = JsonConvert.DeserializeObject<List<Bikes>>(json);

                IEnumerable<BikeModel> results = model
                    .GroupBy(b => b.bikes)
                    .Select(b => new BikeModel
                    {
                        BikeName = b.Key,
                        BikeCount = b.Key.Count()
                    })
                    .OrderByDescending(b => b.BikeCount)
                    .Skip(0)
                    .Take(20)
                    .ToList();

                return results;
            }
        }
    }

    public class Bikes
    {
        public IList<string> bikes { get; set; }
    }

    public class BikeModel
    {

        public IList<string> BikeName { get; set; }

        public int BikeCount { get; set; }
    }

}
