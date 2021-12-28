using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class RevenueReport
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public float RoomCost { get; set; }
        public float ServiceCost { get; set; }
        public float TotalCost { get; set; }
        public string CheckInDate1 { get; set; }
        public string CheckOutDate1 { get; set; }
    }
}
