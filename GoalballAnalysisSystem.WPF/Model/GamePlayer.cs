using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GoalballAnalysisSystem.Model
{
    public class GamePlayer : INotifyPropertyChanged
    {
        // reikia patikslinti ar reikalingas [Indexed]
        private int gameId;
        [PrimaryKey, Indexed]
        public int GameId
        {
            get { return gameId; }
            set
            {
                gameId = value;
                OnPropertyChanged("GameId");
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

        private DateTime startTime;

        public DateTime StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                OnPropertyChanged("StartTime");
            }
        }

        private DateTime endTime;

        public DateTime EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                OnPropertyChanged("EndTime");
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
