namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("roomType")]
    public partial class roomType
    {
        public roomType()
        {
            
        }

        public int id { get; set; }

        public string category { get; set; }

        public virtual ICollection<room> room { get; set; }
    }
}
