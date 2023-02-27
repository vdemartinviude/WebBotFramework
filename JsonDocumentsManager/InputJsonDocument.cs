using Json.Path;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace JsonDocumentsManager;

public class InputJsonDocument : IInputJsonDocument
{
    public JsonDocument JsonDoc { get; set; }

    public InputJsonDocument(string jsonFilePath)
    {
        string text = File.ReadAllText(jsonFilePath);
        JsonDoc = JsonDocument.Parse(text);
    }

    public string GetStringData(JsonPath jsonPath)
    {
        var matches = jsonPath.Evaluate(JsonDoc.RootElement).Matches;
        if (matches == null || matches.Count == 0)
            return String.Empty;
        return matches![0].Value.ToString();
    }

    public Task ReadJson(string jsonFilePath)
    {
        throw new NotImplementedException();
    }

    public string GetStringData(string jsonPath)
    {
        var theJsonPath = JsonPath.Parse(jsonPath);
        return GetStringData(theJsonPath);
    }

    public double? GetDoubleData(string jsonPath)
    {
        var theJsonPath = JsonPath.Parse(jsonPath);
        var matches = theJsonPath.Evaluate(JsonDoc.RootElement).Matches;
        if (matches!.Count == 0)
        {
            return null;
        }
        return matches[0].Value.GetDouble();
    }

    public bool? GetBoolData(string jsonPath)
    {
        var theJsonPath = JsonPath.Parse(jsonPath);
        var matches = theJsonPath.Evaluate(JsonDoc.RootElement).Matches;
        if (matches!.Count == 0)
        {
            return null;
        }
        return matches[0].Value.GetBoolean();
    }
}