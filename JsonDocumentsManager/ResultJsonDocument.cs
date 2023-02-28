using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Unicode;
using System.Threading.Tasks.Dataflow;

namespace JsonDocumentsManager;

public class ResultJsonDocument : IResultJsonDocument
{
    private readonly JsonObject _rootJson;

    public ResultJsonDocument()
    {
        _rootJson = new();
    }

    public void AddResultMessage(string topic, string message)
    {
        if (!_rootJson.ContainsKey("Messages"))
        {
            _rootJson.Add("Messages", new JsonArray());
        }
        JsonArray messages = _rootJson["Messages"].AsArray();
        JsonObject keyValuePairs = new JsonObject();
        keyValuePairs.Add("topic", topic);
        keyValuePairs.Add("message", message);
        messages.Add(keyValuePairs);
    }

    public void AddResultMessage(string topic, string message, string title)
    {
        if (!_rootJson.ContainsKey("Messages"))
        {
            _rootJson.Add("Messages", new JsonArray());
        }
        JsonArray messages = _rootJson["Messages"].AsArray();
        JsonObject keyValuePairs = new JsonObject();
        keyValuePairs.Add("topic", topic);
        keyValuePairs.Add("message", message);
        keyValuePairs.Add("title", title);

        messages.Add(keyValuePairs);
    }

    public void AddResultValue(string topic, string description, double value)
    {
        if (!_rootJson.ContainsKey("Results"))
        {
            _rootJson.Add("Results", new JsonArray());
        }
        JsonArray results = _rootJson["Results"].AsArray();
        JsonObject keyValuePairs = new JsonObject();
        keyValuePairs.Add("topic", topic);
        keyValuePairs.Add("description", description);
        keyValuePairs.Add("value", value);
        results.Add(keyValuePairs);
    }

    public string GetDocument()
    {
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Latin1Supplement),
            WriteIndented = true
        };

        return JsonSerializer.Serialize(_rootJson, options);
    }

    public async Task SaveDocument(string path)
    {
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Latin1Supplement),
            WriteIndented = true
        };
        using (var arquivo = File.CreateText(path))
        {
            await arquivo.WriteAsync(JsonSerializer.Serialize(_rootJson, options));
        }
    }
}