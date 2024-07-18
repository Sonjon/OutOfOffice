namespace OutOfOffice.Components.Common
{
    public class LocalStorageAccessor : ILocalStorageAccessor
    {


        public LocalStorageAccessor()
        {

        }

        public async Task<T> GetValueAsync<T>(string key)
        {
            throw new NotImplementedException();
        }

        public async Task SetValueAsync<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public async Task Clear()
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAsync(string key)
        {
            throw new NotImplementedException();
        }
    }
}
