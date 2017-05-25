using System.Windows;

namespace GUI
{
    /// <summary>
    /// Interaction logic for TheoryWindow.xaml
    /// </summary>
    public partial class TheoryWindow : Window
    {
        public TheoryWindow()
        {
            InitializeComponent();
            HelpWebBrowser.NavigateToString( Properties.Resources.index );
        }
    }
}
