using System;
using System.Collections.Generic;
using System.Text;

namespace NSwag.Examples.Core
{
    /// <summary>
    /// Null logger implementation
    /// </summary>
    public class NullExampleLogger : IExampleLogger
    {
        public static readonly NullExampleLogger Instance = new();

        public void LogWarning(string message)
        {
            // Do nothing
        }
    }
}
