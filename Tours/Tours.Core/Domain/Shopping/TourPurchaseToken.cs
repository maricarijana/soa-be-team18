using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tours.Core.Domain.Shopping
{
    public class TourPurchaseToken
    {
        public long Id { get; init; }

        public long TourId { get; init; }

        public Tour? Tour { get; private set; } = null;

        public long UserId { get; init; }
    }
}
