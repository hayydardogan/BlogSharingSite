//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BlogSharingSite.Models.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class TBL_Comments
    {
        public int commentID { get; set; }
        public string commentUserName { get; set; }
        public string commentContent { get; set; }
        public Nullable<System.DateTime> commentDate { get; set; }
        public Nullable<int> blogID { get; set; }
        public Nullable<bool> commentStatus { get; set; }
        public string commentUserMail { get; set; }
    
        public virtual TBL_Blogs TBL_Blogs { get; set; }
    }
}
