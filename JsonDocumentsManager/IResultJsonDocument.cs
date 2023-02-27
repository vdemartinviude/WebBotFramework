using Json.Path;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonDocumentsManager;

public interface IResultJsonDocument
{
    public void AddResultMessage(string topic, string message);

    public void AddResultMessage(string topic, string message, string title);

    public string GetDocument();

    public Task SaveDocument(string path);

    public void AddResultValue(string topic, string description, double value);
}