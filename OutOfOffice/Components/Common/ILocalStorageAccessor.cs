using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOffice.Components.Common
{
    public interface ILocalStorageAccessor
    {
        public Task<T> GetValueAsync<T>(string key);

        public Task SetValueAsync<T>(string key, T value);

        public Task Clear();

        public Task RemoveAsync(string key);
    }
}
