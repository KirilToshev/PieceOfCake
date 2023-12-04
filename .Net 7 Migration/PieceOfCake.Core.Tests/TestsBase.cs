using PieceOfCake.Tests.Common;

namespace PieceOfCake.Core.Tests;
public class TestsBase : TestsCommon
{
    public TestsBase () : base(new ServicesRegistration().Register)
    {
    }
}
