using GoalballAnalysisSystem.WPF.Model;
using GoalballAnalysisSystem.WPF.ViewModel.Commands;
using GoalballAnalysisSystem.WPF.ViewModel.DatabaseServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginCommand LoginCommand { get; private set; }

        private User activeUser;        

        public User ActiveUser
        {
            get { return activeUser; }
            set
            {
                activeUser = value;
                //OnPropertyChanged("ActiveUser");
            }
        }

        public LoginViewModel()
            :base()
        {
            ActiveUser = new User();
            LoginCommand = new LoginCommand(this);
        }

        public void Login()
        {
            var user = SQLiteDatabaseService.GetUser(ActiveUser.Email);
            if(user != null && user.Password == ActiveUser.Password)
            {
                App.userId = user.Id;
                App.NavigationCommand.Execute("HomeViewModel");
            }
        }
    }
}
