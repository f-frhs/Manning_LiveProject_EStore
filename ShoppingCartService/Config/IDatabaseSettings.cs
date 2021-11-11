namespace ShoppingCartService.Config
{
    public interface IDatabaseSettings
    {
        string CollectionName { get; }
        string ConnectionString { get; }
        string DatabaseName { get; }
    }
}