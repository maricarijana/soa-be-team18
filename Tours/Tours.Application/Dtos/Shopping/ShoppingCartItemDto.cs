using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tours.Application.Dtos.Shopping
{
    public class ShoppingCartItemDto
    {
        public long Id { get; set; }
        public long ShoppingCartId { get; set; }
        public long TourId { get; set; }
        public string TourName { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
