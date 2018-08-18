using Model.Offers;
using NHibernate;
using NHibernateRepository.Repository;
using SOPS.Repositories.Abstract;
using System;
using System.Linq;

namespace SOPS.Repositories.Implementation
{
    public class OfferTypeRepository : PersistRepository<int, OfferType>, IOfferTypeRepository
    {
        public OfferTypeRepository(ISession session) :
            base(session)
        {

        }
    }
}
