using RA.Utilities.ErrorHandling;

namespace RA.Utilities.Test.ErrorHandling
{
    internal class FakeCustomErrorHandlerBase(params Type[] ignoreException)
        : CustomErrorHandlerBase(ignoreException)
    {
    }
}
