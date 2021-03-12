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

        public ObservableCollection<Team> ListOfTeams
        {
            get { return _teams; }
        }


        public TeamsViewModel(IUserStore userStore)
        {
            _teams = AddFakeData();
        }

        public ObservableCollection<Team> AddFakeData()
        {
            ObservableCollection<Team> list = new ObservableCollection<Team>();


            for (int i = 0; i < 50; i++)
            {
                Team a = new Team();
                a.Name = "Team" + i;
                a.Description = "Description" + i;
                a.Id = i;
                list.Add(a);

            }

            return list;
        }
    }
}
