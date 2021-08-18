using HotPotato.OpenApi.Models;
using HotPotato.OpenApi.Results;
using Newtonsoft.Json;
using NUnit.Framework.Constraints;

namespace HotPotato.Extensions
{
	public class HotPotatoResultConstraint : Constraint
	{
		public override ConstraintResult ApplyTo<TActual>(TActual actual)
		{
			IResultCollector resultCollector = actual as IResultCollector;

			//type check necessary since ApplyTo can't be given a specific type constraint
			if (resultCollector is null)
			{
				Description = "an IResultCollector as input"; 
				return new ConstraintResult(this, actual, false);
			}

			Description = "an OverallResult of State.Pass";
			return new ConstraintResult(this, JsonConvert.SerializeObject(resultCollector.Results), resultCollector.OverallResult == State.Pass);
		}
	}

	public class Is : NUnit.Framework.Is
	{
		/// <summary>
		///   This method checks to see whether HotPotato reported Pass on all methods.
		///   If something failed, HotPotato's results will be printed to help troubleshoot what went wrong.
		/// </summary>
		/// <example><code>
		///	  Assert.That(ResultCollector, Is.ApprovedByHotPotato());
		/// </code></example>
		public static HotPotatoResultConstraint ApprovedByHotPotato()
		{
			return new HotPotatoResultConstraint();
		}
	}
}
