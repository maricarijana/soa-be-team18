using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tours.Application.Dtos.Shopping
{
    public class TourPurchaseTokenDto
    {
        public long Id { get; set; }
        public int TourId { get; set; }
        public long UserId { get; set; }
    }
}
