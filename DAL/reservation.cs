namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("reservation")]
    public partial class reservation
    {
        public int id { get; set; }

        public int status { get; set; }

        [Column(TypeName = "date")]
        public DateTime check_in_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime check_out_date { get; set; }

        public int amount_of_guests { get; set; }

        public double total_price { get; set; }

        public int client { get; set; }

        public int room { get; set; }

        public double paid { get; set; }

        public virtual client client1 { get; set; }

        public virtual room room1 { get; set; }

        public virtual status status1 { get; set; }

        public virtual ICollection<service_string> service_string { get; set; }

        public reservation()
        {
            service_string = new HashSet<service_string>();
        }
    }
}
