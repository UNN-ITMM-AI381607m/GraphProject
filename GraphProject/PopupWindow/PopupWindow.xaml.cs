using System.Windows;

namespace GraphProject.SettingWindow
{
    /// <summary>
    /// Interaction logic for PopupWindow.xaml
    /// </summary>
    public partial class PopupWindow : Window
    {
        public int NewID
        {
            get
            {
                if (NameTextBox == null)
                    return -1;
                try
                {
                    return int.Parse(NameTextBox.Text);
                }
                catch
                {
                    return -1;
                }
            }
        }

        public PopupWindow(string title, string message, string button)
        {
            InitializeComponent();
            Title = title;
            Message.Text = message;
            ButtonField.Content = button;
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            try
            {
                int.Parse(NameTextBox.Text);
            }
            catch
            {
                MessageBox.Show("Please enter positive integer", "Wrong Format");
                return;
            }
            Close();
        }
    }
}
