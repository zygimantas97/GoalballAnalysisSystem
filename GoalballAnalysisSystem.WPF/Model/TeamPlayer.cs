using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GoalballAnalysisSystem.Model
{
    public class TeamPlayer : INotifyPropertyChanged
    {
        // reikia patikslinti ar reikalingas [Indexed]
        private int playerId;
        [PrimaryKey, Indexed]
        public int PlayerId
        {
            get { return playerId; }
            set
            {
                playerId = value;
                OnPropertyChanged("PlayerId");
            }
        }

        // reikia patikslinti ar reikalingas [Indexed]
        private int teamId;
        [PrimaryKey, Indexed]
        public int TeamId
        {
            get { return teamId; }
            set
            {
                teamId = value;
                OnPropertyChanged("TeamId");
            }
        }

        private int role;

        public int Role
        {
            get { return role; }
            set
            {
                role = value;
                OnPropertyChanged("Role");
            }
        }

        private int number;

        public int Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
