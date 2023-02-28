using Newtonsoft.Json.Linq;
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
    private JToken _JsonDocument { get; set; }

    public InputJsonDocument(string jsonFilePath)
    {
        string text = File.ReadAllText(jsonFilePath);
        //JsonDoc = JsonDocument.Parse(text);
        _JsonDocument = JToken.Parse(text);
    }

    public Task ReadJson(string jsonFilePath)
    {
        throw new NotImplementedException();
    }

    public string GetStringData(string jsonPath)
    {
        return (string)_JsonDocument.SelectToken(jsonPath);
    }

    public double? GetDoubleData(string jsonPath)
    {
        //var theJsonPath = JsonPath.Parse(jsonPath);
        //var matches = theJsonPath.Evaluate(JsonDoc.RootElement).Matches;
        //if (matches!.Count == 0)
        //{
        //    return null;
        //}
        //return matches[0].Value.GetDouble();
        return 0;
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