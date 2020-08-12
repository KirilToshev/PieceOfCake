using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PieceOfCake.Core.States
{
    public abstract class DishState
    {
        public abstract Result<DishState> Draft(Func<Result> callback);
        public abstract Result<DishState> AwaitingApproval(Func<Result> callback);
        public abstract Result<DishState> Rejected(Func<Result> callback);
        public abstract Result<DishState> Active(Func<Result> callback);
    }
}
