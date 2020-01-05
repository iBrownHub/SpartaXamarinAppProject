using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using ClimbingApp.Classes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClimbingApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page3 : ContentPage
    {
        public Page3()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLiteConnection conn = new SQLiteConnection(App.filePath))
            {
                conn.CreateTable<Session>();
                var sessions = conn.Table<Session>().ToList();
                SessionsListView.ItemsSource = sessions;
                conn.CreateTable<Climb>();
                var climbs = conn.Table<Climb>().ToList();
                ClimbsListView.ItemsSource = climbs;
            }
        }

        private void SessionsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var session = (Session)SessionsListView.SelectedItem;
            DeleteButton.IsEnabled = true;
            using (SQLiteConnection conn = new SQLiteConnection(App.filePath))
            {
                var climbs = conn.Query<Climb>($"Select * from Climb where SessionID = ?", session.SessionID);
                ClimbsListView.ItemsSource = climbs;
            }
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            DeleteButton.IsEnabled = false;
            var session = (Session)SessionsListView.SelectedItem;
            using (SQLiteConnection conn = new SQLiteConnection(App.filePath))
            {
                conn.Query<Climb>($"Delete from Climb where SessionID = ?", session.SessionID);
                conn.Query<Session>($"Delete from Session where SessionID = ?", session.SessionID);
                conn.CreateTable<Session>();
                var sessions = conn.Table<Session>().ToList();
                SessionsListView.ItemsSource = sessions;
                conn.CreateTable<Climb>();
                var climbs = conn.Table<Climb>().ToList();
                ClimbsListView.ItemsSource = climbs;
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.filePath))
            {
                conn.CreateTable<Session>();
                var sessions = conn.Query<Session>($"Select * from Session where CentreName like '{SessionSearchBar.Text}%'").ToList();
                SessionsListView.ItemsSource = sessions;
                conn.CreateTable<Climb>();
                var climbs = conn.Query<Climb>($"Select * from Climb where CentreName like '{SessionSearchBar.Text}%'").ToList();
                ClimbsListView.ItemsSource = climbs;
            }
        }
    }
}