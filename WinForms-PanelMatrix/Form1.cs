namespace WinForms_PanelMatrix {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();

            panelMatrix1.PanelCreationHook = ConstructCell;
            panelMatrix1.PanelClickHook = OnCellClicked;
            panelMatrix1.GetPanelAt(0, 0).BackColor = Color.Green;
            panelMatrix1.GetLabelAt(0, 0).Text = "O";
        }

        private void ConstructCell(int column, int row, ref Panel panel, ref Label content) {
            panel.BackColor = (column % 2 == row % 2) ? Color.Gray : Color.DarkGray;
            panel.BorderStyle = BorderStyle.FixedSingle;

            content.Text = "X";
        }

        private void OnCellClicked(int column, int row) {
            panelMatrix1.GetPanelAt(column, row).BackColor = Color.Beige;
        }
    }
}