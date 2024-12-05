using AppSettings.Shared.Settings;
using AutoMapper;
using HobbyManagement.Business;
using HobbyManagement.Mapping;
using HobbyManagement.Services;
using HobbyManagement.Services.Csv;
using HobbyManagement.Services.Mock;
using HobbyManagement.Viewmodels;
using HobbyManagment.Data.Database;
using HobbyManagment.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace HobbyManagement;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    #region Fields

    /// <summary>
    /// The service provider.
    /// </summary>
    private IServiceProvider _serviceProvider = default!;

    #endregion

    #region Methods

    /// <summary>
    /// Adds Auto Mapper configuration to the service collection.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
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

    /// <summary>
    /// Creates a service provider.
    /// </summary>
    /// <returns><see cref="IServiceProvider"/></returns>
    private IServiceProvider CreateServiceProvider()
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        return serviceCollection.BuildServiceProvider();
    }

    /// <summary>
    /// Adds services to the service collection. 
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    private void ConfigureServices(IServiceCollection serviceCollection)
    {
        ConfigureAutoMapper(serviceCollection);

        serviceCollection.AddSingleton<IHobbyManagerViewModel, HobbyManagerViewModel>();
        serviceCollection.AddTransient<IHobbyViewModel, HobbyViewModel>();
        serviceCollection.AddTransient<IEditHobbyViewModel, EditHobbyViewModel>();
        serviceCollection.AddSingleton<IHobbyViewModelFactory, HobbyViewModelFactory>();
        serviceCollection.AddSingleton<ICsvService, CsvService>();
        serviceCollection.AddTransient<MainWindow>();
        serviceCollection.AddTransient<IHobbiesRepository, HobbiesRepository>();
        serviceCollection.AddTransient<IHobbyManager, HobbyManager>();
        serviceCollection.AddTransient<IMockDataService, MockDataService>();
        serviceCollection.AddTransient<IHobby, Hobby>();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile(AppSettingsHelper.AppSettingsFileName)
            .Build();

        serviceCollection.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString(AppSettingsHelper.DevDbConnectionStringKey));
        });
    }

    /// <inheritdoc/>
    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        _serviceProvider = CreateServiceProvider();

#if DEBUG
        var dbContext = _serviceProvider.GetService<ApplicationDbContext>();
        dbContext!.Database.Migrate();

        var mockService = _serviceProvider.GetRequiredService<IMockDataService>();
        await mockService.TrySeedHobbies();
#endif

        _serviceProvider.GetRequiredService<MainWindow>().Show();
    }

    #endregion
}
