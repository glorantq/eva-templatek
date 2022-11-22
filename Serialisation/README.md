# Univerzális mentés / betöltés

Objektum mentése:
```c#
byte[] data = Serialiser.Serialise(valamiObjektum);
```

Betöltése:
```c#
ValamiTipus betoltott = Serialiser.Deserialise<ValamiTipus>(data);
```
