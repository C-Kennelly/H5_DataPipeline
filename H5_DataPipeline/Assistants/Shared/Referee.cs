using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_DataPipeline.Assistants.Shared
{
    public class Referee
    {
        private ConcurrentDictionary<int, bool> jobBook;

        public Referee()
        {
            jobBook = new ConcurrentDictionary<int, bool>();
        }

        public bool AllJobsAreDone()
        {
            bool result = false;

            int unfinishedJobCounts = jobBook.Values.Where(value => value == false).Count();
            if(unfinishedJobCounts == 0)
            {
                result = true;
            }

            return result;
        }

        public void RegisterJob(int jobNumber)
        {
            jobBook.AddOrUpdate(jobNumber, false, (key, oldValue) => oldValue = false);
        }

        public async void MarkJobDone(int jobNumber)
        {
            jobBook.TryUpdate(jobNumber, true, false);
        }



    }
}
