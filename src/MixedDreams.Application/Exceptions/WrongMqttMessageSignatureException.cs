using Microsoft.Extensions.Logging;
using MixedDreams.Infrastructure.Common;
using MixedDreams.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Exceptions
{
    public class WrongMqttMessageSignatureException : Exception
    {
        public WrongMqttMessageSignatureException(string topic, string payload, string excpectedType) : 
            base($"Client sent message on topic '${topic}' with wrong signature. Expected '${excpectedType}', received '${payload}'")
        {
        }

        public LogLevel LogLevel { get; init; } = LogLevel.Error;
    }
}
