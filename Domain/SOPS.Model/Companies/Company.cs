using Model.Offers;
using Model.Students;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Companies
{
    public class Company
    {
        public virtual int Id { get; set; }
        public virtual ICollection<Offer> Offers { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }

        public virtual string Name { get; set; }
        public virtual string Url { get; set; }
        public virtual string Street { get; set; }
        public virtual string City { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual string Email { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Description { get; set; }

        public Company()
        {
            Offers = new List<Offer>();
            Contracts = new List<Contract>();
        }        
    }
}
