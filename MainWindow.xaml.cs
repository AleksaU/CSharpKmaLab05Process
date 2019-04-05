using System.Windows;
using CSharpLab5.ViewModels;

namespace CSharpLab5
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ProcessesGrid.DataContext = new MainWindowViewModel();
            ProcessesGrid.EnableRowVirtualization = true;
            ProcessesGrid.EnableColumnVirtualization = true;
        }
    }
}
