using System;
using System.Linq;
using Model.Students;
using System.Collections.Generic;
using Model.System;
using Model.Employees;
using Model.University;
using System.IO;
using DocGen;

namespace SOPS.Services.Contracts
{
    public class ContractGenerator : IContractGenerator
    {
        public void Generate(string templatePath, string saveFilePath, Student student, University university, Employee employee)
        {
            Dictionary<string, string> d = FillDictionary(student,  university, employee);

            FileStream template = new FileStream(templatePath, FileMode.Open, FileAccess.Read);
            FileStream document = new FileStream(saveFilePath, FileMode.OpenOrCreate);

            IDocumentGenerator doc = new DocumentGenerator();
            doc.Generate(template, document, d);

            template.Close();
            document.Close();
        }

        private Dictionary<string, string> FillDictionary(Student student, University university, Employee employee)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            d["$_CREATE_DATE_$"] = DateTime.Now.ToShortDateString();
            d["$_CREATE_YEAR_$"] = DateTime.Now.Year.ToString();

            string academicYear;
            if (DateTime.Now.Month < 10)
                academicYear = string.Format("{0}/{1}", DateTime.Now.Year - 1, DateTime.Now.Year);
            else
                academicYear = string.Format("{0}/{1}", DateTime.Now.Year, DateTime.Now.Year + 1);

            d["$_ACADEMIC_YEAR_$"] = academicYear;
            d["$_UNIVERSITY_NAME_$"] = university.Name;
            d["$_UNIVERSITY_ADDRESS_$"] = university.Address;

            if (student.Contract != null)
            {               
                d["$_UNIVERSITY_REPRESENTATIVE_$"] = student.Contract.UniversityRepresentative;
                d["$_COMPANY_REPRESENTATIVE_$"] = student.Contract.CompanyRepresentative;
                d["$_START_DATE_$"] = student.Contract.StartDate.ToShortDateString();
                d["$_END_DATE_$"] = student.Contract.EndDate.ToShortDateString();

                if (student.Contract.Company != null)
                {
                    d["$_COMPANY_NAME_$"] = student.Contract.Company.Name;
                    d["$_COMPANY_ADDRESS_$"] = student.Contract.Company.Street;
                    d["$_COMPANY_CITY_$"] = student.Contract.Company.City;
                    d["$_COMPANY_POSTAL_CODE_$"] = student.Contract.Company.PostalCode;
                }
            }

            d["$_STUDENT_NAME_$"] = student.FirstName;
            d["$_STUDENT_LASTNAME_$"] = student.LastName;
            d["$_STUDENT_ALBUM_$"] = student.Album;
            d["$_STUDENT_EMAIL_$"] = student.Email;
            d["$_STUDENT_PHONE_$"] = student.Phone;
            d["$_STUDENT_CITY_$"] = student.City;
            d["$_STUDENT_POSTAL_CODE_$"] = student.PostalCode;
            d["$_STUDENT_ADDRESS_$"] = student.Address;
            d["$_MANAGER_NAME_$"] = employee.FirstName;
            d["$_MANAGER_LASTNAME_$"] = employee.LastName;
            
            d["$_STUDENT_DEPARTMENT_$"] = student.Course.Department.Name;
            d["$_STUDENT_COURSE_$"] = student.Course.Name;

            return d;
        }
    }
}
