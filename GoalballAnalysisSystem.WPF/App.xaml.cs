using GoalballAnalysisSystem.Domain.Models;
using GoalballAnalysisSystem.Domain.Services;
using GoalballAnalysisSystem.EntityFramework;
using GoalballAnalysisSystem.EntityFramework.Services;
using GoalballAnalysisSystem.WPF.State.Authenticators;
using GoalballAnalysisSystem.WPF.State.Navigators;
using GoalballAnalysisSystem.WPF.State.Users;
using GoalballAnalysisSystem.WPF.View;
using GoalballAnalysisSystem.WPF.ViewModel;
using GoalballAnalysisSystem.WPF.ViewModel.Factories;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // later maybe delete
        public static ICommand NavigationCommand { get; set; }
        public static string Message = "";
        public static int userId;

        protected override async void OnStartup(StartupEventArgs e)
        {
            IServiceProvider serviceProvider = CreateServiceProvider();
            //IAuthenticationService authenticationService = serviceProvider.GetRequiredService<IAuthenticationService>();
            //await authenticationService.Register("Zygimantas", "Matusevicius", "zygimantas1997@gmail.com", "password", "password");

            Window window = serviceProvider.GetRequiredService<MainWindow>();
            window.Show();

            base.OnStartup(e);
        }

        private IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            // Add all services
            services.AddSingleton<GoalballAnalysisSystemDbContextFactory>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IDataService<User>, UserDataService>();
            services.AddSingleton<IUserDataService, UserDataService>();

            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            // add all view model factories
            services.AddSingleton<IGoalballAnalysisSystemViewModelFactory, GoalballAnalysisSystemViewModelFactory>();            

            services.AddSingleton<CreateViewModel<HomeViewModel>>(s => { return () => new HomeViewModel(); });
            services.AddSingleton<CreateViewModel<GamesViewModel>>(s => { return () => new GamesViewModel(); });
            services.AddSingleton<CreateViewModel<TeamsViewModel>>(s => { return () => new TeamsViewModel(); });
            services.AddSingleton<CreateViewModel<PlayersViewModel>>(s => { return () => new PlayersViewModel(); });
            services.AddSingleton<Renavigator<HomeViewModel>>();
            services.AddSingleton<CreateViewModel<LoginViewModel>>(s =>
            {
                return () => new LoginViewModel(
                    s.GetRequiredService<IAuthenticator>(),
                    s.GetRequiredService<Renavigator<HomeViewModel>>());
            });

            services.AddSingleton<INavigator, Navigator>();
            services.AddSingleton<IAuthenticator, Authenticator>();
            // use for updating CurrentUser object
            services.AddSingleton<IUserStore, UserStore>();
            services.AddScoped<MainViewModel>();

            services.AddScoped<MainWindow>(s => new MainWindow(s.GetRequiredService<MainViewModel>()));
            

            return services.BuildServiceProvider();
        }
    }
}
