//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace H5_DataPipeline.Models.SpartanClash
{
    using System;
    using System.Collections.Generic;
    
    public partial class t_clashdevset
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_clashdevset()
        {
            this.t_companies = new HashSet<t_companies>();
        }
    
        public string MatchId { get; set; }
        public Nullable<int> GameMode { get; set; }
        public string HopperId { get; set; }
        public string MapId { get; set; }
        public Nullable<int> MapVariant_ResourceType { get; set; }
        public string MapVariant_ResourceId { get; set; }
        public Nullable<int> MapVariant_OwnerType { get; set; }
        public string MapVariant_Owner { get; set; }
        public string GameBaseVariantID { get; set; }
        public string GameVariant_ResourceID { get; set; }
        public Nullable<int> GameVariant_ResourceType { get; set; }
        public Nullable<int> GameVariant_OwnerType { get; set; }
        public string GameVariant_Owner { get; set; }
        public Nullable<System.DateTime> MatchCompleteDate { get; set; }
        public string MatchDuration { get; set; }
        public byte[] IsTeamGame { get; set; }
        public string SeasonID { get; set; }
        public string Team1_Company { get; set; }
        public int Team1_Rank { get; set; }
        public Nullable<long> Team1_Score { get; set; }
        public Nullable<int> Team1_DNFCount { get; set; }
        public string Team2_Company { get; set; }
        public int Team2_Rank { get; set; }
        public Nullable<long> Team2_Score { get; set; }
        public Nullable<int> Team2_DNFCount { get; set; }
        public int Status { get; set; }
    
        public virtual t_matchparticipants t_matchparticipants { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_companies> t_companies { get; set; }
    }
}
