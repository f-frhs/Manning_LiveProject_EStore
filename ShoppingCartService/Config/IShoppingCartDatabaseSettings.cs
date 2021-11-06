namespace ShoppingCartService.Config
{
    public interface IShoppingCartDatabaseSettings
    {
        string CollectionName { get; }
        string ConnectionString { get; }
        string DatabaseName { get; }
    }
}