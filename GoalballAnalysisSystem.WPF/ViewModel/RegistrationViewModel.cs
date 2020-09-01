﻿using GoalballAnalysisSystem.WPF.Model;
using GoalballAnalysisSystem.WPF.ViewModel.DatabaseServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.ViewModel
{
    public class RegistrationViewModel : BaseViewModel
    {
        public ICommand RegisterCommand { get; private set; }

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


        public RegistrationViewModel()
            :base()
        {
            ActiveUser = new User();
            RegisterCommand = null;
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