using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common.Resources;

namespace PieceOfCake.Core.DishFeature.States;

public class AwaitingApprovalState : DishState
{
    private IResources _resources;
    public AwaitingApprovalState (IResources resources)
    {
        _resources = resources;
    }

    public override Enumerations.DishState State => Enumerations.DishState.AwaitingApproval;

    public override Result<DishState> Active (Func<Result> callback)
    {
        return callback.Invoke()
            .Map<DishState>(() => new ActiveState(_resources));
    }

    public override Result<DishState> AwaitingApproval (Func<Result> callback)
    {
        return callback.Invoke().Map<DishState>(() => this);
    }

    public override Result<DishState> Draft (Func<Result> callback)
    {
        return callback.Invoke()
            .Map<DishState>(() => new DraftState(_resources));
    }

    public override Result<DishState> Rejected (Func<Result> callback)
    {
        return callback.Invoke()
            .Map<DishState>(() => new RejectedState(_resources));
    }
}
