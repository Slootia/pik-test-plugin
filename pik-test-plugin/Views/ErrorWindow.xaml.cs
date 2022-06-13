using PikTestPlugin.Models;
using PikTestPlugin.ViewModels;
using System.Windows;

namespace PikTestPlugin.Views
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        public ErrorWindow(ErrorModel errorModel)
        {
            DataContext = new ErrorViewModel(errorModel);
            InitializeComponent();
        }
    }
}
