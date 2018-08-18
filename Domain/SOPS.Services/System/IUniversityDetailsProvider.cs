using System;
using System.Linq;
using Model.System;

namespace SOPS.Services.System
{
    public interface IUniversityDetailsProvider
    {
        University GetUniversity();
    }
}
