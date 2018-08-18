using Model.Offers;
using System;
using System.Linq;

namespace SOPS.Services.Offers
{
    public interface IOfferDestructor
    {
        void Destroy(int id);
    }
}
