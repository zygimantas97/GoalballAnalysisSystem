﻿using GoalballAnalysisSystem.WPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace GoalballAnalysisSystem.WPF.ViewModel
{
    public delegate T CreateViewModel<T>() where T : BaseViewModel;

    public class BaseViewModel : ObservableObject
    {
        
    }
}
