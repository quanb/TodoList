using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Core.DAL.Storage
{
    public class TaskSQLiteRepository : BaseSQLiteRepository
    {
        public TaskSQLiteRepository(ISQLitePlatform platform, string path)
            : base(platform, path)
        {
            Debug.WriteLine(">>>>>SQL:" + path);
            CreateSchema();
        }

        private void CreateSchema()
        {
            

        }
    }
}
