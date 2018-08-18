using Model.University;
using NHibernate;
using NHibernateRepository.Repository;
using System;
using System.Linq;
using SOPS.Repositories.Abstract;

namespace SOPS.Repositories.Implementation
{
    public class CourseRepository : PersistRepository<int, Course>, ICourseRepository
    {
        public CourseRepository(ISession session) :
            base(session)
        {

        }
    }
}
