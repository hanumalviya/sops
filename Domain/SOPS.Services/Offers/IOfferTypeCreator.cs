using Model.Offers;
using System;
using System.Linq;

namespace SOPS.Services.Offers
{
    public interface IOfferTypeCreator
    {
        OfferType Create(string name);
    }
}
