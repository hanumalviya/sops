using Model.Offers;
using NHibernateRepository.Repository;
using System;
using System.Linq;

namespace SOPS.Repositories.Abstract
{
    public interface IOfferRepository : IPersistRepository<Offer>, IReadOnlyRepository<int, Offer>
    {
    }
}
