using Model.Companies;
using Model.Offers;
using System;
using System.Linq;

namespace SOPS.Services.Offers
{
    public interface IOfferCreator
    {
        Offer Create(string title, string description, string trade, Company company, OfferType type);
    }
}
