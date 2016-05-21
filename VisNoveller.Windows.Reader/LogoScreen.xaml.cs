using System;
using System.Reflection;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace VisNoveller.Windows.Reader
{
    public partial class LogoScreen : Grid
    {
        Random Magic { get; set; }
        int SlideNumber = 0;
        public LogoScreen()
        {
            InitializeComponent();
            Magic = new Random();
            Loaded += (s, EA) => CoolTextEffect("KAD_TECHi");

        }
        void CoolTextEffect(string baseString)
        {
            Opacity = 1;
            LogoTB.Foreground = Brushes.Lime;
            Background = Brushes.Black;
            var displayString = "";
            new Thread(() =>
            {
                displayString = Extensions.GetRandomString(baseString.Length);
                while (OffsetString(baseString, ref displayString))
                    Dispatcher.Invoke((Action)(() => LogoTB.Text = displayString), DispatcherPriority.Background);
                Dispatcher.Invoke((Action)(() => LogoTB.Text = displayString), DispatcherPriority.Background);
                Thread.Sleep(2000);
                Dispatcher.Invoke((Action)(() => ((Storyboard)Resources["FlashStoryboard"]).Begin()), DispatcherPriority.Background);
            }).Start();
            SlideNumber++;
        }
        bool OffsetString(string baseString, ref string displayString)
        {
            for (int i = 0; i < displayString.Length; i++)
            {
                var arr = displayString.ToCharArray();
                if (displayString[i] < baseString[i])
                    arr[i]++;
                else if (displayString[i] > baseString[i])
                    arr[i]--;
                displayString = new string(arr);
                Thread.Sleep(5);
            }
            return displayString != baseString;
        }

        private void DisappearAnimation_Completed(object sender, EventArgs e)
        {
            if (SlideNumber == 1)
            {
                LogoTB.FontFamily = new FontFamily("Arial");
                LogoTB.FontSize *= 2;
                ((Storyboard)Resources["DisappearStoryboard"]).AutoReverse = false;
                CoolTextEffect("VisNoveller");
            }
        }
    }
}
