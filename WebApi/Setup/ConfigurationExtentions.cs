namespace WebApi
{
    public static class ConfigurationExtentions
    {
        public static string GetRequiredConnectionString(this IConfiguration config, string connectionStringName)
        {
            string? connectionString = config.GetConnectionString(connectionStringName);

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException();

            return connectionString;
        }
    }
}