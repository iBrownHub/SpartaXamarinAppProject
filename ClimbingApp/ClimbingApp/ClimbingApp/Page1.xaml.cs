using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClimbingApp.Classes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;

namespace ClimbingApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        
        int i = 0;
        int iGrade = -1;
        int jGrade = 1;
        int iClimbs = 0;
        bool opened = false;
        public Page1()
        {
            InitializeComponent();
        }

        
        private void AddAttempt_Clicked(object sender, EventArgs e)
        {
            i = Convert.ToInt32(ClimbAttemptsEntry.Text);
            i++;
            ClimbAttemptsEntry.Text = i.ToString();
        }

            
        
        private void SubtractAttempt_Clicked(object sender, EventArgs e)
        {
            if (i > 0)
            {
                i = Convert.ToInt32(ClimbAttemptsEntry.Text);
                i--;
                ClimbAttemptsEntry.Text = i.ToString();
            }
        }

        private void AddClimbButton_Clicked(object sender, EventArgs e)
        {
            iClimbs = Convert.ToInt32(ClimbsSessionEntry.Text);
            iClimbs++;
            ClimbsSessionEntry.Text = iClimbs.ToString();
        }

        private void SubtractClimbButton_Clicked(object sender, EventArgs e)
        {
            if (iClimbs > 0)
            {
                iClimbs = Convert.ToInt32(ClimbsSessionEntry.Text);
                iClimbs--;
                ClimbsSessionEntry.Text = iClimbs.ToString();
            }
        }

        private void AddGradeButton_Clicked(object sender, EventArgs e)
        {
            iGrade++;
            jGrade++;
            ClimbGradeEntry.Text = $"V{iGrade.ToString()} - V{jGrade.ToString()}";
        }

        private void SubtractGradeButton_Clicked(object sender, EventArgs e)
        {
            if (iGrade == 0)
            {
                iGrade--;
                jGrade--;
                ClimbGradeEntry.Text = $"VB - V{jGrade.ToString()}";
            }
            else if (iGrade > -1)
            {
                iGrade--;
                jGrade--;
                ClimbGradeEntry.Text = $"V{iGrade.ToString()} - V{jGrade.ToString()}";
            }
        }

        private void NextClimbButton_Clicked(object sender, EventArgs e)
        {
            Climb climb = new Climb()
            {
                SessionID = DateTime.Today.Date,
                AreaInCentre = ClimbAreaEntry.Text,
                ClimbGrade = ClimbGradeEntry.Text,
                ClimbAttempts = Convert.ToInt32(ClimbAttemptsEntry.Text)
            };
            using (SQLiteConnection conn = new SQLiteConnection(App.filePath))
            {
                conn.CreateTable<Climb>();
                conn.Insert(climb);
            }
            i = 0;
            iGrade = -1;
            jGrade = 1;
            iClimbs++;
            ClimbAttemptsEntry.Text = i.ToString();
            ClimbsSessionEntry.Text = iClimbs.ToString();
            ClimbGradeEntry.Text = $"VB - V{jGrade.ToString()}";
            ClimbAreaEntry.Text = null;
        }

         private void FinishSessionButton_Clicked(object sender, EventArgs e)
        {
            Session session = new Session()
            {
                SessionID = DateTime.Today.Date,
                AmountOfClimbs = Convert.ToInt32(ClimbsSessionEntry.Text),
                ClimbTime = SessionTimer.Text
            };
            using (SQLiteConnection conn = new SQLiteConnection(App.filePath))
            {
                conn.CreateTable<Session>();
                conn.Insert(session);
            }
            i = 0;
            iGrade = -1;
            jGrade = 1;
            iClimbs = 0;
            ClimbAttemptsEntry.Text = i.ToString();
            ClimbsSessionEntry.Text = iClimbs.ToString();
            ClimbGradeEntry.Text = $"VB - V{jGrade.ToString()}";
            ClimbAreaEntry.Text = null;
        }
       protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!opened)
            {
                Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
                {
                    TimeSpan ts = Page2.s.Elapsed;
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
                    SessionTimer.Text = elapsedTime;
                    return true;
                });
            }
        }
    }
}