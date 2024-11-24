using CSharpFunctionalExtensions;

namespace PieceOfCake.Core.DishFeature.States;

public abstract class DishState
{
    public abstract Enumerations.DishState State { get; }
    public abstract Result<DishState> Draft (Func<Result> callback);
    public abstract Result<DishState> AwaitingApproval (Func<Result> callback);
    public abstract Result<DishState> Rejected (Func<Result> callback);
    public abstract Result<DishState> Active (Func<Result> callback);
}
