using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskProject.Services
{
    public interface IDataStore<T>
    {
        Task<bool> DeleteItemAsync(int id);
        Task<T> GetItemAsync(int id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
    }
}
