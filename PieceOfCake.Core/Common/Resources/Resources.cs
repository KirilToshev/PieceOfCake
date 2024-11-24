using Microsoft.Extensions.Localization;
using System.Linq.Expressions;

namespace PieceOfCake.Core.Common.Resources;

public class Resources : IResources
{
    public Resources (
        IStringLocalizer<UserErrors> userErrorsResource,
        IStringLocalizer<CommonTerms> commonTermsResource
        )
    {
        UserErrors = new UserErrors(userErrorsResource);
        CommonTerms = new CommonTerms(commonTermsResource);
    }

    public IUserErrors UserErrors { get; private set; }

    public ICommonTerms CommonTerms { get; private set; }

    public string GenereteSentence (
        Expression<Func<IResources, string>> sentenceBaseExpression, 
        params Expression<Func<IResources, string>>[] wordsExpressions)
    {
        var sentenceBase = sentenceBaseExpression.Compile().Invoke(this);
        var words = wordsExpressions.Select(we => we.Compile().Invoke(this));
        return string.Format(sentenceBase, words.ToArray());
    }
}
