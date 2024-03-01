using Avalonia.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using GUI.Services;

namespace GUI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var service = (FilesService)Ioc.Default.GetRequiredService<IFilesService>();
            service.setTargetWindow(this);
        }
    }
}