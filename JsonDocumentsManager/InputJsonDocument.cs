using Newtonsoft.Json.Linq;

namespace JsonDocumentsManager;

public class InputJsonDocument : IInputJsonDocument
{
    private JToken _JsonDocument;

    public InputJsonDocument(string jsonFilePath)
    {
        string text = File.ReadAllText(jsonFilePath);
        _JsonDocument = JToken.Parse(text);
    }

    public string GetStringData(string jsonPath)
    {
        return (string)_JsonDocument.SelectToken(jsonPath);
    }

    public bool? GetBoolData(string jsonPath)
    {
        //var theJsonPath = JsonPath.Parse(jsonPath);
        //var matches = theJsonPath.Evaluate(JsonDoc.RootElement).Matches;
        //if (matches!.Count == 0)
        //{
        //    return null;
        //}
        //return matches[0].Value.GetBoolean();
        return false;
    }
}