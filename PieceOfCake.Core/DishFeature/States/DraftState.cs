using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common.Resources;

namespace PieceOfCake.Core.DishFeature.States;

public class DraftState : DishState
{
    private IResources _resources;

    public DraftState (IResources resources)
    {
        _resources = resources;
    }

    public override Enumerations.DishState State => Enumerations.DishState.Draft;

    public override Result<DishState> Active (Func<Result> callback)
    {
        return Result.Failure<DishState>(_resources.GenereteSentence(x =>
        x.UserErrors.InvalidStateTransition,
        x => nameof(Enumerations.DishState.Draft),
        x => nameof(Enumerations.DishState.Active)));
    }

    public override Result<DishState> AwaitingApproval (Func<Result> callback)
    {
        return callback.Invoke()
            .Map<DishState>(() => new AwaitingApprovalState(_resources));
    }

    public override Result<DishState> Draft (Func<Result> callback)
    {
        return callback.Invoke().Map<DishState>(() => this);
    }

    public override Result<DishState> Rejected (Func<Result> callback)
    {
        return Result.Failure<DishState>(_resources.GenereteSentence(x =>
        x.UserErrors.InvalidStateTransition,
        x => nameof(Enumerations.DishState.Draft),
        x => nameof(Enumerations.DishState.Rejected)));
    }
}
