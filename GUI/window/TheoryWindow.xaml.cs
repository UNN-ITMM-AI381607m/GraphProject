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
        }

        private void PropertiesButton_Click(object sender, RoutedEventArgs e)
        {
            //TheoryRichTextBox.Document.Blocks.Clear();
            TheoryRichTextBox.Text = Properties.Resources._1;
        }

        private void FlatButton_Click(object sender, RoutedEventArgs e)
        {
            //TheoryRichTextBox.Document.Blocks.Clear();
            TheoryRichTextBox.Text = Properties.Resources._2;
        }

        private void EstimatesButton_Click(object sender, RoutedEventArgs e)
        {
            //TheoryRichTextBox.Document.Blocks.Clear();
            TheoryRichTextBox.Text = Properties.Resources._3;
            //TheoryRichTextBox.AppendText("EstimatesButton_Click");
        }

    }
}
