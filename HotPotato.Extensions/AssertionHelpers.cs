using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using Newtonsoft.Json;
using NUnit.Framework;

namespace HotPotato.Extensions
{
	public class AssertionHelpers
	{
		public void AssertHotPotatoApproves(IResultCollector resultCollector)
		{
			//TODO: after some thinking, it seems weird that this project would have to refer to its own NUnit reference when its caller would already have to be using an NUnit itself. Ideally this method would be more of a macro to be performed by its caller than logic performed within this project.
			Assert.That(resultCollector.OverallResult, Is.EqualTo(State.Pass), JsonConvert.SerializeObject(resultCollector.Results));
		}
	}
}
