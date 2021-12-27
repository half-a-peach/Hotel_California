namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class service_string
    {
        public int id { get; set; }

        public int service { get; set; }

        public int reservation { get; set; }

        public double cost { get; set; }

        public virtual reservation reservation1 { get; set; }

        public virtual service service1 { get; set; }
    }
}
