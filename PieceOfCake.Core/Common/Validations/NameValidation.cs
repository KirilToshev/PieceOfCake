using CSharpFunctionalExtensions;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using PieceOfCake.Core.ValueObjects;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace PieceOfCake.Core.Common.Validations
{
    public class NameValidation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResources _resources;
        public NameValidation(
            IUnitOfWork unitOfWork,
            IResources resources)
        {
            _unitOfWork = unitOfWork;
            _resources = resources;
        }

        public Result IsUnique<TEntity>(
            string? name,
            Expression<Func<TEntity, bool>> nameComparisonCondition)
            where TEntity : Entity
        {
            var repository = _unitOfWork.GetRepositoryByType<TEntity>();

            var measureUnit = repository.GetFirstOrDefault(nameComparisonCondition);
            if (measureUnit != null)
                return Result.Failure(_resources.GenereteSentence(x => x.UserErrors.NameAlreadyExists, x => name));
            return Result.Ok();
        }
    }
}
