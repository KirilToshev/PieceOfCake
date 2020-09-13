using System;
using System.Collections.Generic;
using System.Text;

namespace PieceOfCake.Shared.ViewModels.Menu
{
    public class MenuVm
    {
        public byte ServingsPerDay { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
