using Model.Students;
using NUnit.Framework;
using System;
using System.Linq;

namespace Sops.Services.IntegrationTests.Offers
{
    [TestFixture]
    class OffersTest : TestBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void CanCreateOffer()
        {
            var company = companyCreator.Create("company", "street", "city", "postal code", "email", "phone", "url", "desc");
            var offerType = offerTypeCreator.Create("testType");
            var offer = offerCreator.Create("title", "desc", "inf", company, offerType);

            Assert.That(company.Offers.Count, Is.EqualTo(1), "company should contain one offer");
            Assert.That(offer.Company, Is.Not.Null, "offer company can't be null");

            companyDestructor.Destroy(company.Id);

            var currentCompany = companiesProvider.GetCompany(company.Id);
            Assert.IsNull(currentCompany, "company should be null");
            var currenOffer = offerProvider.GetOffer(offer.Id);
            Assert.IsNull(currenOffer, "offer should be null");
        }
    }
}
