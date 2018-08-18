using Model.Companies;
using System;
using System.Linq;

namespace Model.Students
{
    public class Contract
    {
        public virtual int Id { get; set; }
        public virtual string CompanyRepresentative { get; set; }
        public virtual string UniversityRepresentative { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual Student Student { get; set; }
        public virtual Company Company { get; set; }

        public Contract()
        {
            StartDate = EndDate = DateTime.Now;
        }
    }
}
