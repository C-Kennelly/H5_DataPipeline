using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloSharp.Model.Halo5.Stats.Common;

namespace H5_DataPipeline.Assistants.MatchParticipants
{
    public class CSREntry
    {
        int Tier;                           //Tier within Designation - pull from Metadata API
        int DesignationID;                  //Gold, Onyx, Diamond, etc
        int Csr;                            //0 if not Onyx or Champion
        int PercentToNextTier;              
        int? Rank;                          //Null if not Onyx or Champion

        //https://developer.haloapi.com/docs/services/58acdf27e2f7f71ad0dad84b/operations/58acdf28e2f7f70db4854b37
        public CSREntry(CompetitiveSkillRanking csr)
        {
            Tier = csr.Tier;
            DesignationID = (int)csr.DesignationId;
            Csr = csr.Csr;
            PercentToNextTier = csr.PercentToNextTier;
            Rank = csr.Rank;
        }
    }

}
