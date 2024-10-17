using Alachisoft.NCache.Client;
using NCache_Real_Time_Cache_Monitoring.Data;
using NCache_Real_Time_Cache_Monitoring.IRepository;
using NCache_Real_Time_Cache_Monitoring.Model;

namespace NCache_Real_Time_Cache_Monitoring.Repository
{
    public class ProductRepository : IProductRepository<Product> 
    {
        private readonly ICache _cache;
        private readonly ProductDbContext _dbContext;
        private const string ProductCacheKeyPrefix = "Product_";

        public ProductRepository(ICache cache, ProductDbContext dbContext)
        {
            _cache = cache;
            _dbContext = dbContext;
        }

        // Cache Operations
        public void AddToCache(Product product)
        {
            _cache.Insert(ProductCacheKeyPrefix + product.ProductId, product);
        }

        public Product GetFromCache(int productId)
        {
            return _cache.Get<Product>(ProductCacheKeyPrefix + productId) as Product;
        }

        public void InvalidateCache(int productId)
        {
            _cache.Remove(ProductCacheKeyPrefix + productId);
        }

        // Database Operations
        public void Add(Product product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
            AddToCache(product); // Add to cache after saving to DB
        }

        public Product Get(int productId)
        {
            // First, try to get from cache
            var product = GetFromCache(productId);
            if (product == null)
            {
                // If not found in cache, retrieve from database
                product = _dbContext.Products.Find(productId);
                if (product != null)
                {
                    AddToCache(product); // Cache the product for future requests
                }
            }
            return product;
        }

        public void Delete(int productId)
        {
            var product = _dbContext.Products.Find(productId);
            if (product != null)
            {
                _dbContext.Products.Remove(product);
                _dbContext.SaveChanges();
                InvalidateCache(productId); // Invalidate cache after deletion
            }
        }
    }
}
