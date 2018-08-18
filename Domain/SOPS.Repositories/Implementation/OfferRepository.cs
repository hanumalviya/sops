using Model.Offers;
using NHibernate;
using NHibernateRepository.Repository;
using SOPS.Repositories.Abstract;
using System;
using System.Linq;

namespace SOPS.Repositories.Implementation
{
    public class OfferRepository : PersistRepository<int, Offer>, IOfferRepository
    {
        public OfferRepository(ISession session) :
            base(session)
        {

        }
    }
}
