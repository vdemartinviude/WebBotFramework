using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonDocumentsManager;

public interface IInputJsonDocument
{
    public abstract string GetStringData(string jsonPath);

    public abstract bool? GetBoolData(string jsonPath);
}