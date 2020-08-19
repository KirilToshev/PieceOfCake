using PieceOfCake.Shared.ViewModels.MeasureUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Services.Interfaces
{
    public interface IMeasureUnitHttpService
    {
        Task<IEnumerable<MeasureUnitVm>> GetAllMeasureUnits();
    }
}
