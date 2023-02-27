using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatesAndEvents;

public class EstimateParametersException : Exception
{
    public EstimateParametersException()
    {
    }

    public EstimateParametersException(string message) : base(message)
    {
    }

    public EstimateParametersException(string message, Exception inner) : base(message, inner)
    {
    }
}