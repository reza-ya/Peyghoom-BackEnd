using MongoDB.Driver;

namespace Peyghoom_BackEnd.Infrastructures
{
    public interface IPeyghoomContext
    {
        IMongoDatabase GetRPeyghoomDatabase();
    }
}