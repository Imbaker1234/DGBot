namespace DGBot
{
    using System.Threading.Tasks;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;
    using Microsoft.Extensions.DependencyInjection;

    class Bot
    {
        private static DiscordClient _client;
        private static CommandsNextExtension _commandsNext;

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            var services = new ServiceCollection();
            services.AddTransient<PublicService>();
            services.AddTransient<ModeratorService>();
            services.AddTransient<TagRepository>(sp => new TagRepository(sp.GetService<GuildBotDbContext>()));
            services.AddSingleton<GuildBotDbContext>(provider =>
                new GuildBotDbContext(ConfigurationProvider.GetAppSettings()["db"]));



            _client = new DiscordClient(new DiscordConfiguration()
            {
                Token = ConfigurationProvider.GetAppSettings()["token"],
                TokenType = TokenType.Bot,
                AutoReconnect = true
            });

            _commandsNext = _client.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefixes = new []{"/"},
                CaseSensitive = false,
                EnableDms = true,
                DmHelp = false,
                Services = services.BuildServiceProvider()
            });
            
            _commandsNext.RegisterAllCommandModules();

            _client
                .HandleUserAdded()
                .HandleUserLeft();
            
            //Ensure database is created
            await _commandsNext.Services.GetService<GuildBotDbContext>().Database.EnsureCreatedAsync();
            
            await _client.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
