using System;
using System.Linq.Expressions;

namespace PieceOfCake.BlazorApp.Resources
{
    public interface IResources
    {
        public ICommonTerms CommonTerms { get; }
    }
}