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
using Android.Support.V4.Widget;

namespace ToDoList
{
    [Activity(Label = "ToDoList", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class MainActivity : AppCompatActivity
    {
        private ListView mTaskListView;
        private ArrayAdapter<string> mAdapter;
        protected IList<Task> tasks;
        private SupportToolbar mToolbar;
        private DrawerLayout mDrawerLayout;
        private MyActionBarDrawerToggle mDrawerToggle;
        private ListView mLeftDrawer;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            mTaskListView = (ListView)FindViewById(Resource.Id.list_todo);
            mToolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);
            SetSupportActionBar(mToolbar);

            mDrawerToggle = new MyActionBarDrawerToggle(
                this,                           //Host Activity
                mDrawerLayout,                  //DrawerLayout
                Resource.String.openDrawer,     //Opened Message
                Resource.String.closeDrawer     //Closed Message
            );

            mDrawerLayout.SetDrawerListener(mDrawerToggle);
            //SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(true);
            mDrawerToggle.SyncState();

            updateUI();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.main_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            mDrawerToggle.OnOptionsItemSelected(item);
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

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            mDrawerToggle.SyncState();
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            mDrawerToggle.OnConfigurationChanged(newConfig);
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

