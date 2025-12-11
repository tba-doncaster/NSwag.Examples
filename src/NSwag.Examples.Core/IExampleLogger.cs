using System;
using System.Collections.Generic;
using System.Text;

namespace NSwag.Examples.Core
{
    /// <summary>
    /// Simple logging abstraction to avoid dependency on Microsoft.Extensions.Logging
    /// </summary>
    public interface IExampleLogger
    {
        void LogWarning(string message);
    }
}
