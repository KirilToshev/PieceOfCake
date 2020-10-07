using PieceOfCake.BlazorApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Services
{
    public class EventsService : IEventsService
    {
        public event EventHandler ProductCreation;

        public void OnProductCreation() => ProductCreation?.Invoke(this, EventArgs.Empty);
    }
}
