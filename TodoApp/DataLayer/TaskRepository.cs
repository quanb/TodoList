using System;
using System.Collections.Generic;

namespace TodoApp.DataLayer
{
    public class TaskRepository
    {
        TaskDatabase db = null;
        protected static TaskRepository me;
        static TaskRepository()
        {
            me = new TaskRepository();
        }
        protected TaskRepository()
        {
            db = new TaskDatabase(TaskDatabase.DatabaseFilePath);
        }

        public static Task GetTask(int id)
        {
            return me.db.GetTask(id);
        }

        public static IEnumerable<Task> GetTasks()
        {
            return me.db.GetTasks();
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