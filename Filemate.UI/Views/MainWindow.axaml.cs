using Avalonia.Controls;
using Filemate.UI.ViewModels;

namespace Filemate.UI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.DataContext = new MainWindowViewModel();

    }
}