using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Util;
using Android.Views;
using Java.Lang.Reflect;
using Java.Lang;
using System;
using Android.Graphics;

namespace ToolbarCustomFont.Droid.AppCompat
{
    [Activity(Label = "ToolbarCustomFont.Droid.AppCompat", MainLauncher = true, Theme = "@style/MyTheme")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;
            global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity.TabLayoutResource = Resource.Layout.tabs;

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App());
        }

        #region material font toolbar item

        private static Class _actionMenuItemViewClass;
        private static Constructor _actionMenuItemViewConstructor;

        private static Typeface _typeface;

        public static Typeface Typeface => _typeface ?? (_typeface =
                                               Typeface.CreateFromAsset(
                                                   Xamarin.Forms.Forms.Context.ApplicationContext.Assets,
                                                   "Fonts/materialicons.ttf"));

        public override View OnCreateView(string name, Context context, IAttributeSet attrs)
        {
            if (name.Equals("android.support.v7.view.menu.ActionMenuItemView",
                StringComparison.InvariantCultureIgnoreCase))
            {
                System.Diagnostics.Debug.WriteLine(name);
                var customLoginIfNeeded = CreateCustomToolbarItem(name, context, attrs);
                if (customLoginIfNeeded != null)
                    return customLoginIfNeeded;
            }

            return base.OnCreateView(name, context, attrs);
        }

        public override View OnCreateView(View parent, string name, Context context, IAttributeSet attrs)
        {
            if (name.Equals("android.support.v7.view.menu.ActionMenuItemView",
                StringComparison.InvariantCultureIgnoreCase))
            {
                System.Diagnostics.Debug.WriteLine(name);
                var customLoginIfNeeded = CreateCustomToolbarItem(name, context, attrs);
                if (customLoginIfNeeded != null)
                    return customLoginIfNeeded;
            }

            return base.OnCreateView(parent, name, context, attrs);
        }

        private View CreateCustomToolbarItem(string name, Java.Lang.Object context, IAttributeSet attrs)
        {
            // android.support.v7.widget.Toolbar
            // android.support.v7.view.menu.ActionMenuItemView
            View view;

            try
            {
                if (_actionMenuItemViewClass == null)
                    _actionMenuItemViewClass = ClassLoader.LoadClass(name);
            }
            catch (ClassNotFoundException ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
                return null;
            }

            if (_actionMenuItemViewClass == null)
                return null;

            if (_actionMenuItemViewConstructor == null)
            {
                try
                {
                    _actionMenuItemViewConstructor =
                        _actionMenuItemViewClass.GetConstructor(Class.FromType(typeof(Context)),
                            Class.FromType(typeof(IAttributeSet)));
                }
                catch (SecurityException ex)
                {
                    System.Diagnostics.Debug.Write(ex.Message);
                    return null;
                }
                catch (NoSuchMethodException ex)
                {
                    System.Diagnostics.Debug.Write(ex.Message);
                    return null;
                }
            }
            if (_actionMenuItemViewConstructor == null)
                return null;

            try
            {
                Java.Lang.Object[] args = { context, (Java.Lang.Object)attrs };
                view = (View)_actionMenuItemViewConstructor.NewInstance(args);
            }
            catch (IllegalArgumentException ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
                return null;
            }
            catch (InstantiationException ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
                return null;
            }
            catch (IllegalAccessException ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
                return null;
            }
            catch (InvocationTargetException ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
                return null;
            }
            if (null == view)
                return null;

            var v = view;
            var handler = new Handler();
            handler.Post(() =>
            {

                try
                {
                    var layout = v as LinearLayout;
                    if (layout != null)
                    {
                        var ll = layout;
                        for (var i = 0; i < ll.ChildCount; i++)
                        {
                            var button = ll.GetChildAt(i) as Button;

                            var title = button?.Text;

                            if (!string.IsNullOrEmpty(title) && title.Length == 1)
                            {
                                button.SetTypeface(Typeface, TypefaceStyle.Normal);
                                button.SetTextSize(ComplexUnitType.Sp, size: 27);
                            }
                        }
                    }
                    else if (v is TextView)
                    {
                        var tv = (TextView)v;
                        var title = tv.Text;

                        if (!string.IsNullOrEmpty(title) && title.Length == 1)
                        {
                            tv.SetTypeface(Typeface, TypefaceStyle.Normal);
                            tv.SetTextSize(ComplexUnitType.Sp, size: 27);
                        }
                    }
                }
                catch (ClassCastException ex)
                {
                    System.Diagnostics.Debug.Write(ex.Message);
                }
            });

            return view;
        }

        #endregion
    }
}