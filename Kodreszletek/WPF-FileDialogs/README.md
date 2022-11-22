# Fájlválasztó ablakok WPF-ben

### Megnyitás
```c#
OpenFileDialog openFileDialog = new() {
    Filter = "Tetris-mentések (*.tetris)|*.tetris",
    FilterIndex = 0,
    RestoreDirectory = true
};

if (openFileDialog.ShowDialog() == true) {
    using (Stream fileStream = openFileDialog.OpenFile()) {
        // Itt lehet mindent csinálni
    }
}
```

### Mentés
```c#
SaveFileDialog saveFileDialog = new() {
    Filter = "Tetris-mentések (*.tetris)|*.tetris",
    FilterIndex = 0,
    RestoreDirectory = true
};

if (saveFileDialog.ShowDialog() == true) {
    using (Stream fileStream = saveFileDialog.OpenFile()) {
        // Itt lehet mindent csinálni
    }
}
```
