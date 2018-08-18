using Model.University;
using System;
using System.Linq;

namespace SOPS.Services.Courses
{
    public interface IModesCreator
    {
        Mode Create(string name);
    }
}
