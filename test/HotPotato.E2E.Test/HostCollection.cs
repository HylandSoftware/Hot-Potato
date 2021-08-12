using Xunit;

namespace HotPotato.E2E.Test
{
	[CollectionDefinition("Host")]
	public class HostCollection : ICollectionFixture<HostFixture>
	{
		//https://xunit.net/docs/shared-context
		// This class has no code, and is never created. Its purpose is simply
		// to be the place to apply [CollectionDefinition] and all the
		// ICollectionFixture<> interfaces.
	}
}
