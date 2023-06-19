using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Exceptions.InternalServerError
{
    public class RelationCannotBeFoundException : InternalServerErrorException
    {
        public override LogLevel LogLevel { get; init; } = LogLevel.Error;
        public RelationCannotBeFoundException(string parentEntity, string childEntity, string parentEntityId) : base($"Entity '{childEntity}' doesn't have foreign key {parentEntityId} related to entity '{parentEntity}'") { }
    }
}
