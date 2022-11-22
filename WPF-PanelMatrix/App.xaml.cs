using System.Windows;

using WPF_PanelMatrix.ViewModel;
using WPF_PanelMatrix.View;

namespace WPF_PanelMatrix {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public App() {
            Startup += OnAppStartup;
        }

        private void OnAppStartup(object? sender, StartupEventArgs args) {
            MainWindow window = new() {
                DataContext = new MainViewModel()
            };

            window.Show();
        }
    }
}
