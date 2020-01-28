using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus.Configuration
{
    internal interface IInitializationContext
    {
        Type GetType(string str);
    }
}
