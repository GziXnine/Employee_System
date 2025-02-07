using Microsoft.Extensions.Configuration;

namespace Employee_System
{
    public static class DataBaseHelper
    {
        public static string GetConnectionString()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true)
                .Build();

            return config.GetConnectionString("DefaultConnection") ?? string.Empty;
        }
    }
}
