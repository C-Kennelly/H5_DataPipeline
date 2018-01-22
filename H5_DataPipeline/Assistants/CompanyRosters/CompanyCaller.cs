using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HaloSharp;
using HaloSharp.Model.Halo5.Stats;
using HaloSharp.Query.Halo5.Stats;
using HaloSharp.Extension;
using HaloSharp.Exception;
using H5_DataPipeline.Models;


namespace H5_DataPipeline.Assistants.CompanyRosters
{
    public class CompanyCaller
    {

        public async Task<SpartanCompany> GetWaypointCompanyInformation(string companyId, IHaloSession session)
        {
            SpartanCompany result = null;
            bool retry = true;

            while(retry)
            {
                retry = false;
                try
                {
                    var query = new GetSpartanCompany(new Guid(companyId));
                    result = await session.Query(query);
                }
                catch (HaloApiException haloAPIException)
                {
                    if (haloAPIException.HaloApiError.Message.Contains("Rate limit"))
                    {
                        retry = true;
                        await Task.Delay(50);
                    }
                    else if(haloAPIException.HaloApiError.StatusCode == 404)
                    {
                        result = null;
                        using (var db = new dev_spartanclashbackendEntities())
                        {
                            t_teams companyRecord = db.t_teams.Find(companyId);

                            if(companyRecord != null)
                            {
                                Console.WriteLine("{0} received a 404 from the Halo API, changing tracking index to -1.", companyRecord.teamName);
                                companyRecord.trackingIndex = -1;

                                db.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("CompanyCaller: The Halo API threw an exception for company {0}, error {1} - {2}.  Stopping calls.", companyId, haloAPIException.HaloApiError.StatusCode, haloAPIException.HaloApiError.Message);
                        result = null;
                        //TODO -> Handle errors here... removing 404's?  Common class for handling API errors?
                    }


                    
                }

            }

            return result;
        }
    }
}
