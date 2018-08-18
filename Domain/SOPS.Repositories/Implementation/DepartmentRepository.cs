using Model.University;
using NHibernate;
using NHibernateRepository.Repository;
using System;
using System.Linq;
using SOPS.Repositories.Abstract;

namespace SOPS.Repositories.Implementation
{
    public class DepartmentRepository : PersistRepository<int, Department>, IDepartmentRepository
    {
        public DepartmentRepository(ISession session) :
            base(session)
        {

        }
    }
}
