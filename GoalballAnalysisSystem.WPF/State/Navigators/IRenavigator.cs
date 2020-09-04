using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.WPF.State.Navigators
{
    public interface IRenavigator
    {
        void Renavigate(ViewType viewType);
    }
}
