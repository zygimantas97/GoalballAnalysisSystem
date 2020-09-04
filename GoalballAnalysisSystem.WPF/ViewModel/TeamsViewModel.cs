using GoalballAnalysisSystem.Domain.Models;
using GoalballAnalysisSystem.WPF.State.Users;
using GoalballAnalysisSystem.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace GoalballAnalysisSystem.WPF.ViewModel
{
    public class TeamsViewModel : BaseViewModel
    {
        private readonly IUserStore _userStore;
        private readonly ObservableCollection<Team> _teams;

        public IEnumerable<Team> Teams => _teams;

        public TeamsViewModel(IUserStore userStore)
        {
            _userStore = userStore;
            _teams = new ObservableCollection<Team>();
            _userStore.StateChanged += UserStore_StateChanged;
            ResetTeams();
        }

        private void ResetTeams()
        {
            _teams.Clear();
            if(_userStore.CurrentUser != null)
            {
                foreach (Team team in _userStore.CurrentUser.Teams)
                {
                    _teams.Add(team);
                }
            }
        }

        private void UserStore_StateChanged()
        {
            ResetTeams();
        }
    }
}
