using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GoalballAnalysisSystem.WPF.Model
{
    public class Game : INotifyPropertyChanged
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

        private string title;
        [MaxLength(50)]
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }

        private string comment;

        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                OnPropertyChanged("Comment");
            }
        }

        private int userId;
        [Indexed]
        public int UserId
        {
            get { return userId; }
            set
            {
                userId = value;
                OnPropertyChanged("UserId");
            }
        }

        private int team1Id;
        [Indexed]
        public int Team1Id
        {
            get { return team1Id; }
            set
            {
                team1Id = value;
                OnPropertyChanged("Team1Id");
            }
        }

        private int team2Id;
        [Indexed]
        public int Team2Id
        {
            get { return team2Id; }
            set
            {
                team2Id = value;
                OnPropertyChanged("Team2Id");
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
