using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.DataAccess
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration configuration)
        {
            SetupDatabase(configuration);
            CatalogContextSeed.SeedData(Products);
        }

        public IMongoCollection<Product> Products { get; set; }

        // setup Mongodb database, collections
        public void SetupDatabase(IConfiguration configuration)
        {
            var dbSettings = configuration.GetSection("DbSettings").Get<DbSettings>();
            var mongoCleint = new MongoClient(dbSettings.ConnectionString);
            var database = mongoCleint.GetDatabase(dbSettings.DatabaseName);

            Products = database.GetCollection<Product>(dbSettings.CollectionName);
        }
    }
}
