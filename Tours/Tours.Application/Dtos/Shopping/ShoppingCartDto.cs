using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tours.Application.Dtos.Shopping
{
    public class ShoppingCartDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public IEnumerable<ShoppingCartItemDto> Items { get; set; } = new List<ShoppingCartItemDto>();
    }
}
