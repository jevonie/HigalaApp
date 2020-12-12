using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HigalaApp.Models;
using HigalaApp.Data;
using HigalaApp.Services;
using System.Diagnostics;
using HigalaApp.Utility;

namespace HigalaApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        RestService _restService;
        DataServices _dataService;
        public LoginPage()
        {
            InitializeComponent();
            _restService = new RestService();
            _dataService = new DataServices();
           
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BackgroundImageSource = ImageSource.FromResource("HigalaApp.Views.Image.background.jpg");
            higalalogo.Source = ImageSource.FromResource("HigalaApp.Views.Image.HigalaNew.png");
            CheckAppVersion();
        }
        async void OnLoginClick(object sender, EventArgs e)
        {
            string username_Input = txtUsername.Text;
            string password_Input = txtPassword.Text;
            if (username_Input == null)
            {
                await DisplayAlert("Alert", "please enter username", "ok");
            }
            else if (password_Input == null)
            {
                await DisplayAlert("Alert", "please enter password", "ok");
            }
            else
            {
                ai.IsRunning = true;
                aiLayout.IsVisible = true;
                var utility = new UtilityProvider();
                ClientLoginOnline Locallogin = await App.Database.ClientLoginAsync(username_Input.Trim());
                Debug.WriteLine("\tLOCAL LOGIN USER {0}", username_Input.Trim());
                Debug.WriteLine("\tLOCAL LOGIN {0}", Locallogin);
                if (Locallogin != null)
                {
                    //Check if Online and Update password
                    var current = Connectivity.NetworkAccess;
                    if (current == NetworkAccess.Internet)
                    {
                        Debug.WriteLine("\tTIBONG {0}", "CONNECTING ONLINE");
                        ClientLoginOnline login = await _restService.GetClientDataAsync(GenerateRequestUri(ConstantData.HigalaApi + "logincustomer", username_Input.Trim(), password_Input.Trim()));
                        if (login != null)
                        {
                            ClientLoginOnline loginuser = await App.Database.GetUserByCustomerIDAsync(login.customer_id);
                            if (loginuser != null)
                            {
                                loginuser.customer_username = username_Input.Trim();
                                loginuser.Passwords = utility.getEncodeString(password_Input.Trim());
                                loginuser.is_active = "1";
                                Debug.WriteLine("\tINFO {0}", "Update local login" + await App.Database.SaveClientLoginAsync(loginuser));
                            }

                            CustomerDataOnline customerOnline = await _restService.GetClientDataDetailsAsync(ConstantData.HigalaApi + "getcustomer/" + login.customer_id);
                            if (customerOnline != null)
                            {
                                CustomerDataOnline customerLocal = await App.Database.GetCustomerByIDAsync(customerOnline.customer_id);
                                if (customerLocal != null)
                                {
                                    Debug.WriteLine("\tTIBONG {0}", "Local Update Customer Details");
                                    customerOnline.ID = customerLocal.ID;
                                    var item = await App.Database.SaveCustomerAsync(customerOnline);
                                }
                            }

                        }
                    }

                    //Initiate login user

                    Debug.WriteLine("\tTIBONG {0}", Locallogin.Passwords);
                    var stringyHash = utility.getEncodeString(password_Input.Trim());
                    if (Locallogin.Passwords == stringyHash)
                    {
                        Debug.WriteLine("\tTIBONG {0} Inside the login");
                        CustomerDataOnline userDetails = await App.Database.GetCustomerByIDAsync(Locallogin.customer_id);
                        if (userDetails != null)
                        {
                            App.UserID = userDetails.customer_id;
                            App.CustomerName = userDetails.customer_firstname + " " + userDetails.customer_lastname + " " + userDetails.customer_extension;
                            App.QrCode = userDetails.QrCombination;
                            Debug.WriteLine("\tQR_CODE {0}", userDetails.QrCombination);
                            CheckLocalData();
                        }
                        else
                        {
                            await DisplayAlert("Failed", "User data not found", "ok");
                            aiLayout.IsVisible = false;
                            ai.IsRunning = false;
                        }

                    }
                    else
                    {
                        await DisplayAlert("Login Failed", "Username/Password incorrect", "ok");
                        aiLayout.IsVisible = false;
                        ai.IsRunning = false;
                    }

                }
                else
                {
                    //first login should have internet

                    var current = Connectivity.NetworkAccess;
                    if (current == NetworkAccess.Internet)
                    {
                        ClientLoginOnline login = await _restService.GetClientDataAsync(GenerateRequestUri(ConstantData.HigalaApi + "logincustomer", username_Input.Trim(), password_Input.Trim()));
                        if (login != null)
                        {

                            Debug.WriteLine("\tINFOR {0}", "login using remote");
                            ClientLoginOnline loginuser = await App.Database.GetUserByCustomerIDAsync(login.customer_id);
                            if (loginuser != null)
                            {
                                loginuser.customer_username = username_Input.Trim();
                                loginuser.Passwords = utility.getEncodeString(password_Input.Trim());
                                loginuser.is_active = "1";
                                Debug.WriteLine("\tINFO {0}", "update login local" + await App.Database.SaveClientLoginAsync(loginuser));
                            }
                            else
                            {
                                ClientLoginOnline user = new ClientLoginOnline();
                                user.customer_id = login.customer_id;
                                user.customer_username = username_Input.Trim();
                                user.Passwords = utility.getEncodeString(password_Input.Trim());
                                user.is_active = "1";
                                Debug.WriteLine("\tINFO {0}", "insert login local" + await App.Database.SaveClientLoginAsync(user));
                            }

                            CustomerDataOnline customerdata = await _restService.GetClientDataDetailsAsync(ConstantData.HigalaApi + "getcustomer/" + login.customer_id);
                            if (customerdata != null)
                            {
                                CustomerDataOnline details = await App.Database.GetCustomerByIDAsync(customerdata.customer_id);
                                if (details != null)
                                {
                                    Debug.WriteLine("\tTIBONG {0}", "Update");
                                    customerdata.ID = details.ID;
                                    var item = await App.Database.SaveCustomerAsync(customerdata);
                                }
                                else
                                {
                                    Debug.WriteLine("\tTIBONG {0}", "Insert");
                                    var item = await App.Database.SaveCustomerAsync(customerdata);
                                }

                                App.UserID = customerdata.customer_id;
                                App.CustomerName = customerdata.customer_firstname + " " + customerdata.customer_lastname + " " + customerdata.customer_extension;
                                App.QrCode = customerdata.QrCombination;
                                CheckLocalData();
                            }
                            else
                            {
                                await DisplayAlert("Failed", "User Not Found", "OK");
                                aiLayout.IsVisible = false;
                                ai.IsRunning = false;
                            }
                        }
                        else
                        {
                            await DisplayAlert("Failed", "Login failed", "OK");
                            aiLayout.IsVisible = false;
                            ai.IsRunning = false;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Failed", "First login needs internet connection!", "ok");
                        aiLayout.IsVisible = false;
                        ai.IsRunning = false;
                    }
                }
            }
        }
        public string GenerateRequestUri(string endpoint, string username, string password)
        {
            string requestUri = endpoint;
            requestUri += $"?account={username}";
            requestUri += $"&password={password}"; // or units=metric
            return requestUri;
        }
        public async void CheckLocalData()
        {
            List<QuestionFormOnline> questionsHistory = await App.Database.GetQuestionsNotSyncAsync();
            if (questionsHistory.Count > 0)
            {
                Application.Current.MainPage = new SyncDataPage();
            }
            else
            {
                updateEstalishment();
            }

        }
        public async void CheckAppVersion()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                AppVersionOnline appversion = await _restService.GetAppItemVersion(ConstantData.HigalaApi + "getversion/" + ConstantData.AppVersion);
                if (appversion == null)
                {
                    bool answer = await DisplayAlert("Youre Higala App is outdated!", "Download new version?", "Yes", "No");
                    Debug.WriteLine("Answer: " + answer);
                    if (answer)
                    {
                        var uri = new Uri(ConstantData.ApkUrl);
                        await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
                    }
                }
                else
                {
                    Debug.WriteLine("Updated");
                }
            }
        }
        public async void updateEstalishment()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                try
                {
                    Debug.WriteLine("\tTIBONG {0}", "Start Update Local Establishents!!");
                    await _dataService.SyncQuestions();
                    await _dataService.DowloadEstablishments();
                    await _dataService.DowloadQuestionHistory();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("\t" + ex, "Error on Synching Login items!!");
                }
            }

            App.Current.MainPage = new NavigationPage(new HomePage());
            aiLayout.IsVisible = false;
            ai.IsRunning = false;
        }

    }
}