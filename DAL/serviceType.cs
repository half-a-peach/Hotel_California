namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("serviceType")]
    public partial class serviceType
    {
        public serviceType()
        {
            
        }

        public int id { get; set; }

        public string name { get; set; }

        public virtual ICollection<service> service { get; set; }
    }
}
