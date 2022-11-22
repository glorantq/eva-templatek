# Billentyűzet input Windows Forms-ban

Ehhez a formon be kell állítani egy Event Handlert a KeyPress eventre, ezt legegyszerűbben a designerben lehet megtenni:
![HdeRM1R.png](https://iili.io/HdeRM1R.png)

Utána a C# kódban így érdemes megírni a metódust, azért, hogy a Space gomb esetén itt se lopják el a gombok az eventet:
```c#
private void Form1_KeyPress(object sender, KeyPressEventArgs e) {
    gameDisplay.Focus(); // gameDisplay helyett itt bármi más ami nem egy gomb, jó kis hack

    // Itt lehet mindenféle jóságot kezdeni (e.KeyChar a lenyomott karakter)
}
```
