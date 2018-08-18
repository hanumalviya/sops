using LumenWorks.Framework.IO.Csv;
using Model.University;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SOPS.Services.Students
{
    public class CsvStudentsImporter : IStudentsImporter    
    {
        private readonly IStudentCreator _studentCreator;

        public CsvStudentsImporter(IStudentCreator studentCreator)
        {
            _studentCreator = studentCreator;
        }

        public void Import(Stream stream, Course course, Mode mode)
        {
            var students = ReadFromStream(stream);

            foreach (var s in students)
            {
                _studentCreator.Create(s.FirstName, s.LastName, s.Album, course, mode);
            }
        }

        private IEnumerable<StudentCSVModel> ReadFromStream(Stream stream)
        {
            var result = new List<StudentCSVModel>();

            using (CsvReader csv = new CsvReader(new StreamReader(stream), true, '\t'))
            {
                csv.GetFieldHeaders();
                while (csv.ReadNextRecord())
                {
                    var s = new StudentCSVModel()
                    {
                        LastName = csv["nazwisko"],
                        FirstName = csv[2],
                        Album = csv[4]
                    };

                    result.Add(s);
                }
            }


            return result;
        }
    }

    internal class StudentCSVModel
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Album { get; set; }
    }
}
