using SQLite;
using SQLite.Net;
using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Core.DAL
{
    /// <summary>
	/// SQLite repository provides generic queries.
	/// </summary>
	public abstract class BaseSQLiteRepository : SQLiteConnection
    {


        /// <summary>
        /// Thread safe locker.
        /// </summary>
        protected static object _locker = new object();

        public BaseSQLiteRepository() : base(null,null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSQLiteRepository"/> class.
        /// </summary>
        public BaseSQLiteRepository(ISQLitePlatform platform, string path)
            : base(platform, path)
        {
        }

        /// <summary>
        /// Gets the item based on the id.
        /// </summary>
        public T Get<T>(int id = 0) where T : class, IEntity, new()
        {
            lock (_locker)
            {
                if (id == 0)
                {
                    return Table<T>().FirstOrDefault();
                }
                else
                {
                    return Table<T>().FirstOrDefault(x => x.Id == id);
                }
            }
        }

        /// <summary>
        /// Get item by condition
        /// </summary>
        public T Get<T>(Func<T, bool> filter) where T : class, IEntity, new()
        {
            lock (_locker)
            {
                return Table<T>().FirstOrDefault(filter);
            }
        }


        public int? GetMaxKey<T>(Func<T, int> filter) where T : class, IEntity, new()
        {
            lock (_locker)
            {
                return Table<T>().Max(filter);
            }
        }

        public int GetMinKey<T>(Func<T, int> filter) where T : class, IEntity, new()
        {
            lock (_locker)
            {
                return Table<T>().Min(filter);
            }
        }

        /// <summary>
        /// Gets all the items.
        /// </summary>
        public List<T> GetList<T>(Func<T, bool> filter) where T : class, IEntity, new()
        {
            lock (_locker)
            {
                return Table<T>().Where(filter).ToList();
            }
        }

        /// <summary>
        /// Gets all the items.
        /// </summary>
        public List<T> GetAll<T>() where T : class, IEntity, new()
        {
            lock (_locker)
            {
                return (from i in Table<T>()
                        select i).ToList();
            }
        }

        /// <summary>
        /// Saves the item.
        /// </summary>
        public int Save<T>(T item) where T : class, IEntity, new()
        {
            lock (_locker)
            {
                if (item is BaseAutoIncrease)
                {
                    if (item != null)
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
                    return -1;
                }
                else if (item != null)
                {
                    return InsertOrReplace(item);
                }
                return -1;
            }
        }

        /// <summary>
        /// Saves all the items.
        /// </summary>
        public void Save<T>(IList<T> items) where T : class, IEntity, new()
        {
            try
            {
                lock (_locker)
                {
                    BeginTransaction();

                    foreach (T item in items)
                    {
                        Save<T>(item);
                    }

                    Commit();
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Deletes the item by the id.
        /// </summary>
        public int Delete<T>(int id) where T : class, IEntity, new()
        {
            lock (_locker)
            {
                return Delete<T>(new T() { Id = id });
            }
        }

        /// <summary>
        /// Deletes all the items in table.
        /// </summary>
        public new int DeleteAll<T>() where T : class, IEntity, new()
        {
            lock (_locker)
            {
                return base.DeleteAll<T>();
            }
        }

        public new int Delete(object objectToDelete)
        {
            lock (_locker)
            {
                return base.Delete(objectToDelete);
            }
        }
    }
}
