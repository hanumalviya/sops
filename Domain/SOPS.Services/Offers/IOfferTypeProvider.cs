using Model.Offers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SOPS.Services.Offers
{
    public interface IOfferTypeProvider
    {
        IList<OfferType> GetAllOfferTypes();
        OfferType GetOfferType(int id);
    }
}
