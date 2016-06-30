using SQLite;
using SQLite.Net.Attributes;
using System;


namespace TodoApp.Core.DataLayer
{
    public class Task
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        public string Title { get; set; }
    }
}