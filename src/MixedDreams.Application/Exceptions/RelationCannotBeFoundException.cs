using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Exceptions
{
    public class RelationCannotBeFoundException : InternalServerErrorException
    {
        public RelationCannotBeFoundException(string parentEntity, string childEntity, string parentEntityId) : base($"Entity '{childEntity}' doesn't have foreign key {parentEntityId} related to entity '{parentEntity}'") { }
    }
}
