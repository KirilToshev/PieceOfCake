using CSharpFunctionalExtensions;
using PieceOfCake.Core.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace PieceOfCake.Core.States
{
    public class RejectedState : DishState
    {
        private IResources _resources;
        public RejectedState(IResources resources)
        {
            _resources = resources;
        }

        public override Result<DishState> Active(Func<Result> callback)
        {
            return Result.Failure<DishState>(_resources.GenereteSentence(x =>
            x.UserErrors.InvalidStateTransition,
            x => nameof(Enumerations.DishState.Rejected),
            x => nameof(Enumerations.DishState.Active)));
        }

        public override Result<DishState> AwaitingApproval(Func<Result> callback)
        {
            return Result.Failure<DishState>(_resources.GenereteSentence(x =>
            x.UserErrors.InvalidStateTransition,
            x => nameof(Enumerations.DishState.Rejected),
            x => nameof(Enumerations.DishState.AwaitingApproval)));
        }

        public override Result<DishState> Draft(Func<Result> callback)
        {
            return callback.Invoke()
                .Map<DishState>(() => new DraftState(_resources));
        }

        public override Result<DishState> Rejected(Func<Result> callback)
        {
            return Result.Success<DishState>(this);
        }
    }
}
