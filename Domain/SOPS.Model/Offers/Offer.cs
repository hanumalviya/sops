using Model.Companies;
using System;
using System.Linq;

namespace Model.Offers
{
    public class Offer
    {
        public virtual int Id { get; set; }
        public virtual OfferType Type { get; set; }
        public virtual Company Company { get; set; }

        public virtual string Title { get; set; }
        public virtual bool Approved { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string Trade { get; set; }
        public virtual string Description { get; set; }

        public Offer()
        {
            Type = new OfferType();
            Company = new Company();
        }
    }
}
