using Model.University;
using NHibernateRepository.Repository;
using System;
using System.Linq;

namespace SOPS.Repositories.Abstract
{
    public interface ICourseRepository : IPersistRepository<Course>, IReadOnlyRepository<int, Course>
    {
    }
}
