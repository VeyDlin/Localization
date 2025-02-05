namespace Localization;


public class LocalizedString {
    private LocalizationManager localizationManager;
    private string text;


    public LocalizedString(LocalizationManager localizationManager, string text) {
        this.localizationManager = localizationManager;
        this.text = text;
    }


    public override string ToString() {
        return localizationManager.TryTranslate(text);
    }


    public static implicit operator string(LocalizedString localizedString) {
        return localizedString.ToString();
    }
}

