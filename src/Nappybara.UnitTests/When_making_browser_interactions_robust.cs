﻿using System;
using Nappybara.Robustness;
using NUnit.Framework;

namespace Nappybara.UnitTests
{
    [TestFixture]
    public class When_making_browser_interactions_robust
    {
        [Test]
        public void When_a_Function_throws_a_recurring_exception_It_should_retry_at_regular_intervals()
        {
            TimeSpan timeout = TimeSpan.FromMilliseconds(50);
            TimeSpan interval = TimeSpan.FromMilliseconds(10);
            var robustness = new WaitAndRetryRobustWrapper(timeout, interval);

            int tries = 0;
            Func<object> function = () =>
                                        {
                                            tries++;
                                            throw new ExplicitlyThrownTestException("Fails every time");
                                        };

            try
            {
                robustness.Robustly(function);
            }
            catch (ExplicitlyThrownTestException)
            {
            }

            Assert.That(tries, Is.EqualTo(5));
        }

        [Test]
        public void
            When_a_Function_throws_a_recurring_exception_It_should_retry_until_the_timeout_is_reached_then_rethrow()
        {
            TimeSpan expectedTimeout = TimeSpan.FromMilliseconds(123);
            TimeSpan interval = TimeSpan.FromMilliseconds(2);
            var robustness = new WaitAndRetryRobustWrapper(expectedTimeout, interval);

            Func<object> function = () => { throw new ExplicitlyThrownTestException("Fails every time"); };
            const int allowMilisecondsForFuncToReturn = 2;

            DateTime startTime = DateTime.Now;
            try
            {
                robustness.Robustly(function);
                Assert.Fail("Expected 'Fails every time' exception");
            }
            catch (ExplicitlyThrownTestException e)
            {
                Assert.That(e.Message, Is.EqualTo("Fails every time"));
            }
            DateTime endTime = DateTime.Now;

            TimeSpan actualDuration = (endTime - startTime);
            TimeSpan endOfTimeoutWindow = interval.Add(TimeSpan.FromMilliseconds(allowMilisecondsForFuncToReturn));
            Assert.That(actualDuration, Is.InRange(expectedTimeout,
                                                   expectedTimeout.Add(endOfTimeoutWindow)));
        }

        [Test]
        public void When_a_Function_throws_an_exception_first_time_It_should_retry()
        {
            var robustness = new WaitAndRetryRobustWrapper(TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(5));
            int tries = 0;
            var expectedReturnValue = new object();
            Func<object> function = () =>
                                        {
                                            tries++;
                                            if (tries == 1)
                                                throw new Exception("Fails first time");

                                            return expectedReturnValue;
                                        };

            object actualReturnValue = robustness.Robustly(function);

            Assert.That(tries, Is.EqualTo(2));
            Assert.That(actualReturnValue, Is.SameAs(expectedReturnValue));
        }

        [Test]
        public void When_an_Action_throws_a_recurring_exception_It_should_retry_at_regular_intervals()
        {
            TimeSpan timeout = TimeSpan.FromMilliseconds(50);
            TimeSpan interval = TimeSpan.FromMilliseconds(10);
            var robustness = new WaitAndRetryRobustWrapper(timeout, interval);

            int tries = 0;
            Action action = () =>
                                {
                                    tries++;
                                    throw new ExplicitlyThrownTestException("Fails every time");
                                };

            try
            {
                robustness.Robustly(action);
            }
            catch (ExplicitlyThrownTestException)
            {
            }

            Assert.That(tries, Is.EqualTo(5));
        }

        [Test]
        public void
            When_an_Action_throws_a_recurring_exception_It_should_retry_until_the_timeout_is_reached_then_rethrow()
        {
            TimeSpan expectedTimeout = TimeSpan.FromMilliseconds(123);
            TimeSpan interval = TimeSpan.FromMilliseconds(2);
            var robustness = new WaitAndRetryRobustWrapper(expectedTimeout, interval);

            const int allowMilisecondsForActionToReturn = 2;
            Action action = () => { throw new ExplicitlyThrownTestException("Fails every time"); };

            DateTime startTime = DateTime.Now;
            try
            {
                robustness.Robustly(action);
                Assert.Fail("Expected 'Fails every time' exception");
            }
            catch (ExplicitlyThrownTestException e)
            {
                Assert.That(e.Message, Is.EqualTo("Fails every time"));
            }
            DateTime endTime = DateTime.Now;

            TimeSpan actualDuration = (endTime - startTime);
            TimeSpan endOfTimeoutWindow = interval.Add(TimeSpan.FromMilliseconds(allowMilisecondsForActionToReturn));
            Assert.That(actualDuration, Is.InRange(expectedTimeout,
                                                   expectedTimeout.Add(endOfTimeoutWindow)));
        }

        [Test]
        public void When_an_Action_throws_an_exception_first_time_It_should_retry()
        {
            var robustness = new WaitAndRetryRobustWrapper(TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(5));
            int tries = 0;
            Action action = () =>
                                {
                                    tries++;
                                    if (tries == 1)
                                        throw new ExplicitlyThrownTestException("Fails first time");
                                };

            robustness.Robustly(action);

            Assert.That(tries, Is.EqualTo(2));
        }
    }

    public class ExplicitlyThrownTestException : Exception
    {
        public ExplicitlyThrownTestException(string message) : base(message)
        {
        }
    }
}