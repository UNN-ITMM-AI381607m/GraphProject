using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GraphComponent.SettingWindow
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
        public PopupWindow()
        {
            InitializeComponent();
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
