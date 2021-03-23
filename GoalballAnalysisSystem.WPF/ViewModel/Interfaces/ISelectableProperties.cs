using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using GoalballAnalysisSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.WPF.ViewModel.Interfaces
{
    interface ISelectableProperties
    {
        void ChangeSelectedObject(object parameter);
        void ChangeEditMode();
        void DeleteSelectedObject(object parameter);

    }
}
