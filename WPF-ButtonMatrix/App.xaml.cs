using System.Windows;

using WPF_ButtonMatrix.ViewModel;
using WPF_ButtonMatrix.View;

namespace WPF_ButtonMatrix {
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
