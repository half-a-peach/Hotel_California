namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class status
    {
        public status()
        {
            
        }

        public int id { get; set; }

        public string name { get; set; }

        public virtual ICollection<reservation> reservation { get; set; }
    }
}
