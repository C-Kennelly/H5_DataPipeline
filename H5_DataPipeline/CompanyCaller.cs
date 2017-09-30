﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HaloSharp;
using HaloSharp.Model.Halo5.Stats;
using HaloSharp.Query.Halo5.Stats;
using HaloSharp.Extension;
using HaloSharp.Exception;


namespace H5_DataPipeline
{
    public class CompanyCaller
    {

        public async Task<SpartanCompany> GetWaypointCompanyInformation(string companyId, IHaloSession session)
        {
            SpartanCompany result = null;
            try
            {
                var query = new GetSpartanCompany(new Guid(companyId));
                result = await session.Query(query);    
            }
            catch(HaloApiException e)
            {
                Console.WriteLine("CompanyCaller: The Halo API threw an exception for company {0}, error {1} - {2}.  Stopping calls.", companyId, e.HaloApiError.StatusCode, e.HaloApiError.Message);
                result = null;
                //TODO -> Handle errors here... removing 404's?  Common class for handling API errors?
            }

            return result;
        }
    }
}