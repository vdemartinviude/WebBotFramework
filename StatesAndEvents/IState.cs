using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatesAndEvents;

public interface IState : IComparable
{
    public abstract TimeSpan StateTimeout { get; }
}