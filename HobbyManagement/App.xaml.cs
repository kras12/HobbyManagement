using AutoMapper;
using HobbyManagement.Mapping;
using HobbyManagement.Services;
using HobbyManagement.Viewmodels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace HobbyManagement;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IServiceProvider _serviceProvider = default!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        _serviceProvider = CreateServiceProvider();
        _serviceProvider.GetRequiredService<MainWindow>().Show();
    }

    private void ConfigureAutoMapper(IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IMapper>((service) =>
        {
            var mapperConfig = new MapperConfiguration(x =>
            {
                x.AddProfile(new AutoMapperProfile());
                x.ConstructServicesUsing(x => service.GetRequiredService(x));
            });

            return mapperConfig.CreateMapper();
        });
    }

    private void ConfigureServices(IServiceCollection serviceCollection)
    {
        ConfigureAutoMapper(serviceCollection);

        serviceCollection.AddSingleton<IHobbyManagerViewModel, HobbyManagerViewModel>();
        serviceCollection.AddTransient<IHobbyViewModel, HobbyViewModel>();
        serviceCollection.AddSingleton<IHobbyViewModelFactory, HobbyViewModelFactory>();
        serviceCollection.AddTransient<MainWindow>();
    }

    private IServiceProvider CreateServiceProvider()
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        return serviceCollection.BuildServiceProvider();
    }
}
