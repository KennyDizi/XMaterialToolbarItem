using System;
using ToolbarCustomFont.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof (NavigationPage), typeof (CustomFontNavigationPageRenderer))]

namespace ToolbarCustomFont.iOS
{
    // https://blog.xamarin.com/custom-fonts-in-ios/
    public class CustomFontNavigationPageRenderer : NavigationRenderer
    {
        private const string CustomFontName = "materialicons.ttf";
        private readonly nfloat _customFontSize = 27.0f;

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (NavigationBar == null) return;

            SetNavBarStyle();
            SetNavBarItems();
        }

        private void SetNavBarStyle()
        {
            NavigationBar.ShadowImage = new UIImage();
            NavigationBar.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
            UINavigationBar.Appearance.ShadowImage = new UIImage();
            UINavigationBar.Appearance.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
        }

        private void SetNavBarItems()
        {
            var navPage = Element as NavigationPage;

            if (navPage == null) return;

            var textAttributes = new UITextAttributes
            {
                Font = UIFont.FromName(CustomFontName, _customFontSize)
            };

            var textAttributesHighlighted = new UITextAttributes
            {
                TextColor = Color.Black.ToUIColor(),
                Font = UIFont.FromName(CustomFontName, _customFontSize)
            };

            UIBarButtonItem.Appearance.SetTitleTextAttributes(textAttributes,
                UIControlState.Normal);
            UIBarButtonItem.Appearance.SetTitleTextAttributes(textAttributesHighlighted,
                UIControlState.Highlighted);
        }
    }
}