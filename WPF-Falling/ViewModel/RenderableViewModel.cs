using System.Windows.Media;

namespace WPF_Falling.ViewModel {
    internal class RenderableViewModel : ViewModelBase {
        private int _x = 0;
        private int _y = 0;
        private int _width = 16;
        private int _height = 16;

        private Brush _brush = new SolidColorBrush(Colors.Blue);

        public int X {
            get => _x;
            set {
                _x = value;
                OnPropertyChanged();
            }
        }

        public int Y {
            get => _y;
            set {
                _y = value;
                OnPropertyChanged();
            }
        }

        public int Width {
            get => _width;
            set {
                _width = value;
                OnPropertyChanged();
            }
        }

        public int Height {
            get => _height;
            set {
                _height = value;
                OnPropertyChanged();
            }
        }

        public Brush Brush {
            get => _brush;
            set {
                _brush = value;
            }
        }
    }
}
