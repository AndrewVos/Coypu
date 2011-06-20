﻿using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_finding_then_hovering : BrowserInteractionTests
    {
        [Test]
        public void It_makes_robust_call_to_find_then_click_element_on_underlying_driver()
        {
            var element = new StubElement();
            var finderCalled = false;
            driver.StubCss("something.to hover", element);

            session.Hover(() =>
                          {
                              finderCalled = true;
                              return element;
                          });

            Assert.That(finderCalled, Is.False, "Finder not called robustly");
            Assert.That(driver.HoveredElements, Is.Empty, "Hover not called robustly");

            spyRobustWrapper.DeferredActions.Single()();

            Assert.That(finderCalled, Is.True);
            Assert.That(driver.HoveredElements, Has.Member(element));
        }
    }
}