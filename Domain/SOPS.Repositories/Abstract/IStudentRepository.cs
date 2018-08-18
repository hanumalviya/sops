using Model.Students;
using NHibernateRepository.Repository;
using System;
using System.Linq;

namespace SOPS.Repositories.Abstract
{
    public interface IStudentRepository : IPersistRepository<Student>, IReadOnlyRepository<int, Student>
    {
    }
}
