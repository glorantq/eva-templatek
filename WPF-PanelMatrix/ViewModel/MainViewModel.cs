using System.Collections.ObjectModel;
using System.Windows.Media;

namespace WPF_PanelMatrix.ViewModel {
    internal class MainViewModel : ViewModelBase {
        // Ez a lista tárolja magukat a cellákat
        public ObservableCollection<CellViewModel> Cells { get; set; }

        // Minden property ami itt van ugyan azt csinálja mint Windows Formsban, de egyébként magától
        // értetődő mi mit csinál

        private int _cellWidth = 32;
        private int _cellHeight = 32;
        private int _columns = 10;
        private int _rows = 10;

        public int CellWidth {
            get => _cellWidth;
            set {
                _cellWidth = value;
                OnPropertyChanged();
                ConstructMatrix();
            }
        }

        public int CellHeight {
            get => _cellHeight;
            set {
                _cellHeight = value;
                OnPropertyChanged();
                ConstructMatrix();
            }
        }

        public int Columns {
            get => _columns;
            set {
                _columns = value;
                OnPropertyChanged();
                ConstructMatrix();
            }
        }

        public int Rows {
            get => _rows;
            set {
                _rows = value;
                OnPropertyChanged();
                ConstructMatrix();
            }
        }

        public int MatrixWidth {
            get => CellWidth * Columns;
            set {
                OnPropertyChanged();
            }
        }

        public int MatrixHeight {
            get => CellHeight * Rows;
            set {
                OnPropertyChanged();
            }
        }

        public MainViewModel() {
            Cells = new ObservableCollection<CellViewModel>();
            ConstructMatrix();

            // Ez csak portja a Windows Formsos demonak, így lehet módosítani a cellák kinézetét kódban
            GetCellAt(0, 0).BackgroundBrush = new SolidColorBrush(Colors.Green);
            GetCellAt(0, 0).Content = "O";
        }

        // Létrehozza újra a mátrixot, amikor olyan property változik, ami a teljes layoutot módosítja
        private void ConstructMatrix() {
            Cells.Clear();

            for(int i = 0; i < Rows; i++) {
                for(int j = 0; j < Columns; j++) {
                    CellViewModel cell = new() {
                        Content = $"{(char)(65 + (j % 25))}{i}"
                    };

                    OnCellBeingCreated(j, i, ref cell);

                    cell.Column = j;
                    cell.Row = i;
                    cell.ClickCommand = new((object? parameter) => OnCellClicked(cell.Column, cell.Row));

                    Cells.Add(cell);
                }
            }
        }

        // Visszaadja a megadott oszlopban és sorban levő cellát, hogy lehessen módosítani
        private CellViewModel? GetCellAt(int column, int row) {
            foreach(var cell in Cells) {
                if(cell.Column == column && cell.Row == row) {
                    return cell;
                }
            }

            return null;
        }

        // Akkor lesz meghívva, amikor létrejön egy cella, itt állíts be mindenféle alapértelmezett értéket
        private void OnCellBeingCreated(int column, int row, ref CellViewModel cell) {
            cell.BackgroundBrush = new SolidColorBrush(column % 2 == row % 2 ? Colors.LightGray : Colors.DarkGray);
            cell.Content = "X";
        }

        // Akkor lesz meghívva ha a felhasználó rákattint egy cellára, itt tudod kezelni ezt
        private void OnCellClicked(int column, int row) {
            GetCellAt(column, row).BackgroundBrush = new SolidColorBrush(Colors.Beige);
        }
    }
}
