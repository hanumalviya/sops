using System;
using System.Linq;
using Model.University;
using System.Collections.Generic;

namespace SOPS.Services.Courses
{
    public interface IModesProvider
    {
        IList<Mode> GetModes();
    }
}
