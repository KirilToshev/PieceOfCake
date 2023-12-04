using System.Linq.Expressions;

namespace PieceOfCake.Core.Common.Resources;

public interface IResources
{
    public ICommonTerms CommonTerms { get; }
    public IUserErrors UserErrors { get; }

    public string GenereteSentence (
        Expression<Func<IResources, string>> sentenceBaseExpression, 
        params Expression<Func<IResources, string>>[] wordsExpression);
}
