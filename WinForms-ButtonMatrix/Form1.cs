namespace WinForms_ButtonMatrix {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();

            buttonMatrix1.ButtonCreationHook = ConstructCell;
            buttonMatrix1.ButtonClickHook = OnCellClicked;
            buttonMatrix1.GetButtonAt(0, 0).BackColor = Color.Green;
            buttonMatrix1.GetButtonAt(0, 0).Text = "O";
        }

        private void ConstructCell(int column, int row, ref Button button) {
            button.BackColor = (column % 2 == row % 2) ? Color.Gray : Color.DarkGray;
            button.Text = "X";
        }

        private void OnCellClicked(int column, int row) {
            buttonMatrix1.GetButtonAt(column, row).BackColor = Color.Beige;
        }
    }
}