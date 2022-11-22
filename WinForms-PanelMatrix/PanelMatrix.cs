using System.ComponentModel;

namespace WinForms_PanelMatrix {
    public partial class PanelMatrix : UserControl {
        // Különböző eventeket kezelő delegatek
        public delegate void PanelCreationHookType(int column, int row, ref Panel newPanel, ref Label panelContent);
        public delegate void PanelClickHookType(int column, int row);

        private PanelCreationHookType? _panelCreationHook = null;
        private PanelClickHookType? _panelClickHook = null;

        // Azért kell a sok dekoráció ide, mert nem szeretnénk ha ezek a propertyk megjelennének a 
        // designerben, úgyse tudunk ott valük mit kezdeni. A PanelCreationHook beállításakor újraépítésre
        // kerül a mátrix, mivel az megjelenítéssel kapcsolatos, a PanelClickHook esetében azonban ez nem
        // történik meg, mivel az elemeknek konstrukció után van click handlerjük, ami tud delegálni ide
        // akkor is, ha később kerül beállításra

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Bindable(false)]
        [Browsable(false)]
        public PanelCreationHookType? PanelCreationHook {
            get => _panelCreationHook;
            set {
                _panelCreationHook = value;
                PopulateMatrix();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Bindable(false)]
        [Browsable(false)]
        public PanelClickHookType? PanelClickHook {
            get => _panelClickHook;
            set {
                _panelClickHook = value;
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
        private Panel[,] _panelMatrix = { };

        public PanelMatrix() {
            InitializeComponent();
            PopulateMatrix();
        }

        // Teljsen újragenerálja a mátrixot, akkor van használva ha valamelyik megjelenítéssel kapcsolatos
        // property megváltozik (automatikusan)
        private void PopulateMatrix() {
            Controls.Clear();

            _panelMatrix = new Panel[Columns, Rows];

            int startX = ApplyHorizontalAlignment(HorizontalAlignment, Width, Columns * CellWidth);
            int startY = ApplyVerticalAlignment(VerticalAlignment, Height, Rows * CellHeight);

            for (int i = 0; i < Columns; i++) {
                for(int j = 0; j < Rows; j++) {
                    Panel toAdd = CreatePanel(i, j);

                    int xLocation = startX + i * _cellWidth;
                    int yLocation = startY + j * _cellHeight;

                    toAdd.Location = new(xLocation, yLocation);

                    _panelMatrix[i, j] = toAdd;
                    Controls.Add(toAdd);
                }
            }
        }

        // Kiszámolja a megadott Alignment alapján hol fog elhelyezkedni a tartalom bal széle
        private int ApplyHorizontalAlignment(Alignment alignment, int containerWidth, int contentWidth) {
            int startX = 0;

            switch(alignment) {
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

            switch(alignment) {
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
        private Panel CreatePanel(int column, int row) {
            Panel panel = new() {
                Size = new(CellWidth, CellHeight)
            };

            Label label = new() {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            if (PanelCreationHook != null) {
                PanelCreationHook(column, row, ref panel, ref label);
            } else {
                panel.BackColor = Color.LightGray;
                panel.BorderStyle = BorderStyle.FixedSingle;
                label.Text = $"{(char)(65 + (column % 25))}{row}";
            }

            panel.Click += (object? sender, EventArgs args) => DoClickEvent(column, row);
            label.Click += (object? sender, EventArgs args) => DoClickEvent(column, row);

            panel.Controls.Add(label);

            return panel;
        }

        // Minden cellában levő elem a click eventjét ide dobja át, ami továbbhív általunk megadott kódba,
        // ahol ténylegesen kezeljük az eventet (ha nincs beállítva PanelClickHook, akkor nem történik semmi)
        private void DoClickEvent(int column, int row) {
            if (PanelClickHook != null) {
                PanelClickHook(column, row);
            }
        }

        // Visszaadja a megadott oszlop (x) és sorban (y) levő Panelt, ha módosítani szeretnénk
        public Panel GetPanelAt(int column, int row) {
            return _panelMatrix[column, row];
        }

        // Visszaadja a megadott oszlop (x) és sorban (y) található Panelen levő Labelt, ha
        // módosítani szeretnénk
        public Label GetLabelAt(int column, int row) {
            return (Label) _panelMatrix[column, row].Controls[0];
        }
    }
}
