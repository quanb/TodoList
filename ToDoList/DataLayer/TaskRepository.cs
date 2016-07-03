using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite;
using TodoApp.Core.DataLayer;
using SQLite.Net.Platform.XamarinAndroid;

namespace TodoApp.DataLayer
{
    public class TaskRepository
    {
        public static string DatabaseFilePath
        {
            get
            {
                var sqliteFilename = "TaskDB.db3";

                string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); ;
                var path = Path.Combine(libraryPath, sqliteFilename);
                return path;
            }
        }

        TaskDatabase db = null;
        protected static TaskRepository me;
        static TaskRepository()
        {
            me = new TaskRepository();
        }
        protected TaskRepository()
        {
            db = new TaskDatabase(new SQLitePlatformAndroid(), DatabaseFilePath);
        }

        public static Task GetTask(int id)
        {
            return me.db.GetTask(id);
        }

        public static IEnumerable<Task> GetTasks()
        {
            return me.db.GetTasks();
        }

        public static IEnumerable<Task> GetTasksByType(int type)
        {
            return me.db.GetTasks().Where(c => c.Type == type);
        }

        public static int SaveTask(Task item)
        {
            return me.db.SaveTask(item);
        }

        public static int DeleteTask(Task item)
        {
            return me.db.DeleteTask(item);
        }
    }
}