using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tours.Core.Domain.Shopping;

namespace Tours.Core.Domain.RepositoryInterfaces
{
    public interface IShoppingCartRepository
    {
        Task SaveChangesAsync();
        Task<ShoppingCart?> GetByUserIdAsync(long userId);
        ShoppingCart Create(ShoppingCart cart);
        Task<ShoppingCart> CreateAsync(ShoppingCart cart);
    }

}