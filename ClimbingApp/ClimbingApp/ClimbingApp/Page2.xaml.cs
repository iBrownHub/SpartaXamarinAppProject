using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClimbingApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page2 : ContentPage
    {
        public static bool started = false;
        public static Stopwatch s = new Stopwatch();
        public Page2()
        {
            InitializeComponent();
            TimerLabel.FontSize = 48;
        }
        private void StartButton_Clicked(object sender, EventArgs e)
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
            {
                TimeSpan ts = s.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                TimerLabel.Text = elapsedTime;
                progressBar.Progress = (double)ts.Seconds / 60.0;
                return true;
            });
            if (StartButton.Text == "Start")
            {
                Page2.started = true;
                s.Start();
                StartButton.Text = "Stop";
                ResetButton.IsEnabled = false;
            }
            else
            {
                Page2.started = false;
                s.Stop();
                StartButton.Text = "Start";
                ResetButton.IsEnabled = true;
            }
        }
        private void ResetButton_Clicked(Object sender, EventArgs e)
        {
            s.Reset();
            ResetButton.IsEnabled = false;
        }
    }
}