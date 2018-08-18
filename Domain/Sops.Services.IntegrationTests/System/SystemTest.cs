using NUnit.Framework;
using System;
using System.Linq;

namespace Sops.Services.IntegrationTests.Offers
{
    [TestFixture]
    class SystenTest : TestBase
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
        public void CanCreateUniversityDetails()
        {
            // arrange
            var university = universityDetailsCreator.Create("university", "address");

            // act
            university.Name = "uni";
            universityUpdater.Update(university);

            //Assert
            var currentUniversity = universityDetailsProvider.GetUniversity();
            Assert.That(currentUniversity.Name, Is.EqualTo("uni"), "university name is not equal to expected"); 
        }
    }
}
