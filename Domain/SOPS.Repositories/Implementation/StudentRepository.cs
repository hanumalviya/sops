using Model.Students;
using NHibernate;
using NHibernateRepository.Repository;
using SOPS.Repositories.Abstract;
using System;
using System.Linq;

namespace SOPS.Repositories.Implementation
{
    public class StudentRepository : PersistRepository<int, Student>, IStudentRepository
    {
        public StudentRepository(ISession session) :
            base(session)
        {

        }
    }
}
