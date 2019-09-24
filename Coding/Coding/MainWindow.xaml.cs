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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
