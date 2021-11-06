namespace ShoppingCartService.Config
{
    public class ShoppingCartDatabaseSettings : IShoppingCartDatabaseSettings
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}