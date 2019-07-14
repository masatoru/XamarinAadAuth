using System;
using Microsoft.Identity.Client;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinAadAuth.Front.Models;

namespace XamarinAadAuth.Front
{
    public partial class App : Application
    {
        public static IPublicClientApplication PCA = null;

        public static string[] Scopes = { "User.Read" };
        public static string Username = string.Empty;
        public static object ParentWindow { get; set; }

        public App()
        {
            InitializeComponent();

            PCA = PublicClientApplicationBuilder.Create(Constant.ClientId)
                .WithRedirectUri($"msal{Constant.ClientId}://auth")
                .Build();

            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
