using GoalballAnalysisSystem.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GoalballAnalysisSystem.ViewModel.DatabaseServices
{
    public class SQLiteDatabaseService
    {
        public static string dbFile = Path.Combine(Environment.CurrentDirectory, "GAS_DB.db3");

        public static bool Insert<T>(T item)
        {
            bool result = false;

            using (SQLiteConnection connection = new SQLiteConnection(dbFile))
            {
                connection.CreateTable<T>();
                int numberOfRows = connection.Insert(item);
                if (numberOfRows > 0)
                    result = true;
            }

            return result;
        }

        public static bool Update<T>(T item)
        {
            bool result = false;

            using (SQLiteConnection connection = new SQLiteConnection(dbFile))
            {
                connection.CreateTable<T>();
                int numberOfRows = connection.Update(item);
                if (numberOfRows > 0)
                    result = true;
            }

            return result;
        }

        public static bool Delete<T>(T item)
        {
            bool result = false;

            using (SQLiteConnection connection = new SQLiteConnection(dbFile))
            {
                connection.CreateTable<T>();
                int numberOfRows = connection.Delete(item);
                if (numberOfRows > 0)
                    result = true;
            }

            return result;
        }

        public static User GetUser(string email)
        {
            User user = null;
            using(SQLiteConnection connection = new SQLiteConnection(dbFile))
            {
                connection.CreateTable<User>();
                user = connection.Table<User>().Where(u => u.Email == email).FirstOrDefault();
            }
            return user;
        }

        public static List<Player> GetPlayers(int userId)
        {
            List<Player> players = null;
            using (SQLiteConnection connection = new SQLiteConnection(dbFile))
            {
                connection.CreateTable<Player>();
                players = connection.Table<Player>().Where(p => p.UserId == userId).ToList();
            }
            return players;
        }

        public static List<Team> GetTeams(int userId)
        {
            List<Team> teams = null;
            using (SQLiteConnection connection = new SQLiteConnection(dbFile))
            {
                connection.CreateTable<Team>();
                teams = connection.Table<Team>().Where(t => t.UserId == userId).ToList();
            }
            return teams;
        }

        public static List<Game> GetGames(int userId)
        {
            List<Game> games = null;
            using (SQLiteConnection connection = new SQLiteConnection(dbFile))
            {
                connection.CreateTable<Game>();
                games = connection.Table<Game>().Where(g => g.UserId == userId).ToList();
            }
            return games;
        }

        public static List<Throw> GetThrows(int gameId)
        {
            List<Throw> throws = null;
            using (SQLiteConnection connection = new SQLiteConnection(dbFile))
            {
                connection.CreateTable<Throw>();
                throws = connection.Table<Throw>().Where(t => t.GameId == gameId).ToList();
            }
            return throws;
        }
    }
}
