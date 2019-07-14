
using Android.App;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinAadAuth.Front;
using XamarinAadAuth.Front.Droid;

[assembly: ExportRenderer(typeof(LoginPage), typeof(MainPageRenderer))]

namespace XamarinAadAuth.Front.Droid
{
    class MainPageRenderer : PageRenderer
    {
        public MainPageRenderer(Context context) : base(context)
        {

        }

        LoginPage page;

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            page = e.NewElement as LoginPage;
            var activity = this.Context as Activity;
        }

    }
}
