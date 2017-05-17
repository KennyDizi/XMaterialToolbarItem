using System;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Lang.Reflect;
using Android.OS;
using Android.Graphics;

/*** Based on http://stackoverflow.com/a/5205945/5064986 ***/
namespace ToolbarCustomFont.Droid.AppCompat
{
    public class CustomLayoutInflaterFactory : Java.Lang.Object, Android.Support.V4.View.ILayoutInflaterFactory
    {
        private static Class _actionMenuItemViewClass;
        private static Constructor _actionMenuItemViewConstructor;

        private static Typeface _typeface;

        public static Typeface Typeface => _typeface ?? (_typeface =
                                               Typeface.CreateFromAsset(
                                                   Xamarin.Forms.Forms.Context.ApplicationContext.Assets,
                                                   "Fonts/fontawesome.ttf"));

        public View OnCreateView(View parent, string name, Context context, IAttributeSet attrs)
        {
            System.Diagnostics.Debug.WriteLine(name);

            if (name.Equals("android.support.v7.internal.view.menu.ActionMenuItemView",
                StringComparison.InvariantCultureIgnoreCase))
            {
                View view;

                try
                {
                    if (_actionMenuItemViewClass == null)
                        _actionMenuItemViewClass = ClassLoader.SystemClassLoader.LoadClass(name);
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
                    Java.Lang.Object[] args = {context, (Java.Lang.Object) attrs};
                    view = (View) _actionMenuItemViewConstructor.NewInstance(args);
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
                                    button.SetTextSize(ComplexUnitType.Mask, size: 25);
                                }
                            }
                        }
                        else if (v is TextView)
                        {
                            var tv = (TextView) v;
                            var title = tv.Text;

                            if (!string.IsNullOrEmpty(title) && title.Length == 1)
                            {
                                tv.SetTypeface(Typeface, TypefaceStyle.Normal);                                
                            }
                        }
                    }
                    catch (ClassCastException)
                    {
                    }
                });

                return view;
            }

            return null;
        }
    }
}