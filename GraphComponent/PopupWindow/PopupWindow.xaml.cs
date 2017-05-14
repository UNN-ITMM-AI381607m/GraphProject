using System.Windows;

namespace GraphComponent.SettingWindow
{
    /// <summary>
    /// Interaction logic for PopupWindow.xaml
    /// </summary>
    public partial class PopupWindow : Window
    {
        public int Result
        {
            get
            {
                return int.Parse(NameTextBox.Text);
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
                MessageBox.Show("Пожалуйста, введите положительное число.", "Неверный формат");
                return;
            }
            DialogResult = true;
            Close();
        }
    }
}
