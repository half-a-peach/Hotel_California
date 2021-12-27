namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("client")]
    public partial class client
    {
        public int id { get; set; }

        public string full_name { get; set; }

        [Column(TypeName = "date")]
        public DateTime birthdate { get; set; }

        public string phone_number { get; set; }

        public string client_document { get; set; }

        public virtual ICollection<reservation> reservation { get; set; }

        public client()
        {
            
        }
    }
}
