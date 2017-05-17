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
using System.IO;

namespace GraphComponent.PopupWindow
{
    /// <summary>
    /// Interaction logic for TheoryWindow.xaml
    /// </summary>
    public partial class TheoryWindow : Window
    {
        public TheoryWindow()
        {
            InitializeComponent();
        }

        private void PropertiesButton_Click(object sender, RoutedEventArgs e)
        {
            //TheoryRichTextBox.Document.Blocks.Clear();
            TheoryRichTextBox.Text = (File.ReadAllText("C:\\Users\\Александра\\Documents\\git\\GraphProject\\Theory\\1.txt"));
        }

        private void FlatButton_Click(object sender, RoutedEventArgs e)
        {
            //TheoryRichTextBox.Document.Blocks.Clear();
            TheoryRichTextBox.Text = (File.ReadAllText("C:\\Users\\Александра\\Documents\\git\\GraphProject\\Theory\\2.txt"));
        }

        private void EstimatesButton_Click(object sender, RoutedEventArgs e)
        {
            //TheoryRichTextBox.Document.Blocks.Clear();
            TheoryRichTextBox.Text = (File.ReadAllText("C:\\Users\\Александра\\Documents\\git\\GraphProject\\Theory\\3.txt"));
            //TheoryRichTextBox.AppendText("EstimatesButton_Click");
        }

    }
}
