namespace TA.Contracts
{
    public interface IApplicationSettings
    {
        string ConnectionString { get; set; }
        string ApiKey { get; set; }
    }
}