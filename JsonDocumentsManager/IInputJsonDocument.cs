using Json.Path;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonDocumentsManager;

public interface IInputJsonDocument
{
    public abstract Task ReadJson(string jsonFilePath);

    public abstract string GetStringData(JsonPath jsonPath);

    public abstract string GetStringData(string jsonPath);

    public abstract bool? GetBoolData(string jsonPath);
}