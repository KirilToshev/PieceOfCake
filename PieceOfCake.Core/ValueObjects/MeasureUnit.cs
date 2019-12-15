using CSharpFunctionalExtensions;
using System;

namespace PieceOfCake.Core.ValueObjects
{
    public class MeasureUnit
    {
        private MeasureUnit(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }

        public static Result <MeasureUnit> Create(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Fail<MeasureUnit>("Error");
             
            return Result.Ok(new MeasureUnit(name));
        }
    }
}
