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
    }
}
