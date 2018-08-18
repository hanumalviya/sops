using FluentNHibernate.Mapping;
using Model.Offers;
using System;
using System.Linq;

namespace SOPS.Repositories.Mappings
{
    public class OfferTypeMap : ClassMap<OfferType>
    {
        public OfferTypeMap()
        {
            Id(n => n.Id);
            Map(n => n.Name);
        }
    }
}
