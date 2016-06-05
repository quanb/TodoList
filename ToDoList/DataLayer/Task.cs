using SQLite;
using System;


namespace TodoApp.DataLayer
{
    public class Task
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        public string Title { get; set; }
    }
}