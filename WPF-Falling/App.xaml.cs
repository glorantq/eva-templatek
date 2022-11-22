using System.Windows;

using WPF_Falling.ViewModel;
using WPF_Falling.View;

namespace WPF_Falling {
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
