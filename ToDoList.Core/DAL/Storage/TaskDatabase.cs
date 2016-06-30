using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite;
using SQLite.Net;
using SQLite.Net.Interop;

namespace TodoApp.Core.DataLayer
{
    public class TaskDatabase : SQLiteConnection
    {
        static object locker = new object();

        public TaskDatabase(ISQLitePlatform platform, string path)
            : base(platform, path)
        {
            // create the tables
            CreateTable<Task>();
        }

        public IEnumerable<Task> GetTasks()
        {
            lock (locker)
            {
                return (from i in Table<Task>() select i).ToList();
            }
        }

        public Task GetTask(int id)
        {
            lock (locker)
            {
                return Table<Task>().FirstOrDefault(x => x.Id == id);
            }
        }

        public int SaveTask(Task item)
        {
            lock (locker)
            {
                if (item.Id != 0)
                {
                    Update(item);
                    return item.Id;
                }
                else
                {
                    return Insert(item);
                }
            }
        }

        //		public int DeleteTask(int id) 
        //		{
        //			lock (locker) {
        //				return Delete<Task> (new Task () { Id = id });
        //			}
        //		}
        public int DeleteTask(Task Task)
        {
            lock (locker)
            {
                return Delete<Task>(Task.Id);
            }
        }
    }
}