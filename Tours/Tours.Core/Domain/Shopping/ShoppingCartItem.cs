using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tours.Core.Domain.Shopping
{
    public class ShoppingCartItem
    {
        public long Id { get; init; }

        public long ShoppingCartId { get; init; }

        public long TourId { get; init; }

        public Tour? Tour { get; private set; } = null;

        public string TourName { get; private set; } = string.Empty;

        public decimal Price { get; private set; }
    }
}
