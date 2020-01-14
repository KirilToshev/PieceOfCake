using PieceOfCake.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PieceOfCake.Core.Persistence
{
    public interface IMeasureUnitRepository
    {
        MeasureUnit? Get(string name);

        void Create(MeasureUnit measureUnit);
    }
}
