using System.Text.Json;

namespace Localization;


public class LocalizationPack {

    private Dictionary<int, Dictionary<string, string>> pack = new();   // <id, <code, text>>

    private Dictionary<string, Dictionary<string, int>> links = new();  // <code, <text, id>>


    // [<code, text>]
    public LocalizationPack(List<Dictionary<string, string>> arrayPack) {
        for (int i = 0; i < arrayPack.Count; i++) {
            var lines = arrayPack[i]; // <code, text>
            pack.Add(i, lines);

            foreach (var line in lines) {
                if (!links.ContainsKey(line.Key)) {
                    links.Add(line.Key, new());
                }

                links[line.Key].Add(line.Value.ToLower(), i);
            }
        }
    }



    public static LocalizationPack FromJson(FileInfo jsonFile) {
        var json = File.ReadAllText(jsonFile.FullName);
        var arrayPack = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);
        return new LocalizationPack(arrayPack!);
    }



    public static LocalizationPack FromLPack(FileInfo lPackFile) {
        // Split the input text into lines
        var text = File.ReadAllText(lPackFile.FullName);
        var lines = text.Split(["\r\n", "\n"], StringSplitOptions.None);
        var arrayPack = new List<Dictionary<string, string>>();
        var currentBlock = new Dictionary<string, string>();

        for (int i = 0; i < lines.Length; i++) {
            var line = lines[i].Trim();

            // If the line is empty, it separates blocks
            if (string.IsNullOrWhiteSpace(line)) {
                if (currentBlock.Count > 0) {
                    arrayPack.Add(currentBlock);
                    currentBlock = new Dictionary<string, string>();
                }
                continue;
            }

            // Check if the line contains a colon
            var colonIndex = line.IndexOf(':');
            if (colonIndex <= 0 || colonIndex == line.Length - 1) {
                throw new FormatException($"Invalid line format at line {i + 1}: \"{line}\"");
            }

            // Extract the code (key) and the text (value)
            var code = line.Substring(0, colonIndex).Trim();
            var textValue = line.Substring(colonIndex + 1).Trim();

            // Ensure the code is not duplicated within the same block
            if (currentBlock.ContainsKey(code)) {
                throw new FormatException($"Duplicate code \"{code}\" in block at line {i + 1}");
            }

            // Add the code and text to the current block
            currentBlock[code] = textValue;
        }

        // Add the last block if it is not empty
        if (currentBlock.Count > 0) {
            arrayPack.Add(currentBlock);
        }

        return new LocalizationPack(arrayPack);
    }



    public string? Translate(string translateCode, string? text) {
        if (text is null) {
            return null;
        }

        foreach (var link in links) {
            if (link.Value.ContainsKey(text.Trim().ToLower())) {

                var id = link.Value[text.Trim().ToLower()];
                if (pack[id].ContainsKey(translateCode)) {
                    return pack[id][translateCode];
                }
            }
        }

        return null;
    }
}