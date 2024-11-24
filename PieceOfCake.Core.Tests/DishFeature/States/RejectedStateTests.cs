using CSharpFunctionalExtensions;
using Moq;
using NUnit.Framework;
using PieceOfCake.Core.DishFeature.States;

namespace PieceOfCake.Core.Tests.DishFeature.States;
public class RejectedStateTests : TestsBase
{
    private readonly RejectedState _activeState;

    public RejectedStateTests ()
    {
        _activeState = new RejectedState(Resources);
    }

    [Test]
    public void RejectedState_To_Active_Is_Forbidden ()
    {
        var callbackMock = new Mock<Func<Result>>();
        callbackMock.Setup(x => x.Invoke()).Returns(Result.Success());

        var result = _activeState.Active(() => callbackMock.Object());
        callbackMock.Verify(funck => funck.Invoke(), Times.Never);
        Assert.That(result.IsFailure);
    }

    [Test]
    public void RejectedState_To_AwaitingApproval_Is_Forbidden ()
    {
        var callbackMock = new Mock<Func<Result>>();
        callbackMock.Setup(x => x.Invoke()).Returns(Result.Success());

        var result = _activeState.AwaitingApproval(() => callbackMock.Object());
        callbackMock.Verify(funck => funck.Invoke(), Times.Never);
        Assert.That(result.IsFailure);
    }

    [Test]
    public void RejectedState_To_Draft_Is_Allowed ()
    {
        var callbackMock = new Mock<Func<Result>>();
        callbackMock.Setup(x => x.Invoke()).Returns(Result.Success());

        var result = _activeState.Draft(() => callbackMock.Object());
        callbackMock.Verify(funck => funck.Invoke(), Times.Once);
        Assert.That(result.Value.State, Is.EqualTo(Core.DishFeature.Enumerations.DishState.Draft));
    }

    [Test]
    public void RejectedState_To_Draft_Should_Fail_If_Callback_Is_Error ()
    {
        var callbackMock = new Mock<Func<Result>>();
        callbackMock.Setup(x => x.Invoke()).Returns(Result.Failure("Error"));

        var result = _activeState.Draft(() => callbackMock.Object());
        callbackMock.Verify(func => func.Invoke(), Times.Once);
        Assert.That(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo("Error"));
    }

    [Test]
    public void RejectedState_To_Rejected_Is_Allowed ()
    {
        var callbackMock = new Mock<Func<Result>>();
        callbackMock.Setup(x => x.Invoke()).Returns(Result.Success());

        var result = _activeState.Rejected(() => callbackMock.Object());
        callbackMock.Verify(funck => funck.Invoke(), Times.Once);
        Assert.That(result.Value.State, Is.EqualTo(Core.DishFeature.Enumerations.DishState.Rejected));
    }
}
