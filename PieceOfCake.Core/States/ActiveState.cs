using CSharpFunctionalExtensions;
using PieceOfCake.Core.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace PieceOfCake.Core.States
{
    public class ActiveState : DishState
    {
        private IResources _resources;
        public ActiveState(IResources resources)
        {
            _resources = resources;
        }

        public override Enumerations.DishState State => Enumerations.DishState.Active;

        public override Result<DishState> Active(Func<Result> callback)
        {
            return callback.Invoke().Map<DishState>(() => this);
        }

        public override Result<DishState> AwaitingApproval(Func<Result> callback)
        {
            return Result.Failure<DishState>(_resources.GenereteSentence(x => 
            x.UserErrors.InvalidStateTransition, 
            x => nameof(Enumerations.DishState.Active), 
            x => nameof(Enumerations.DishState.AwaitingApproval)));
        }

        public override Result<DishState> Draft(Func<Result> callback)
        {
            var callbackResult = callback.Invoke();
            if (callbackResult.IsFailure)
                return callbackResult.ConvertFailure<DishState>();

            return Result.Success<DishState>(new DraftState(_resources));
        }

        public override Result<DishState> Rejected(Func<Result> callback)
        {
            var callbackResult = callback.Invoke();
            if (callbackResult.IsFailure)
                return callbackResult.ConvertFailure<DishState>();

            return Result.Success<DishState>(new RejectedState(_resources));
        }
    }
}
