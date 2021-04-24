using GoalballAnalysisSystem.WPF.Services;
using GoalballAnalysisSystem.WPF.State.Authenticators;
using GoalballAnalysisSystem.WPF.State.Navigators;
using GoalballAnalysisSystem.WPF.State.Users;
using GoalballAnalysisSystem.WPF.View;
using GoalballAnalysisSystem.WPF.ViewModel;
using GoalballAnalysisSystem.WPF.ViewModel.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            IServiceProvider serviceProvider = CreateServiceProvider();
            
            Window window = serviceProvider.GetRequiredService<MainWindow>();
            window.Show();

            base.OnStartup(e);
        }

        private IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            // dependencies of services
            services.AddSingleton<IIdentityService, IdentityService>();
            services.AddSingleton<PlayersService>();
            services.AddSingleton<TeamsService>();
            services.AddSingleton<TeamPlayersService>();
            services.AddSingleton<GamesService>();
            services.AddSingleton<GamePlayersService>();
            services.AddSingleton<ProjectionsService>();

            // dependencies of view models
            services.AddSingleton<IGoalballAnalysisSystemViewModelFactory, GoalballAnalysisSystemViewModelFactory>();            

            services.AddSingleton<CreateViewModel<HomeViewModel>>(s => { return () => new HomeViewModel(s.GetRequiredService<IRenavigator>()); });
            services.AddSingleton<CreateViewModel<GamesViewModel>>(s => 
            { 
                return () => new GamesViewModel(
                    s.GetRequiredService<GamesService>(),
                    s.GetRequiredService<ProjectionsService>(),
                    s.GetRequiredService<GamePlayersService>(),
                    s.GetRequiredService<TeamsService>(),
                    s.GetRequiredService<TeamPlayersService>(),
                    s.GetRequiredService<PlayersService>()); 
            });
            services.AddSingleton<CreateViewModel<TeamsViewModel>>(s => 
            { 
                return () => new TeamsViewModel(
                    s.GetRequiredService<TeamsService>(),
                    s.GetRequiredService<TeamPlayersService>(),
                    s.GetRequiredService<PlayersService>()); 
            });
            services.AddSingleton<CreateViewModel<PlayersViewModel>>(s => 
            {
                return () => new PlayersViewModel(
                    s.GetRequiredService<PlayersService>());
            });
            services.AddSingleton<CreateViewModel<CalibrationViewModel>>(s => 
            { 
                return () => new CalibrationViewModel(
                    s.GetRequiredService<GamesService>(),
                    s.GetRequiredService<TeamsService>(),
                    s.GetRequiredService<PlayersService>(),
                    s.GetRequiredService<TeamPlayersService>()); 
            });
            services.AddSingleton<CreateViewModel<LoginViewModel>>(s =>
            {
                return () => new LoginViewModel(
                    s.GetRequiredService<IAuthenticator>(),
                    s.GetRequiredService<IRenavigator>());
            });
            services.AddSingleton<CreateViewModel<RegistrationViewModel>>(s =>
            {
                return () => new RegistrationViewModel(
                    s.GetRequiredService<IAuthenticator>(),
                    s.GetRequiredService<IRenavigator>());
            });

            // dependencies of state management
            services.AddSingleton<INavigator, Navigator>();
            services.AddSingleton<IRenavigator, Renavigator>();
            services.AddSingleton<IAuthenticator, Authenticator>();
            services.AddSingleton<IUserStore, UserStore>();
            
            // dependencies of main window
            services.AddScoped<MainViewModel>();
            services.AddScoped<MainWindow>(s => new MainWindow(s.GetRequiredService<MainViewModel>()));
            
            return services.BuildServiceProvider();
        }
    }
}
