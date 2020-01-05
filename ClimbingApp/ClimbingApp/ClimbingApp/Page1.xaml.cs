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
        public static bool stopped = false;
        int i = 0;
        int iGrade = -1;
        int jGrade = 1;
        int iClimbs = 0;
        public Page1()
        {
            InitializeComponent();
        }


        private void AddAttempt_Clicked(object sender, EventArgs e)
        {
            if (i == 0)
            {
                SubtractAttemptButton.IsEnabled = true;
            }
            i = Convert.ToInt32(ClimbAttemptsEntry.Text);
            i++;
            ClimbAttemptsEntry.Text = i.ToString();
        }



        private void SubtractAttempt_Clicked(object sender, EventArgs e)
        {
            if (i > 0)
            {
                if (i == 1)
                {
                    SubtractAttemptButton.IsEnabled = false;
                }
                i = Convert.ToInt32(ClimbAttemptsEntry.Text);
                i--;
                ClimbAttemptsEntry.Text = i.ToString();
            }
        }

        private void AddGradeButton_Clicked(object sender, EventArgs e)
        {
            if (iGrade < 14)
            {
                if (iGrade < 0)
                {
                    SubtractGradeButton.IsEnabled = true;
                }
                if (iGrade == 13)
                {
                    AddGradeButton.IsEnabled = false;
                }
                iGrade++;
                jGrade++;
                ClimbGradeEntry.Text = $"V{iGrade.ToString()} - V{jGrade.ToString()}";
            }
        }

        private void SubtractGradeButton_Clicked(object sender, EventArgs e)
        {
            if (iGrade > 13)
            {
                AddGradeButton.IsEnabled = true;
            }
            if (iGrade == 0)
            {
                SubtractGradeButton.IsEnabled = false;
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
                CentreName = CentreEntry.Text,
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
            ClimbsSessionEntry.Text = iClimbs.ToString();
            ClimbAttemptsEntry.Text = i.ToString();
            SubtractAttemptButton.IsEnabled = false;
            ClimbGradeEntry.Text = $"VB - V{jGrade.ToString()}";
            SubtractGradeButton.IsEnabled = false;
            AddGradeButton.IsEnabled = true;
            ClimbAreaEntry.Text = null;
        }

        private void FinishSessionButton_Clicked(object sender, EventArgs e)
        {
            Session session = new Session()
            {
                SessionID = DateTime.Today.Date,
                CentreName = CentreEntry.Text,
                AmountOfClimbs = Convert.ToInt32(ClimbsSessionEntry.Text),
                ClimbTime = (EndSessionTime.Time - StartSessionTime.Time).ToString()
            };
            using (SQLiteConnection conn = new SQLiteConnection(App.filePath))
            {
                conn.CreateTable<Session>();
                var sessionToUpdate = conn.Find<Session>(DateTime.Today.Date);
                if (sessionToUpdate == null)
                {
                    conn.Insert(session);
                }
                else
                {
                    sessionToUpdate.AmountOfClimbs = sessionToUpdate.AmountOfClimbs + session.AmountOfClimbs;
                    var oldArray = sessionToUpdate.ClimbTime.Split(':');
                    var newArray = session.ClimbTime.Split(':');
                    int[] finalArray = new int[oldArray.Length];
                    for (int i = 0; i < oldArray.Length; i++)
                    {
                        finalArray[i] = Convert.ToInt32(oldArray[i]) + Convert.ToInt32(newArray[i]);
                        if (finalArray[i] > 59)
                        {
                            finalArray[i] -= 60;
                            finalArray[i - 1]++;
                            if (finalArray[i - 1] > 59)
                            {
                                finalArray[i - 1] -= 60;
                                finalArray[i - 2]++;
                            }
                        }
                    }
                    sessionToUpdate.ClimbTime = String.Join(":", finalArray.Select(i => string.Format("{0:00}", i)));
                    conn.Update(sessionToUpdate);
                }
            }
            i = 0;
            iGrade = -1;
            jGrade = 1;
            iClimbs = 0;
            ClimbsSessionEntry.Text = iClimbs.ToString();
            ClimbAttemptsEntry.Text = i.ToString();
            ClimbGradeEntry.Text = $"VB - V{jGrade.ToString()}";
            CentreEntry.Text = null;
            ClimbAreaEntry.Text = null;
            StartSessionTime.Time = new TimeSpan(0);
            EndSessionTime.Time = new TimeSpan(0);
            Page2.s.Stop();
            stopped = true;
        }
    }
}