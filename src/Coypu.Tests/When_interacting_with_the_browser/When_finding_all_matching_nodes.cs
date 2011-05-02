﻿using System;
using System.Collections.Generic;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
	[TestFixture]
	public class When_finding_all_matching_nodes
	{
		private FakeDriver driver;
		private SpyRobustWrapper spyRobustWrapper;
		private Session session;

		[SetUp]
		public void SetUp()
		{
			driver = new FakeDriver();
			spyRobustWrapper = new SpyRobustWrapper();
			session = new Session(driver, spyRobustWrapper);
		}

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

		protected void Should_find_robustly(Func<string, IEnumerable<Node>> subject, Action<string, IEnumerable<Node>> stub)
		{
			var locator = "Find me " + DateTime.Now.Ticks;

			var expectedImmediateResult = new[] {new StubNode()};

			stub(locator, expectedImmediateResult);

			var actualImmediateResult = subject(locator);
			Assert.That(actualImmediateResult, Is.SameAs(expectedImmediateResult));

			Assert.That(spyRobustWrapper.DeferredFunctions, Is.Empty);
		}
	}
}