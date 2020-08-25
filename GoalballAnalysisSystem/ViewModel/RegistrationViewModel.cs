using GoalballAnalysisSystem.Model;
using GoalballAnalysisSystem.ViewModel.Commands;
using GoalballAnalysisSystem.ViewModel.DatabaseServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace GoalballAnalysisSystem.ViewModel
{
    public class RegistrationViewModel : BaseViewModel
    {
        public RegisterCommand RegisterCommand { get; private set; }

        private User activeUser;

        public User ActiveUser
        {
            get { return activeUser; }
            set
            {
                activeUser = value;
                OnPropertyChanged("ActiveUser");
            }
        }


        public RegistrationViewModel()
            :base()
        {
            ActiveUser = new User();
            RegisterCommand = new RegisterCommand(this);
        }

        public void Register()
        {
            bool result = SQLiteDatabaseService.Insert(ActiveUser);
            if (result)
            {
                App.Message = "Registration was successful.";
                App.NavigationCommand.Execute("LoginViewModel");
            }
        }
    }
}
