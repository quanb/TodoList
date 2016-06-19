using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.Content;
using Android.Graphics;
using System.Collections.Generic;

namespace ToDoList
{
    public class NavigationDrawerAdapter : ArrayAdapter<string>
    {
        public int SelectedItem { get; set; }

        public NavigationDrawerAdapter(Context context, int resource, List<string> objects) : base(context, resource, objects)
        {
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            TextView view = (TextView)base.GetView(position, convertView, parent);
            view.Text = GetItem(position);
            if (position == SelectedItem)
            {
                view.SetTextColor(ContextCompat.GetColorStateList(Context, Resource.Color.colorPrimary));
                view.Typeface = Typeface.DefaultBold;
            }
            else
            {
                view.SetTextColor(ContextCompat.GetColorStateList(Context, Resource.Color.primary_text_default_material_light));
            }
            return view;
        }
    }
}