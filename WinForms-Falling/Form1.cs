using WinForms_Falling.Properties;
using Timer = System.Windows.Forms.Timer;

namespace WinForms_Falling {
    public partial class Form1 : Form {
        // Ehhez sajnos nem lehet olyan szép UserControl-t gyártani mint a mátrixokhoz, ehhez innen kell
        // majd összekukázni a kódrészleteket, de igyekeztem minimálisra venni hogy átlátható legyen.

        // Ez a lista tárol minden objektumot amit rajzolni kell, közvetlen ezt ne módosítsd
        private readonly List<Renderable> Renderables = new();

        // Ez a lista tárolja azokat az objektumokat amiket hozzá szeretnénk adni a fentebbi listához, ezt se
        // módosítsd közvetlen
        private readonly List<Renderable> AdditionQueue = new();

        #region Demo-kód
        private Timer _timer = new();
        private Random _random = new();
        #endregion

        public Form1() {
            InitializeComponent();

            _timer.Interval = 10;
            _timer.Tick += (sender, args) => { PerformRendering(); };
            _timer.Start();
        }

        // Ezt a metódust kell meghívni mindig amikor frissíteni szeretnénk a rajzolt objektumokat; amelyikeket tötölni
        // szeretnénk ténylegesen kitörli, amelyeket hozzá szeretnénk adni a listához azokat pedig beállítja rendesen
        private void PerformRendering() {
            List<Renderable> toBeRemoved = new();

            foreach(var renderable in Renderables) {
                if (renderable.MarkedForDeletion) {
                    toBeRemoved.Add(renderable);
                } else {
                    UpdateObject(renderable);
                    renderable.Render(); 
                }
            }


            toBeRemoved.ForEach((renderable) => renderable.Destroy());
            Renderables.RemoveAll((renderable) => toBeRemoved.Contains(renderable));

            foreach(var renderable in AdditionQueue) {
                renderable.CreateControl(Controls);
                Renderables.Add(renderable);
            }

            AdditionQueue.Clear();
        }

        // Ezzel a metódussal adhatunk hozzá új objektumot a rajzoltak listájához
        private void QueueForRendering(Renderable renderable) {
            AdditionQueue.Add(renderable);
        }

        // Ezzel a metódussal törölhetünk egy objektumot a listából
        private void RemoveFromRendering(Renderable renderable) {
            renderable.MarkedForDeletion = true;
        }

        // Ez a metódus pedig kiüríti a teljes listát, ha elölről szeretnénk kezdeni, vagy úgy szinkronizálunk
        // a modellel, hogy mindig újracsináljuk
        private void RemoveAllFromRendering() {
            Renderables.ForEach((renderable) => renderable.MarkedForDeletion = true);
        }

        // Rajzolásnál ez a metódus hívódik meg minden objektumra, ha módosítani szeretnénk (ez is modellel való
        // szinkronizáláskor hasznos)
        private void UpdateObject(Renderable renderable) {
            renderable.Y += 3;

            if(renderable.Y > Height) {
                RemoveFromRendering(renderable);
            }
        }

        #region Demo-kód
        private void button1_Click(object sender, EventArgs e) {
            QueueForRendering(new SolidColourRenderable(Color.Blue) {
                X = _random.Next(0, Width - 16),
                Y = 0,
                Width = 16,
                Height = 16
            });
        }

        private void button2_Click(object sender, EventArgs e) {
            RemoveAllFromRendering();
        }

        private void button3_Click(object sender, EventArgs e) {
            QueueForRendering(new ImageRenderable(Resources._61911978_2660946003934748_3603680562973245440_n_451201142418935) {
                X = _random.Next(0, Width - 16),
                Y = 0,
                Width = 32,
                Height = 32
            });
        }
        #endregion
    }

    // Ezek az implementációk csak példának vannak itt főleg (persze lehet ezeket is használni), de a "helyes" megoldás az
    // lenne, ha ezek mintájára írunk egy saját Renderable alosztályt, ami a modellünkben levő objektumokkal
    // kommunikál.

    public class ImageRenderable : Renderable {
        private readonly Bitmap _bitmap;

        public ImageRenderable(Bitmap image) {
            _bitmap = image;
        }

        public override void CreateControl(Control.ControlCollection container) {
            AssociatedControl = new PictureBox() {
                Location = new(X, Y),
                Size = new(Width, Height),
                BackgroundImage = _bitmap,
                BackgroundImageLayout = ImageLayout.Stretch
            };

            container.Add(AssociatedControl);
            _container = container;
        }

        public override void Render() {
            AssociatedControl!.Location = new(X, Y);
            AssociatedControl!.Size = new(Width, Height);
        }
    }

    public class SolidColourRenderable : Renderable {
        private Color _color = Color.White;

        public SolidColourRenderable(Color color) { 
            _color = color;
        }

        public Color Color {
            get => _color;
            set {
                _color = value;
            }
        }

        public override void CreateControl(Control.ControlCollection container) {
            AssociatedControl = new Panel() {
                Location = new(X, Y),
                Size = new(Width, Height),
                BackColor = _color
            };

            container.Add(AssociatedControl);
            _container = container;
        }

        public override void Render() {
            AssociatedControl!.Location = new(X, Y);
            AssociatedControl!.Size = new(Width, Height);
            AssociatedControl!.BackColor = _color;
        }
    }

    public abstract class Renderable {
        protected Control.ControlCollection? _container = null;
        private Control? _associatedControl = null;

        private int _x = 0;
        private int _y = 0;
        private int _width = 16;
        private int _height = 16;

        private bool _markedForDeletion = false;

        public int X {
            get => _x;
            set {
                _x = value;
            }
        }

        public int Y {
            get => _y;
            set {
                _y = value;
            }
        }

        public int Width {
            get => _width;
            set {
                _width = value;
            }
        }

        public int Height {
            get => _height;
            set {
                _height = value;
            }
        }

        public bool MarkedForDeletion {
            get => _markedForDeletion;
            set {
                _markedForDeletion = value;
            }
        }

        public Control? AssociatedControl {
            get => _associatedControl;
            set {
                _associatedControl = value;
            }
        }

        public abstract void Render();
        public abstract void CreateControl(Control.ControlCollection container);

        public void Destroy() {
            if(_associatedControl != null && _container != null) {
                _container.Remove(_associatedControl);
            }
        }
    }
}