using System.Windows;

namespace Coding
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var convolutionViewMode = new ConvolutionViewModel();

            this.DataContext = convolutionViewMode;
            
        }

        private void ConvolutionButton_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as ConvolutionViewModel).ConvertGrammaticalText();
        }
    }
}
