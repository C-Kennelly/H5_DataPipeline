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
    
    public partial class t_h5matches
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_h5matches()
        {
            this.t_players = new HashSet<t_players>();
        }
    
        public string matchID { get; set; }
    
        public virtual t_h5matches_matchdetails t_h5matches_matchdetails { get; set; }
        public virtual t_h5matches_playersformatch t_h5matches_playersformatch { get; set; }
        public virtual t_h5matches_teamsinvolved_halowaypointcompanies t_h5matches_teamsinvolved_halowaypointcompanies { get; set; }
        public virtual t_h5matches_teamsinvolved_spartanclashfireteams t_h5matches_teamsinvolved_spartanclashfireteams { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_players> t_players { get; set; }
    }
}