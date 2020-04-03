using CSharpFunctionalExtensions;
using PieceOfCake.Core.BusinessRules;
using PieceOfCake.Core.Entities;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PieceOfCake.BusinessRules
{
    public class MeasureUnitBr : IMeasureUnitBr
    {
        private readonly IResources _resources;
        private readonly IUnitOfWork _unitOfWork;

        public MeasureUnitBr(
            IResources resources,
            IUnitOfWork unitOfWork)
        {
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Result<IReadOnlyCollection<MeasureUnit>> Get()
        {
            var measureUnits = _unitOfWork.MeasureUnitRepository.Get();

            if (!measureUnits.Any())
                return Result.Failure<IReadOnlyCollection<MeasureUnit>>(
                    _resources.GenereteSentence(x => x.UserErrors.SequenceContainsNoElements));

            return Result.Success(measureUnits);
        }

        public Result<MeasureUnit> Get(int id)
        {
            var measureUnit = _unitOfWork.MeasureUnitRepository.GetById(id);

            if (measureUnit == null)
                return Result.Failure<MeasureUnit>(
                    _resources.GenereteSentence(x => x.UserErrors.IdNotFound, x => id.ToString()));

            return Result.Success(measureUnit);
        }

        public Result<MeasureUnit> Update(int id, string? name)
        {
            var measureUnit = _unitOfWork.MeasureUnitRepository.GetById(id);

            if (measureUnit == null)
                return Result.Failure<MeasureUnit>(
                    _resources.GenereteSentence(x => x.UserErrors.IdNotFound, x => id.ToString()));

            return measureUnit.Update(name, _resources, _unitOfWork)
                .Tap(x => { _unitOfWork.Save(); });
        }

        public Result<MeasureUnit> Create(string name)
        {
            return MeasureUnit.Create(name, _resources, _unitOfWork)
                .Tap(x => { 
                    _unitOfWork.Save(); 
                });
        }

        public Result Delete(int id)
        {
            return this.Get(id)
                .Tap(mu => _unitOfWork.MeasureUnitRepository.Delete(mu));
        }
    }
}
