using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_DataPipeline.Models.SpartanClash
{
    public partial class t_companies
    {
        public t_companies(string companyID, string printableCompanyName)
        {
            companyId = companyID;
            companyName = printableCompanyName;
            rank = -1;
            wins = -1;
            losses = -1;
            total_matches = -1;
            win_percent = null;
            times_searched = 0;
        }

    }
}
