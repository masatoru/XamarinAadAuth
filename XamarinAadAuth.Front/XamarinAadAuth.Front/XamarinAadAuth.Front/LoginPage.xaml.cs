using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XamarinAadAuth.Front.Models;

namespace XamarinAadAuth.Front
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class LoginPage : ContentPage
    {
        AuthenticationResult AuthResult = null;

        public LoginPage()
        {
            InitializeComponent();
        }


        private async void OnLogin(object sender, EventArgs e)
        {
            IEnumerable<IAccount> accounts = await App.PCA.GetAccountsAsync();
            try
            {
                if (btnSignInSignOut.Text == "Sign in")
                {
                    // let's see if we have a user in our belly already
                    try
                    {

                        IAccount firstAccount = accounts.FirstOrDefault();
                        AuthResult = await App.PCA.AcquireTokenSilent(App.Scopes, firstAccount)
                                              .ExecuteAsync();
                        await RefreshUserDataAsync(AuthResult.AccessToken).ConfigureAwait(false);
                        Device.BeginInvokeOnMainThread(() => { btnSignInSignOut.Text = "Sign out"; });
                    }
                    catch (MsalUiRequiredException ex)
                    {
                        try
                        {
                            AuthResult = await App.PCA.AcquireTokenInteractive(App.Scopes)
                                                      .WithParentActivityOrWindow(App.ParentWindow)
                                                      .ExecuteAsync();

                            await RefreshUserDataAsync(AuthResult.AccessToken);
                            Device.BeginInvokeOnMainThread(() => { btnSignInSignOut.Text = "Sign out"; });
                        }
                        catch (Exception ex2)
                        {
                            await DisplayAlert("SignIn",ex2.Message,"OK");
                        }
                    }
                }
                else
                {
                    while (accounts.Any())
                    {
                        await App.PCA.RemoveAsync(accounts.FirstOrDefault());
                        accounts = await App.PCA.GetAccountsAsync();
                    }

                    slUser.IsVisible = false;
                    Device.BeginInvokeOnMainThread(() => { btnSignInSignOut.Text = "Sign in"; });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("SignIn", ex.Message, "OK");
            }
        }

        private void OnLogout(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public async Task RefreshUserDataAsync(string token)
        {
            //get data from API
            HttpClient client = new HttpClient();
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me");
            message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
            HttpResponseMessage response = await client.SendAsync(message);
            string responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                JObject user = JObject.Parse(responseString);

                slUser.IsVisible = true;

                Device.BeginInvokeOnMainThread(() =>
                {

                    lblDisplayName.Text = user["displayName"].ToString();
                    lblGivenName.Text = user["givenName"].ToString();
                    lblId.Text = user["id"].ToString();
                    lblSurname.Text = user["surname"].ToString();
                    lblUserPrincipalName.Text = user["userPrincipalName"].ToString();

                    // just in case
                    btnSignInSignOut.Text = "Sign out";
                });
            }
            else
            {
                await DisplayAlert("Something went wrong with the API call", responseString, "Dismiss");
            }
        }

        private async void OnWebApi(object sender, EventArgs e)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthResult.AccessToken);
                client.DefaultRequestHeaders.ExpectContinue = false;

                // JSONデータをPOST
                HttpResponseMessage response = await client.GetAsync(Constant.BaseUrl + "/api/todo/get");

                if (response.IsSuccessStatusCode)
                {

                    // Read the response and data-bind to the GridView to display To Do items.
                    string resultText = await response.Content.ReadAsStringAsync();

                    await DisplayAlert("WebApi", resultText, "OK");
                }
                else
                {
                    await DisplayAlert("WebApi", response.ToString(), "OK");
                }
            }
        }
    }
}
