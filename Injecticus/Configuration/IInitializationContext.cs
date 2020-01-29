using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Injectikus.Configuration
{
    internal interface IInitializationContext
    {
        IObjectBuildingExpressionTreeBuilder Builder { get; }

        void AddAlias(string key, Expression expression);
        Expression GetAlias(string key);

        Type GetType(string str);

    }
}
