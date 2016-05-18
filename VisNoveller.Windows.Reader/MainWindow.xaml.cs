using System.Windows;

namespace VisNoveller.Windows.Reader
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Top = Left = 0;
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
        }
    }
}
