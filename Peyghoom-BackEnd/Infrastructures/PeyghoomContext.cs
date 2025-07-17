using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Peyghoom_BackEnd.Options;

namespace Peyghoom_BackEnd.Infrastructures
{
    public class PeyghoomContext : IPeyghoomContext
    {
        private PeyghoomContextOptions _optionsSnapshot;

        public PeyghoomContext(IOptionsSnapshot<PeyghoomContextOptions> optionsSnapshot)
        {
            _optionsSnapshot = optionsSnapshot.Value;
        }

        public IMongoDatabase GetRPeyghoomDatabase()
        {
            var connectionString = _optionsSnapshot.ConnectionString;

            if (_optionsSnapshot.ConnectionString == null)
            {
                // TODO: log and throw exception
                throw new Exception();
            }

            var client = new MongoClient(connectionString);


            var database = client.GetDatabase("rpeyghoom");

            return database;
        }
    }
}
