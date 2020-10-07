using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Services.Interfaces
{
    public interface IEventsService
    {
        event EventHandler ProductCreation;

        void OnProductCreation();
    }
}
