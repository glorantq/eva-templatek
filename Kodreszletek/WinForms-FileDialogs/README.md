# Fájlválasztó ablakok Windows Formsban

### Megnyitás
```c#
using(OpenFileDialog openFileDialog = new()) {
    openFileDialog.Filter = "Tetris-mentések (*.tetris)|*.tetris";
    openFileDialog.FilterIndex = 0;
    openFileDialog.RestoreDirectory = true;
    
    if(openFileDialog.ShowDialog() == DialogResult.OK) {
        using(Stream stream = openFileDialog.OpenFile()) {
            // Itt lehet mindent csinálni
        }
    }
}
```

### Mentés
```c#
SaveFileDialog saveFileDialog = new();
saveFileDialog.Filter = "Tetris-mentések (*.tetris)|*.tetris";
saveFileDialog.FilterIndex = 0;
saveFileDialog.RestoreDirectory = true;

if(saveFileDialog.ShowDialog() == DialogResult.OK) {
    using(Stream fileStream = saveFileDialog.OpenFile()) {
        // Itt lehet mindent csinálni
    }
}
```
