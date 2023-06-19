using Microsoft.Extensions.Logging;
using MixedDreams.Application.Common;
using MixedDreams.Application.Common;
using MixedDreams.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Exceptions
{
    public class WrongMqttMessageSignatureException : BaseException
    {
        public WrongMqttMessageSignatureException(string topic, string payload, string excpectedType) : 
            base($"Client sent message on topic '${topic}' with wrong signature. Expected '${excpectedType}', received '${payload}'")
        {
        }

        public override LogLevel LogLevel { get; init; } = LogLevel.Error;
    }
}
