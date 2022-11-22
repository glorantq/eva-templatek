# Billentyűzet input WPF-ben

Meglepően egyszerű, csak egy pár sor XAML kódot kell hozzáadni közvetlen a `Window` taghez:
```xaml
<Window.InputBindings>
    <KeyBinding Command="{Binding MoveLeftCommand}" Key="A"/>
    <KeyBinding Command="{Binding MoveRightCommand}" Key="D"/>
    <KeyBinding Command="{Binding MoveDownCommand}" Key="S"/>
    <KeyBinding Command="{Binding InstantDownCommand}" Key="Space"/>
    <KeyBinding Command="{Binding RotateCWCommand}" Key="E"/>
    <KeyBinding Command="{Binding RotateCCWCommand}" Key="Q"/>
    <KeyBinding Command="{Binding HoldPieceCommand}" Key="C"/>
</Window.InputBindings>
```

(Ha a Space gombot akarjuk használni, minden gombon be kell állítani hogy `Focusable="False"`, hogy ne azok vigyék el a kattintást)
