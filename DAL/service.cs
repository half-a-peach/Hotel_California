namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("service")]
    public partial class service
    {
        public service()
        {
            
        }

        public int id { get; set; }

        public double price { get; set; }

        public int service_type { get; set; }

        public string name { get; set; }

        public virtual serviceType serviceType { get; set; }

        public virtual ICollection<service_string> service_string { get; set; }
    }
}
