# Univerzális mentés / betöltés

Objektum mentése:
```c#
byte[] data = Serialiser.Serialise(valamiObjektum);
```

Betöltése:
```c#
ValamiTipus betoltott = Serialiser.Deserialise<ValamiTipus>(data);
```

Ha valami gond történne (úgyse fog™), akkor csatolva van a `SerialisationTemplate.bt` fájl, ami egy a 010 Editorban használható binary template, ezzel lehet manuálisan
átnézni mit is tartalmaz a byte halmaz ami generálva lett:

![kép](https://user-images.githubusercontent.com/17655680/203452194-cf8d34b9-75dc-43be-97ed-caf6758030c8.png)
