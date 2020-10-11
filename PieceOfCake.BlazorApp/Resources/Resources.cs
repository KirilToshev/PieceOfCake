using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace PieceOfCake.BlazorApp.Resources
{
    public class Resources : IResources
    {
        public Resources(
            IStringLocalizer<CommonTerms> commonTermsResource
            )
        {
            CommonTerms = new CommonTerms(commonTermsResource);
        }

        public ICommonTerms CommonTerms { get; private set; }
    }
}
