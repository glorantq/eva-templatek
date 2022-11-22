using System;
using System.Windows.Media;

namespace WPF_PanelMatrix.ViewModel {
    internal class CellViewModel : ViewModelBase {
        private int _column = 0;
        private int _row = 0;

        private string _content = "";

        private Brush _backgroundBrush = new SolidColorBrush(Colors.LightGray);

        public int Column {
            get => _column;
            set {
                _column = value;
                OnPropertyChanged();
            }
        }

        public int Row {
            get => _row;
            set {
                _row = value;
                OnPropertyChanged();
            }
        }

        public string Content {
            get => _content;
            set {
                _content = value;
                OnPropertyChanged();
            }
        }

        public Brush BackgroundBrush {
            get => _backgroundBrush;
            set {
                _backgroundBrush = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand ClickCommand { get; set; }

        public CellViewModel() {
            ClickCommand = new((object? parameter) => { });
        }
    }
}
