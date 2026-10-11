using Microsoft.Extensions.DependencyInjection;
using MyVyshKat.Infrastructure;
using MyVyshKat.Infrastructure.ConsoleUi;

using var provider = new ServiceCollection()
    .AddMyVyshKat()
    .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true });

provider.GetRequiredService<ConsoleMenu>().Run();
