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

            int unfinishedJobCounts = GetNumberOfUnfinishedJobs();
            if(unfinishedJobCounts == 0)
            {
                result = true;
            }

            return result;
        }

        public int GetNumberOfUnfinishedJobs()
        {
            return jobBook.Values.Where(value => value == false).Count();
        }

        public void RegisterJob(int jobNumber)
        {
            bool retry = true;

            while (retry)
            {
                bool result = jobBook.TryAdd(jobNumber, false);
                if (result == true)
                {
                    retry = false;
                }
            }
        }

        public void MarkJobDone(int jobNumber)
        {
            bool retry = true;
            while (retry)
            {
                bool result = jobBook.TryUpdate(jobNumber, true, false);
                if (result == true)
                {
                    retry = false;
                }
            }
            
        }




    }
}
