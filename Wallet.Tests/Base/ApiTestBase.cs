using Wallet.Tests.Helpers;
using Wallet.Tests.Infrastructure;

namespace Wallet.Tests.Base;

public abstract class ApiTestBase : IClassFixture<TestFactory>
{
    protected readonly TestHelper Helper;

    protected ApiTestBase(TestFactory factory)
    {
        var client = factory.CreateClient();
        Helper = new TestHelper(client);
    }
}