using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GoalballAnalysisSystem.WPF.Model
{
    public class Throw : INotifyPropertyChanged
    {
        private int id;
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        private int x1;

        public int X1
        {
            get { return x1; }
            set
            {
                x1 = value;
                OnPropertyChanged("X1");
            }
        }

        private int y1;

        public int Y1
        {
            get { return y1; }
            set
            {
                y1 = value;
                OnPropertyChanged("Y1");
            }
        }

        private int x2;

        public int X2
        {
            get { return x2; }
            set
            {
                x2 = value;
                OnPropertyChanged("X2");
            }
        }

        private int y2;

        public int Y2
        {
            get { return y2; }
            set
            {
                y2 = value;
                OnPropertyChanged("Y2");
            }
        }

        private double speed;

        public double Speed
        {
            get { return speed; }
            set
            {
                speed = value;
                OnPropertyChanged("Speed");
            }
        }

        private int gameId;
        [Indexed]
        public int GameId
        {
            get { return gameId; }
            set
            {
                gameId = value;
                OnPropertyChanged("GameId");
            }
        }

        private int teamId;
        [Indexed]
        public int TeamId
        {
            get { return teamId; }
            set
            {
                teamId = value;
                OnPropertyChanged("TeamId");
            }
        }

        private int playerId;
        [Indexed]
        public int PlayerId
        {
            get { return playerId; }
            set
            {
                playerId = value;
                OnPropertyChanged("PlayerId");
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
