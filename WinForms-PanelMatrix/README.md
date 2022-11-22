# Windows Forms mátrix panelekkel

Template olyan játékokhoz, ahol Windows Formsot kell használni, és panelekkel szeretnénk megoldani 
a megjelnítést (háttérkép vagy bármi egyéb miatt). Kattintásra ugyan úgy van lehetőseg, mint gombokkal.

Maga az egész egy `PanelMatrix` nevű saját controllal van megvalósítva, innen lényegében az az egyetlen szükséges.

Designerben beállítható tulajdonságok:
- CellWidth (cellák szélessége)
- CellHeight (cellák magassága)
- Columns (oszlopok száma)
- Rows (sorok száma)
- HorizontalAlignment (tartalom vízszintes igazítása)
- VerticalAlignment (tartalom függőleges igazítása)

Ezek beállíthatók kódból is, valamint így lehet vele ügyeskedni C#-ban (Form1.cs tartalma):
```c#
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
```

![HJQz90x.png](https://iili.io/HJQz90x.png)
