using CSharpFunctionalExtensions;
using Moq;
using NUnit.Framework;
using PieceOfCake.Core.DishFeature.States;

namespace PieceOfCake.Core.Tests.DishFeature.States;
public class AwaitingApprovalStateTests : TestsBase
{
    private readonly AwaitingApprovalState _awaitingApproval;

    public AwaitingApprovalStateTests ()
    {
        _awaitingApproval = new AwaitingApprovalState(Resources);
    }

    [Test]
    public void AwaitingApprovalState_To_Active_Is_Allowed ()
    {
        var callbackMock = new Mock<Func<Result>>();
        callbackMock.Setup(x => x.Invoke()).Returns(Result.Success());

        var result = _awaitingApproval.Active(() => callbackMock.Object());
        callbackMock.Verify(func => func.Invoke(), Times.Once);
        Assert.That(result.Value.State, Is.EqualTo(Core.DishFeature.Enumerations.DishState.Active));
    }

    [Test]
    public void AwaitingApprovalState_To_Active_Should_Fail_If_Callback_Is_Error ()
    {
        var callbackMock = new Mock<Func<Result>>();
        callbackMock.Setup(x => x.Invoke()).Returns(Result.Failure("Error"));

        var result = _awaitingApproval.Active(() => callbackMock.Object());
        callbackMock.Verify(func => func.Invoke(), Times.Once);
        Assert.That(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo("Error"));
    }

    [Test]
    public void AwaitingApprovalState_To_AwaitingApproval_Is_Allowed ()
    {
        var callbackMock = new Mock<Func<Result>>();
        callbackMock.Setup(x => x.Invoke()).Returns(Result.Success());

        var result = _awaitingApproval.AwaitingApproval(() => callbackMock.Object());
        callbackMock.Verify(func => func.Invoke(), Times.Once);
        Assert.That(result.Value.State, Is.EqualTo(Core.DishFeature.Enumerations.DishState.AwaitingApproval));
    }

    [Test]
    public void AwaitingApprovalState_To_Draft_Is_Allowed ()
    {
        var callbackMock = new Mock<Func<Result>>();
        callbackMock.Setup(x => x.Invoke()).Returns(Result.Success());

        var result = _awaitingApproval.Draft(() => callbackMock.Object());
        callbackMock.Verify(func => func.Invoke(), Times.Once);
        Assert.That(result.Value.State, Is.EqualTo(Core.DishFeature.Enumerations.DishState.Draft));
    }

    [Test]
    public void AwaitingApprovalState_To_Rejected_Is_Allowed ()
    {
        var callbackMock = new Mock<Func<Result>>();
        callbackMock.Setup(x => x.Invoke()).Returns(Result.Success());

        var result = _awaitingApproval.Rejected(() => callbackMock.Object());
        callbackMock.Verify(func => func.Invoke(), Times.Once);
        Assert.That(result.Value.State, Is.EqualTo(Core.DishFeature.Enumerations.DishState.Rejected));
    }
}
