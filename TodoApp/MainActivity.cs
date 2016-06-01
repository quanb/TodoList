using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using TodoApp.DataLayer;
using System.Collections.Generic;
using System.Linq;
using Android.Support.V7.App;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace TodoApp
{
    [Activity(Label = "TodoApp", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class MainActivity : ActionBarActivity
    {
        private ListView mTaskListView;
        private ArrayAdapter<string> mAdapter;
        protected IList<Task> tasks;
        private SupportToolbar mToolbar;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            mTaskListView = (ListView)FindViewById(Resource.Id.list_todo);
            mToolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            SetSupportActionBar(mToolbar);
            updateUI();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.main_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_add_task:
                    var taskEditText = new EditText(this);
                    Android.Support.V7.App.AlertDialog dialog = new Android.Support.V7.App.AlertDialog.Builder(this)
                            .SetTitle("Add a new task")
                            .SetMessage("What do you want to do next?")
                            .SetView(taskEditText)
                            .SetPositiveButton("Add", (sender, args) =>
                            {
                                // Do something when this button is clicked.
                                var newTask = new Task()
                                {
                                    Title = taskEditText.Text
                                };
                                TaskRepository.SaveTask(newTask);
                                updateUI();
                            })
                            .SetNegativeButton("Cancel", (EventHandler<DialogClickEventArgs>)null)
                            .Create();
                    dialog.Show();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void updateUI()
        {
            tasks = TaskRepository.GetTasks().ToList();

            List<string> tasksTitle = new List<string>();

            foreach (var task in tasks)
            {
                tasksTitle.Add(task.Title);
            }

            if (mAdapter == null)
            {
                mAdapter = new ArrayAdapter<string>(this,
                        Resource.Layout.item_todo,
                        Resource.Id.task_title,
                        tasksTitle);
                mTaskListView.Adapter = (mAdapter);
            }
            else
            {
                mAdapter.Clear();
                mAdapter.AddAll(tasksTitle);
                mAdapter.NotifyDataSetChanged();
            }
        }
    }
}

