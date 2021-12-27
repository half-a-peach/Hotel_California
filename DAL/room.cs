namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("room")]
    public partial class room
    {

        public int id { get; set; }

        public int room_number { get; set; }

        public int capacity { get; set; }

        public int room_type { get; set; }

        public double price { get; set; }

        public virtual ICollection<reservation> reservation { get; set; }

        public virtual roomType roomType { get; set; }

        public room()
        {
            
        }
    }
}
