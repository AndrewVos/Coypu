﻿using System;
using System.Collections.Generic;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
	[TestFixture]
	public class When_finding_all_matching_elements : BrowserInteractionTests
	{
		[Test]
		public void FindAllCss_should_make_direct_call_to_underlying_driver()
		{
			Should_find_robustly(session.FindAllCss, driver.StubAllCss);
		}

		[Test]
		public void FindAllXPath_should_make_direct_call_to_underlying_driver()
		{
			Should_find_robustly(session.FindAllXPath, driver.StubAllXPath);
		}

		protected void Should_find_robustly(Func<string, IEnumerable<Element>> subject, Action<string, IEnumerable<Element>> stub)
		{
			var locator = "Find me " + DateTime.Now.Ticks;

			var expectedImmediateResult = new[] {new StubElement()};

			stub(locator, expectedImmediateResult);

			var actualImmediateResult = subject(locator);
			Assert.That(actualImmediateResult, Is.SameAs(expectedImmediateResult));

			Assert.That(spyRobustWrapper.DeferredFunctions, Is.Empty);
		}
	}
}