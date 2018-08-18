using FluentNHibernate.Mapping;
using Model.Offers;
using System;
using System.Linq;

namespace SOPS.Repositories.Mappings
{
    public class OfferMap : ClassMap<Offer>
    {
        public OfferMap()
        {
            Id(n => n.Id);
            References<OfferType>(n => n.Type);
            References(n => n.Company);

            Map(n => n.Title);
            Map(n => n.Approved);
            Map(n => n.Date);
            Map(n => n.Trade);
            Map(n => n.Description).CustomSqlType("nvarchar(500)");
        }
    }
}
