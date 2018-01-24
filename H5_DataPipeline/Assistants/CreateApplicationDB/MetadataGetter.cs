using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloSharp;
using HaloSharp.Extension;
using HaloSharp.Validation;
using HaloSharp.Validation.Common;
using HaloSharp.Model;
using HaloSharp.Model.Common;
using HaloSharp.Model.Halo5.Metadata;
using HaloSharp.Model.Halo5.Metadata.Common;
using HaloSharp.Query;
using HaloSharp.Query.Halo5;
using HaloSharp.Query.Halo5.Metadata;
using H5_DataPipeline.Models.SpartanClash;
using H5_DataPipeline.Secrets;

namespace H5_DataPipeline.Assistants.CreateApplicationDB
{
    class MetadataGetter
    {
        public static async Task UpdateMetaDataTables(IHaloSession session)
        {    
            var metaDataQuery = new GetMaps();

            var results = await session.Query(metaDataQuery);

            using (var db = new clashdbEntities())
            {
                foreach (Map result in results)
                {
                    t_mapmetadata newRecord = new t_mapmetadata()
                    {
                        mapId = result.Id.ToString(),
                        printableName = result.Name,
                        imageURL = result.ImageUrl
                    };

                    var query = db.t_mapmetadata.Find(newRecord.mapId);

                    if (query == null)
                    {
                        //Console.WriteLine("Adding data for {0}", newRecord.printableName);

                        try
                        {
                            db.t_mapmetadata.Add(newRecord);
                            db.SaveChanges();
                        }
                        catch
                        {
                            //Console.WriteLine("Database write failed.");
                        }


                        Console.WriteLine("Finished Metadat");
                    }
                }
            }
        
        }
    }
}
