using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloSharp;
using HaloSharp.Model;
using H5_DataPipeline.Secrets;

namespace H5_DataPipeline.Shared
{
    public class HaloClientFactory
    {
        private HaloClient _devHaloClient;
        private HaloClient _prodHaloClient;

        private int devRequestLimitPerTenSeconds = 10;
        private int prodRequestLimitPerTenSeconds = 200;

        public HaloClientFactory()
        {
            _devHaloClient = MakeClient(SecretAPIKey.GetDev(), devRequestLimitPerTenSeconds);
            _prodHaloClient = MakeClient(SecretAPIKey.GetProd(), prodRequestLimitPerTenSeconds);
        }

        public HaloClient GetDevClient()
        {
            return _devHaloClient;
        }

        public HaloClient GetProdClient()
        {
            return _prodHaloClient;
        }


        public HaloClient MakeClient(string key, int requestCountOverTenSeconds)
        {
            var product = new Product
            {
                SubscriptionKey = key,
                RateLimit = new RateLimit
                {
                    RequestCount = requestCountOverTenSeconds,
                    TimeSpan = new TimeSpan(0, 0, 0, 10),
                    Timeout = new TimeSpan(0, 0, 0, 10)
                }
            };

            var cacheSettings = new CacheSettings
            {
                CacheDuration = new TimeSpan(0, 1, 0, 0)
            };

            return new HaloClient(product, cacheSettings);

            
        }

    }
}

