using Griffin.MvcContrib.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Griffin.MvcContrib.Localization.ValidationMessages;
using System.Web.Mvc;

namespace SOPS.WebUI
{
    public static class ValidatorConfig
    {
        public static void RegisterValidators()
        {
            var stringProvider = new ResourceStringProvider(Resources.LocalizedStrings.ResourceManager);
            ModelMetadataProviders.Current = new LocalizedModelMetadataProvider(stringProvider);
            ModelValidatorProviders.Providers.Clear();   

            ValidationMessageProviders.Clear();
            ValidationMessageProviders.Add(new MvcDataSource());
            ValidationMessageProviders.Add(new DataAnnotationDefaultStrings());
            ValidationMessageProviders.Add(new GriffinStringsProvider(stringProvider));

            var validatorProvider = new LocalizedModelValidatorProvider();
            ModelValidatorProviders.Providers.Add(validatorProvider);
        }
    }
}