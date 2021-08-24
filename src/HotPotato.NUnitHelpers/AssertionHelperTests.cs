using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using NSubstitute;
using NUnit.Framework;

namespace HotPotato.NUnitHelpers
{
	#if DEBUG
	[TestFixture]
	class AssertionHelperTests
	{
		[Test]
		public void ApprovedByHotPotato_FailsOnWrongType()
		{
			//because this is a test ApprovedByHotPotato(), it's ok that this test is nothing but a single assert
			Assert.That("this is a string, not an IResultCollector", Is.Not.ApprovedByHotPotato());
		}

		[Test]
		public void ApprovedByHotPotato_ChecksOverallResult_PassState()
		{
			IResultCollector resultCollector = Substitute.For<IResultCollector>();
			resultCollector.OverallResult.Returns(State.Pass);
			Assert.That(resultCollector, Is.ApprovedByHotPotato());
		}

		[Test]
		[TestCase(State.Fail)]
		[TestCase(State.Inconclusive)]
		public void ApprovedByHotPotato_ChecksOverallResult_OtherStates(State state)
		{
			IResultCollector resultCollector = Substitute.For<IResultCollector>();
			resultCollector.OverallResult.Returns(state);
			Assert.That(resultCollector, Is.Not.ApprovedByHotPotato());
		}
	}
	#endif
}
