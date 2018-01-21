using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace H5_DataPipeline.Assistants.Shared
{
    public class Referee
    {
        private ConcurrentDictionary<int, bool> jobBook;

        public Referee()
        {
            jobBook = new ConcurrentDictionary<int, bool>();
        }

        /// <summary>
        /// Register a specific job with this Referee.
        /// </summary>
        /// <param name="jobNumber"></param>
        public void WaitToRegisterJob(int jobNumber)
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

        /// <summary>
        /// Tell the Referee that a specific job is complete.
        /// </summary>
        /// <param name="jobNumber"></param>
        public void WaitToMarkJobDone(int jobNumber)
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

        /// <summary>
        /// The Referee will not return from this method until all tracked jobs are complete.
        /// </summary>
        public void WaitUntilAllJobsAreDone(bool silent = false)
        {
            bool jobsAreDone = false;
            while (!jobsAreDone)
            {
                if(!silent)
                {
                    Console.Write("\rWaiting on {0} jobs to complete.                                  ",
                                            GetNumberOfUnfinishedJobs());
                }

                if (AllJobsAreDone())
                {
                    jobsAreDone = true;
                }
                else
                {
                    Thread.Sleep(250);
                }
            }
        }

        private bool AllJobsAreDone()
        {
            bool result = false;

            int unfinishedJobCounts = GetNumberOfUnfinishedJobs();
            if (unfinishedJobCounts == 0)
            {
                result = true;
            }

            return result;
        }

        private int GetNumberOfUnfinishedJobs()
        {
            return jobBook.Values.Where(value => value == false).Count();
        }
    }
}
