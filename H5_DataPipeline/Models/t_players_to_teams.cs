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
    
    public partial class t_players_to_teams
    {
        public string gamertag { get; set; }
        public string teamId { get; set; }
        public int role { get; set; }
        public System.DateTime lastUpdated { get; set; }
        public Nullable<System.DateTime> joinedDate { get; set; }
        public Nullable<System.DateTime> membershipLastModifiedDate { get; set; }
    
        public virtual t_players t_players { get; set; }
        public virtual t_teams t_teams { get; set; }
    }
}
