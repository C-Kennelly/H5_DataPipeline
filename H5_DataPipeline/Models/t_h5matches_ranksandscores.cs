//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace H5_DataPipeline.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class t_h5matches_ranksandscores
    {
        public string matchId { get; set; }
        public byte[] IsTeamGame { get; set; }
        public int team1_Rank { get; set; }
        public Nullable<int> team1_Score { get; set; }
        public int team2_Rank { get; set; }
        public Nullable<int> team2_Score { get; set; }
        public Nullable<int> team3_Rank { get; set; }
        public Nullable<int> team4_Rank { get; set; }
        public Nullable<int> team5_Rank { get; set; }
        public Nullable<int> team6_Rank { get; set; }
        public Nullable<int> team7_Rank { get; set; }
        public Nullable<int> team8_Rank { get; set; }
        public Nullable<int> team3_Score { get; set; }
        public Nullable<int> team4_Score { get; set; }
        public Nullable<int> team5_Score { get; set; }
        public Nullable<int> team6_Score { get; set; }
        public Nullable<int> team7_Score { get; set; }
        public Nullable<int> team8_Score { get; set; }
    
        public virtual t_h5matches t_h5matches { get; set; }
    }
}
