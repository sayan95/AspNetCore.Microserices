using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.API.DataAccess
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get; set; }
        void SetupDatabase(IConfiguration configuration);
    }
}
