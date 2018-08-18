using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHMembership.Exceptions
{
    public class RoleServiceException : ServiceException
    {
        public RoleServiceException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }
}
