using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    interface IInstanceBuilder
    {
        object BuildInstance(IContainer container);
    }
}
