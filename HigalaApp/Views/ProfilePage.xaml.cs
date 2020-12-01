using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HigalaApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HigalaApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
           
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var user = await App.Database.GetCustomerByIDAsync(App.UserID);
            txtFullName.Text = user.customer_firstname + " " + user.customer_middlename + " " + user.customer_lastname;
            txtContactNo.Text = user.customer_contact;
            txtEmail.Text = user.customer_email;
            
        }
        async void OnUpdateClick(object sender, EventArgs e)
        {
            //var user = new Users();
            //user.ID = App.UserID;
            //user.Name = txtName.Text;
            //user.Lastname = txtLastName.Text;
            //user.Username = txtUserName.Text;
            //user.Password = txtPassword.Text;
            //await App.Database.SaveUserAsync(user);
            await DisplayAlert("Save Result", "Successfully Updated", "OK");
        }
            
    }
}