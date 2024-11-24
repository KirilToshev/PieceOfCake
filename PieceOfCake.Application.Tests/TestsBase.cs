using PieceOfCake.Tests.Common;

namespace PieceOfCake.Application.Tests;
public class TestsBase : TestsCommon
{
    public TestsBase () : base(new ServicesRegistration().Register)
    {
    }
}
