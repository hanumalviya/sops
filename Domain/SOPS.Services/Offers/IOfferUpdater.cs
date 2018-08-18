using Model.Offers;
using System;
using System.Linq;

namespace SOPS.Services.Offers
{
    public interface IOfferUpdater
    {
        void Update(Offer offer);
    }
}
