using System.ComponentModel;

namespace WinForms_ButtonMatrix {
    public partial class ButtonMatrix : UserControl {
        // Különböző eventeket kezelő delegatek
        public delegate void ButtonCreationHookType(int column, int row, ref Button newButton);
        public delegate void ButtonClickHookType(int column, int row);

        private ButtonCreationHookType? _buttonCreationHook = null;
        private ButtonClickHookType? _buttonClickHook = null;

        // Azért kell a sok dekoráció ide, mert nem szeretnénk ha ezek a propertyk megjelennének a 
        // designerben, úgyse tudunk ott valük mit kezdeni. A PanelCreationHook beállításakor újraépítésre
        // kerül a mátrix, mivel az megjelenítéssel kapcsolatos, a PanelClickHook esetében azonban ez nem
        // történik meg, mivel az elemeknek konstrukció után van click handlerjük, ami tud delegálni ide
        // akkor is, ha később kerül beállításra

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Bindable(false)]
        [Browsable(false)]
        public ButtonCreationHookType? ButtonCreationHook {
            get => _buttonCreationHook;
            set {
                _buttonCreationHook = value;
                PopulateMatrix();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Bindable(false)]
        [Browsable(false)]
        public ButtonClickHookType? ButtonClickHook {
            get => _buttonClickHook;
            set {
                _buttonClickHook = value;
            }
        }

        // Ezek a propertyk szerintem maguktól éretetődőek

        public enum Alignment {
            LEFT, CENTER, TOP, RIGHT, BOTTOM
        }

        private int _cellWidth = 24;
        private int _cellHeight = 24;

        private int _columns = 4;
        private int _rows = 4;

        private Alignment _horizontalAlignment = Alignment.LEFT;
        private Alignment _verticalAlignment = Alignment.TOP;

        [Category("_Mátrix"),
         Description("Egy cella szélessége")]
        public int CellWidth {
            get => _cellWidth;
            set {
                _cellWidth = value;
                PopulateMatrix();
            }
        }

        [Category("_Mátrix"),
         Description("Egy cella magassága")]
        public int CellHeight {
            get => _cellHeight;
            set {
                _cellHeight = value;
                PopulateMatrix();
            }
        }

        [Category("_Mátrix"),
         Description("Mátrix oszlopszáma")]
        public int Columns {
            get => _columns;
            set {
                _columns = value;
                PopulateMatrix();
            }
        }

        [Category("_Mátrix"),
         Description("Mátrix sorszáma")]
        public int Rows {
            get => _rows;
            set {
                _rows = value;
                PopulateMatrix();
            }
        }

        [Category("_Mátrix"),
         Description("Tartalom vízszintes igazítása")]
        public Alignment HorizontalAlignment {
            get => _horizontalAlignment;
            set {
                _horizontalAlignment = value;
                PopulateMatrix();
            }
        }

        [Category("_Mátrix"),
         Description("Tartalom függőleges igazítása")]
        public Alignment VerticalAlignment {
            get => _verticalAlignment;
            set {
                _verticalAlignment = value;
                PopulateMatrix();
            }
        }

        // Maguk a mátrixot megjelenítő panelek, az első index az oszlop (x), második a sor (y)
        private Button[,] _buttonMatrix = { };

        public ButtonMatrix() {
            InitializeComponent();
            PopulateMatrix();
        }

        // Teljsen újragenerálja a mátrixot, akkor van használva ha valamelyik megjelenítéssel kapcsolatos
        // property megváltozik (automatikusan)
        private void PopulateMatrix() {
            Controls.Clear();

            _buttonMatrix = new Button[Columns, Rows];

            int startX = ApplyHorizontalAlignment(HorizontalAlignment, Width, Columns * CellWidth);
            int startY = ApplyVerticalAlignment(VerticalAlignment, Height, Rows * CellHeight);

            for (int i = 0; i < Columns; i++) {
                for (int j = 0; j < Rows; j++) {
                    Button toAdd = CreateButton(i, j);

                    int xLocation = startX + i * _cellWidth;
                    int yLocation = startY + j * _cellHeight;

                    toAdd.Location = new(xLocation, yLocation);

                    _buttonMatrix[i, j] = toAdd;
                    Controls.Add(toAdd);
                }
            }
        }

        // Kiszámolja a megadott Alignment alapján hol fog elhelyezkedni a tartalom bal széle
        private int ApplyHorizontalAlignment(Alignment alignment, int containerWidth, int contentWidth) {
            int startX = 0;

            switch (alignment) {
                case Alignment.LEFT:
                    break;
                case Alignment.CENTER:
                    startX = containerWidth / 2 - contentWidth / 2;
                    break;
                case Alignment.RIGHT:
                    startX = containerWidth - contentWidth;
                    break;
            }

            return startX;
        }

        // Kiszámolja a megadott Alignment alapján hol fog elhelyezkedni a tartalom teteje
        private int ApplyVerticalAlignment(Alignment alignment, int containerHeight, int contentHeight) {
            int startY = 0;

            switch (alignment) {
                case Alignment.TOP:
                    break;
                case Alignment.CENTER:
                    startY = containerHeight / 2 - contentHeight / 2;
                    break;
                case Alignment.BOTTOM:
                    startY = containerHeight - contentHeight;
                    break;
            }

            return startY;
        }

        // Ez a metódus hozza létre a Panelokat minden cellához, valamint mindegyikbe helyez egy Labelt,
        // ha esetleg szöveget is meg szeretnénk rajtuk jeleníteni. Ha be van állítva a PanelCreationHook
        // property, tehát szeretnénk egy alapértelmezett stílust adni a celláknak készítés során, az is
        // itt kerül meghívásra.
        private Button CreateButton(int column, int row) {
            Button button = new() {
                Size = new(CellWidth, CellHeight)
            };

            if (ButtonCreationHook != null) {
                ButtonCreationHook(column, row, ref button);
            } else {
                button.BackColor = Color.LightGray;
                button.Text = $"{(char)(65 + (column % 25))}{row}";
            }

            button.Click += (object? sender, EventArgs args) => DoClickEvent(column, row);

            return button;
        }

        // Minden cellában levő elem a click eventjét ide dobja át, ami továbbhív általunk megadott kódba,
        // ahol ténylegesen kezeljük az eventet (ha nincs beállítva PanelClickHook, akkor nem történik semmi)
        private void DoClickEvent(int column, int row) {
            if (ButtonClickHook != null) {
                ButtonClickHook(column, row);
            }
        }

        // Visszaadja a megadott oszlop (x) és sorban (y) levő Panelt, ha módosítani szeretnénk
        public Button GetButtonAt(int column, int row) {
            return _buttonMatrix[column, row];
        }
    }
}
