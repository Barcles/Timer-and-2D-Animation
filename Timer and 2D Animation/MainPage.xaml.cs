using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Timer_and_2D_Animation
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        DispatcherTimer dispatcherTimer;
        DateTimeOffset startTime;
        DateTimeOffset lastTime;
        DateTimeOffset stopTime;
        int timesTicked = 1;
        int timesToTick = 10;

        int posX = 100;
        int posY = 100;
        int speedX = 10;
        int speedY = 10;
        int radius = 60;

        public void DispatcherTimerSetup()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1/5);
            //IsEnabled defaults to false
            //TimerLog.Text += "dispatcherTimer.IsEnabled = " + dispatcherTimer.IsEnabled + "\n";
            startTime = DateTimeOffset.Now;
            lastTime = startTime;
            //TimerLog.Text += "Calling dispatcherTimer.Start()\n";
            dispatcherTimer.Start();
            //IsEnabled should now be true after calling start
            //TimerLog.Text += "dispatcherTimer.IsEnabled = " + dispatcherTimer.IsEnabled + "\n";
        }

        void dispatcherTimer_Tick(object sender, object e)
        {
            DateTimeOffset time = DateTimeOffset.Now;
            TimeSpan span = time - lastTime;
            lastTime = time;
            //Time since last tick should be very very close to Interval
            //TimerLog.Text += timesTicked + "\t time since last tick: " + span.ToString() + "\n";
            timesTicked++;
            //if (timesTicked > timesToTick)
            //{
            //    stopTime = time;
            //    TimerLog.Text += "Calling dispatcherTimer.Stop()\n";
            //    dispatcherTimer.Stop();
            //    //IsEnabled should now be false after calling stop
            //    TimerLog.Text += "dispatcherTimer.IsEnabled = " + dispatcherTimer.IsEnabled + "\n";
            //    span = stopTime - startTime;
            //    TimerLog.Text += "Total Time Start-Stop: " + span.ToString() + "\n";
            //}
            Relative_Panel1.Children.Clear();   // Clears Relative panel to prevent previous image from showing
            ellipse();  // Calls ellipse drawing and changes position
        }
        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            DispatcherTimerSetup();
        }

        void ellipse()
        {
            var geometryGroup1 = new GeometryGroup();
            var pathGeometry1 = new PathGeometry();
            //int ColorChange = 0;

            //switch(ColorChange)
            //{
            //    case 0:
            //        Windows.UI.Colors.Yellow;
            //        ColorChange += 1;
            //        break;

            //    case 1:
            //        Windows.UI.Colors.Green;
            //        ColorChange += 1;
            //        break;

            //    case 2:
            //        Windows.UI.Colors.Purple;
            //        ColorChange += 1;
            //        break;

            //    case 3:
            //        Windows.UI.Colors.Blue;
            //        ColorChange += 1;
            //        break;

            //    default:
            //        Windows.UI.Colors.Red;
            //        ColorChange = 0;
            //        break;
            //}

            var path1 = new Windows.UI.Xaml.Shapes.Path();
            path1.Fill = new SolidColorBrush(Windows.UI.Colors.Yellow);    // Fill color of Ellipse
            path1.Stroke = new SolidColorBrush(Windows.UI.Colors.Yellow);   // Outline color of Ellipse
            path1.StrokeThickness = 1;

            var ellipseGeometry1 = new EllipseGeometry();
            ellipseGeometry1.Center = new Point(posX, posY);  // Position of center of ellipse on relative panel
            ellipseGeometry1.RadiusX = radius;  // X component of Ellipse size
            ellipseGeometry1.RadiusY = radius;  // Y component of Ellipse size
            geometryGroup1.Children.Add(ellipseGeometry1);

            geometryGroup1.Children.Add(pathGeometry1);
            path1.Data = geometryGroup1;

            // When you create a XAML element in code, you have to add
            // it to the XAML visual tree. This example assumes you have
            // a panel named 'layoutRoot' in your XAML file, like this:
            // <Grid x:Name="layoutRoot>
            Relative_Panel1.Children.Add(path1);

            posX += speedX; // Allows x-axis position to change based on speed component
            posY += speedY; // Allows y-axis position to change based on speed component

            if (posX + radius > Relative_Panel1.ActualWidth) // Change direction on x-axis if ellipse radius + position hits edge of relative panel
            {
                speedX *= -1;
            }
            if (posY + radius > Relative_Panel1.ActualHeight)    // Change direction on y-axis if ellipse radius + position hits edge of relative panel
            {
                speedY *= -1;
            }
            if (posX - radius < 0)  // Change direction on x-axis if relative panel boundary is reached
            {
                speedX *= -1;
            }
            if (posY - radius < 0)  // Change direction on y-axis if relative panel boundary is reached
            {
                speedY *= -1;
            }

        }

        public void Increase_Speed_Click(object sender, RoutedEventArgs e)  // Button to increase speed on ellipse
        {
            if(speedX > 0)  // If speedX variable is positive, only increments speed with positive number
            {
                speedX += 2;
            }
            if(speedX < 0)  // If speedX variable is negative, only decrements speed with negative number
            {
                speedX -= 2;
            }
            if(speedY > 0)  // If speedY variable is positive, only increments speed with positive number
            {
                speedY += 2;
            }
            if(speedY < 0)  // If speedY variable is positive, only decrements speed with negative number
            {
                speedY -= 2;
            }
        }

        private void Decrease_Speed_Click(object sender, RoutedEventArgs e)
        {
            if (speedX > 0)  // If speedX variable is positive, only increments speed with positive number
            {
                speedX -= 2;
                if(speedX <= 0) // Prevents ellipse from changing direction
                {
                    speedX = 1;
                }
            }
            if (speedX < 0)  // If speedX variable is negative, only decrements speed with negative number
            {
                speedX += 2;
                if(speedX >= 0) // Prevents ellipse from changing direction
                {
                    speedX = -1;
                }
            }
            if (speedY > 0)  // If speedY variable is positive, only increments speed with positive number
            {
                speedY -= 2;
                if(speedY <= 0) // Prevents ellipse from changing direction
                {
                    speedY = 1;
                }
            }
            if (speedY < 0)  // If speedY variable is positive, only decrements speed with negative number
            {
                speedY += 2;
                if(speedY >= 0) // Prevents ellipse from changing direction
                {
                    speedY = -1;
                }
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)  // Reset button for radius and speed, changes X & Y values back to 10 without changing direction
        {
            if(speedX > 0)
            {
                speedX = 10;
            }
            if(speedY > 0)
            {
                speedY = 10;
            }
            if (speedX < 0)
            {
                speedX = -10;
            }
            if(speedY < 0)
            {
                speedY = -10;
            }
            radius = 60;
        }

        private void SizePlus_Click(object sender, RoutedEventArgs e)   // Increase size of ellipse
        {
            radius+= 5;
            if(radius >= 400)   // Prevents ellipse size from increasing past 400
            {
                radius = 395;
            }
        }

        private void SizeMinus_Click(object sender, RoutedEventArgs e)  // Decreases size of ellipse
        {
            radius -= 5;
            if(radius <= 0) // Prevents ellipse size from decreasing past 5
            {
                radius = 5;
            }
        }
    }
}
