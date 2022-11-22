# Windows Forms mátrix gombokkal

Template olyan játékokhoz, ahol Windows Formsot kell használni, és valamilyen oknál fogva gombokkal szeretnénk megoldani 
a megjelnítést.

Maga az egész egy `ButtonMatrix` nevű saját controllal van megvalósítva, innen lényegében az az egyetlen szükséges.

Designerben beállítható tulajdonságok:
- CellWidth (cellák szélessége)
- CellHeight (cellák magassága)
- Columns (oszlopok száma)
- Rows (sorok száma)
- HorizontalAlignment (tartalom vízszintes igazítása)
- VerticalAlignment (tartalom függőleges igazítása)

Ezek beállíthatók kódból is, valamint így lehet vele ügyeskedni C#-ban (Form1.cs tartalma):
```c#
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
```

![HdzL1G2.png](https://iili.io/HdzL1G2.png)
