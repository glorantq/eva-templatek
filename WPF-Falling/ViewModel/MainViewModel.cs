using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Media;
using System.Windows.Threading;

namespace WPF_Falling.ViewModel {
    internal class MainViewModel : ViewModelBase {
        // Ez a lista tárol minden objektumot amit rajzolni kell, közvetlen ezt ne módosítsd
        public ObservableCollection<RenderableViewModel> Renderables { get; set; }

        // Ez a lista tárolja azokat az objektumokat amiket hozzá szeretnénk adni a fentebbi listához
        private List<RenderableViewModel> AdditionQueue = new();

        // Ezek a propertyk az ablak méretét tárolják, kétirányúan vannak bindolva: ha módosítod őket,
        // akkor megváltozik az ablak mérete, és ha megváltozik az ablak mérete, akkor ezek is 
        // frissülni fognak

        private int _windowWidth = 900;
        private int _windowHeight = 512;

        public int WindowWidth {
            get => _windowWidth;
            set {
                _windowWidth = value;
                OnPropertyChanged();
            }
        }

        public int WindowHeight {
            get => _windowHeight;
            set {
                _windowHeight = value;
                OnPropertyChanged();
            }
        }

        #region Demo-kód
        private Random _random = new();
        private DispatcherTimer _timer = new();
        public DelegateCommand ButtonClickCommand { get; set; }
        #endregion

        public MainViewModel() {
            Renderables = new();

            #region Demo-kód
            ButtonClickCommand = new((parameter) => {
                for(int i = 0; i < 15; i++) {
                    AdditionQueue.Add(new RenderableViewModel() {
                        X = _random.Next(0, WindowWidth - 16),
                        Y = _random.Next(0, 16),
                        Width = 16,
                        Height = 16,
                        Brush = new SolidColorBrush(_random.Next(0, 10) % 2 == 0 ? Colors.Red : Colors.Green)
                    });
                }
            });

            _timer.Interval = TimeSpan.FromMilliseconds(10);
            _timer.Tick += (sender, e) => {
                // Azért kell ez az egész, mert nem módosíthatjuk a Renderables listát miközben azon iterálunk
                List<RenderableViewModel> toRemove = new();

                foreach(var renderable in Renderables) {
                    renderable.Y += 3;

                    if(renderable.Y > WindowHeight) {
                        toRemove.Add(renderable);
                    }
                }

                // Amit ki kell venni azt kivesszük
                toRemove.ForEach((it) => Renderables.Remove(it));

                // Amit hozzá kell adni azt hozzáadjuk
                AdditionQueue.ForEach((it) => Renderables.Add(it));
                AdditionQueue.Clear();
            };
            _timer.Start();
            #endregion
        }
    }
}
