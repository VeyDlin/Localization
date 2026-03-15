namespace Localization;


public interface ILocalizationPack {
    string? Translate(string translateCode, string? text);
}
