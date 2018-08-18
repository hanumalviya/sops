using Model.System;
using System;
using System.Linq;

namespace SOPS.Services.System
{
    public interface IUniversityDetailsCreator
    {
        University Create(string name, string address);
    }
}
