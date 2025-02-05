# Localization

Simple manager for handling localization.

Its main difference from most is the ability to get a translation of a string not by key but using one of the translated strings.

To start, you need to create a translation file and load it into a package.
This can be done in several ways.

## JSON
```csharp
var pack = LocalizationPack.FromJson("lang-pack.json");
```

The `lang-pack.json` package will look like this:
```json
[
  {
    "en": "Language select",
    "ru": "Выбор языка",
    "es": "Seleccionar idioma",
    "fr": "Sélection de la langue",
    "de": "Sprachauswahl",
    "it": "Selezione lingua"
  },
  {
    "en": "I agree",
    "ru": "Я согласен",
    "es": "Estoy de acuerdo",
    "fr": "Je suis d'accord",
    "de": "Ich stimme zu",
    "it": "Sono d'accordo"
  }
]
```

## Simplified Text Format
```csharp
var pack = LocalizationPack.FromLPack("lang-pack.lpack");
```

The `lang-pack.lpack` package will look like this:
```ini
en: Language select
ru: Выбор языка
es: Seleccionar idioma
fr: Sélection de la langue
de: Sprachauswahl
it: Selezione lingua

en: I agree
ru: Я согласен
es: Estoy de acuerdo
fr: Je suis d'accord
de: Ich stimme zu
it: Sono d'accordo
```

The separator `:` is used between the language code and the translation string.
You can use any characters after the colon except for new line characters.
Blocks are separated by one or more empty lines. It doesn't matter how many empty lines there are, as long as the block is separated by at least one empty line, it will be considered another translation block.

## Translation

You can get the translation using the `LocalizationPack` directly through the `Translate` method.
It ignores leading and trailing whitespace and case of characters.

Instead of keys, other strings are used. For example, in the "Language select" block - everything is a key!
You can get the translation using any of the strings as a key.

```csharp
pack.Translate("es", "Language select");            // -> "Seleccionar idioma"
pack.Translate("de", "Language select");            // -> "Sprachauswahl"
pack.Translate("de", "Sélection de la langue");     // -> "Sprachauswahl"
pack.Translate("ru", "Sprachauswahl");              // -> "Выбор языка"
```

## LocalizationManager

It is better to use `LocalizationManager`, which eliminates the need to specify the language code every time. Essentially, that's all it does.

```csharp
var manager = new LocalizationManager(pack, "es");

manager.Translate("Language select");               // -> "Seleccionar idioma"
manager.Translate("I agree");                       // -> "Estoy de acuerdo"

manager.code = "ru";

manager.Translate("Language select");               // -> "Выбор языка"
manager.Translate("I agree");                       // -> "Я согласен"

// If translation is not found
manager.code = "en";
manager.Translate("Información");                   // -> null
manager.TryTranslate("Información");                // -> "Información"
```

## LocalizedString

There is also a helper class `LocalizedString` that creates a string and translates it when converted to a string.

```csharp
var text = new LocalizedString(manager, "Sélection de la langue");
text.ToString();                // -> "Language select"
Console.WriteLine(text);        // -> "Language select"

var textTwo = manager.String("Sélection de la langue"); // -> LocalizedString
textTwo.ToString();             // -> "Language select"
Console.WriteLine(textTwo);     // -> "Language select"
```