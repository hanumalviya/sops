using System;
using System.Linq;

namespace SOPS.Services.Courses
{
    public interface ICourseDestructor
    {
        void Destroy(int id);
        bool CanBeDestroyed(int p);
    }
}
