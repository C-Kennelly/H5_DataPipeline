using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models.DataPipeline;

namespace H5_DataPipeline.Assistants.Shared
{
    class Janitor
    {
        public void DeleteMatchTree(string matchIDToDelete)
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_h5matches currentRecord = db.t_h5matches.Find(matchIDToDelete);

                if (currentRecord != null)
                {
                    db.t_h5matches.Remove(currentRecord);
                    db.SaveChanges();
                }
            }
        }
    }
}
