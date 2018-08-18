using Model.University;
using System;
using System.IO;
using System.Linq;

namespace SOPS.Services.Students
{
    public interface IStudentsImporter
    {
        void Import(Stream stream, Course course, Mode mode);
    }
}
