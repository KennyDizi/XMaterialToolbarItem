using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Android.Util;
using Xamarin.Forms;

namespace ToolbarCustomFont.Droid.AppCompat
{
    public class CustomFontSpan : MetricAffectingSpan
    {
        private static readonly LruCache TypefaceCache = new LruCache(5);

        private readonly Typeface _typeFace;

        public CustomFontSpan(string typefaceName)
        {
            _typeFace = (Typeface) TypefaceCache.Get(typefaceName);

            if (_typeFace == null)
            {
                _typeFace = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets, typefaceName);
                TypefaceCache.Put(typefaceName, _typeFace);
            }
        }

        public override void UpdateMeasureState(TextPaint p)
        {
            p.SetTypeface(_typeFace);
            p.Flags = p.Flags | PaintFlags.SubpixelText;
        }

        public override void UpdateDrawState(TextPaint tp)
        {
            tp.SetTypeface(_typeFace);
            tp.Flags = tp.Flags | PaintFlags.SubpixelText;
        }
    }
}