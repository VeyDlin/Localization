namespace Localization;


public class LocalizationManager {
    public string code { get; set; }

    public LocalizationPack? pack { get; set; }



    public LocalizationManager(LocalizationPack? pack = null, string? code = null) {
        this.pack = pack;
        this.code = code ?? "en";
    }


    public LocalizationManager(string? code = null, LocalizationPack? pack = null) {
        this.pack = pack;
        this.code = code ?? "en";
    }


    public LocalizedString String(string text) {
        return new(this, text);
    }


    public string? Translate(string? text) {
        if (pack is null || text is null) {
            return null;
        }

        return pack.Translate(code, text);
    }



    public string TryTranslate(string? text) => Translate(text) ?? text ?? string.Empty;
}