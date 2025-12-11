using Microsoft.Extensions.Logging;
using NSwag.Examples.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nswag.Examples.AspNetCore
{
    /// <summary>
    /// Bridge to Microsoft.Extensions.Logging
    /// </summary>
    internal class MicrosoftExampleLogger : IExampleLogger
    {
        private readonly ILogger _logger;

        public MicrosoftExampleLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }
    }
}
