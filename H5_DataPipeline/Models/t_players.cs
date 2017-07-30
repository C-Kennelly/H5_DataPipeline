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
    
    public partial class t_players
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_players()
        {
            this.t_players_to_teams = new HashSet<t_players_to_teams>();
            this.t_h5matches = new HashSet<t_h5matches>();
        }
    
        public string gamertag { get; set; }
        public Nullable<System.DateTime> dateLastMatchScan { get; set; }
        public Nullable<System.DateTime> dateCompanyRosterUpdated { get; set; }
        public Nullable<System.DateTime> dateCustomTeamsUpdated { get; set; }
        public int scanThresholdInDays { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_players_to_teams> t_players_to_teams { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_h5matches> t_h5matches { get; set; }
    }
}
