# WPF mátrix gombokkal

> Ez ugyan az mint a WPF-PanelMatrix, csak Rectangle helyett Button a control ami megjelenítésre kerül. Arra legalább jó ez 
> a kódrészlet, hogy megmutassa, hogyan lehet egyszerűen változtatni a cellák mögötti controlt a viewmodel átírása nélkül.

Template olyan játékokhoz, ahol WPF-et kell használni, és Button controlokkal szeretnénk megoldani 
a megjelnítést.

Az egész egy demo projekt, bemutatva azt hogyan kell használni. Nevezd át vagy valami, legegyszerűbb ha ebből a 
projektből dolgozol.

A kódban kommentek magyarázzák el mi történik, de a lényegesek azok a propertyk, amik a mátrixot szabályozzák,
a `MainViewModel`-ben, valamint ugyanitt az `OnCellBeingCreated` és az `OnCellClicked` metódus.

![Hd7U7hG.png](https://iili.io/Hd7U7hG.png)
