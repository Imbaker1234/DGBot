namespace DGBot
{
    using System.IO;
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationProvider
    {
        private static IConfigurationRoot Root { get; set; }

        public static IConfigurationRoot GetAppSettings()
        {
            while (true)
            {
                if (!(Root is null)) return Root;

                Root = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
            }
        }
    }
}