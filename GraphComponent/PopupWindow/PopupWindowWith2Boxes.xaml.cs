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

namespace GraphComponent.PopupWindow
{
    /// <summary>
    /// Interaction logic for PopupWindowWith2Boxes.xaml
    /// </summary>
    public partial class PopupWindowWith2Boxes : Window
    {
        public int ID1
        {
            get
            {
                if (FirstTextBox == null)
                    return -1;
                try
                {
                    return int.Parse(FirstTextBox.Text);
                }
                catch
                {
                    return -1;
                }
            }
        }

        public int ID2
        {
            get
            {
                if (SecondTextBox == null)
                    return -1;
                try
                {
                    return int.Parse(SecondTextBox.Text);
                }
                catch
                {
                    return -1;
                }
            }
        }

        public PopupWindowWith2Boxes(string title, string message, string button)
        {
            InitializeComponent();
            Title = title;
            Message.Text = message;
            Button.Content = button;
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            try
            {
                int.Parse(FirstTextBox.Text);
                int.Parse(SecondTextBox.Text);
            }
            catch
            {
                MessageBox.Show("Пожалуйста, введите положительное число.", "Неверный формат");
                return;
            }
            Close();
        }
    }
}
