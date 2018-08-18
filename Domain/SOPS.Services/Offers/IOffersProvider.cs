using Model.Offers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SOPS.Services.Offers
{
    public interface IOffersProvider
    {
        IList<Offer> GetOffers();
        IList<Offer> GetOffers(string filter);
        Offer GetOffer(int id);
    }
}
