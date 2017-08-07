using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloSharp;
using HaloSharp.Model;

namespace H5_DataPipeline
{
    public sealed class Global
    {
        public static Global Instance { get; private set; }
        private static HaloClient _haloClient;

        public static HaloClient haloClient { get { return _haloClient; } }

        private Global()
        {
            _haloClient = makeClient();
        }

        static Global()
        {
            Instance = new Global();
        }

    // API method:
        private HaloClient makeClient()
        {
            
            var product = new Product
            {
                SubscriptionKey = Secrets.SecretAPIKey.Get(),
                RateLimit = new RateLimit
                {
                    RequestCount = 200,
                    TimeSpan = new TimeSpan(0, 0, 0, 10),
                    Timeout = new TimeSpan(0, 0, 0, 10)
                }
            };

            var cacheSettings = new CacheSettings
            {
                CacheDuration = new TimeSpan(0, 1, 0, 0)
            };

            HaloClient client = new HaloClient(product, cacheSettings);

            return client;
        }
    }
}
