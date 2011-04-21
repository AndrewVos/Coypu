﻿using System;
using Coypu.Robustness;

namespace Coypu
{
	public class Session
	{
		private readonly Driver driver;
		private readonly RobustWrapper robustWrapper;

		public Session(Driver driver, RobustWrapper robustWrapper)
		{
			this.driver = driver;
			this.robustWrapper = robustWrapper;
		}

		public void ClickButton(string locator)
		{
			robustWrapper.Robustly(() => driver.Click(driver.FindButton(locator)));
		}

		public void ClickLink(string locator)
		{
			robustWrapper.Robustly(() => driver.Click(driver.FindLink(locator)));
		}

		public void Visit(string url)
		{
			driver.Visit(url);
		}

		public void Click(Node node)
		{
			robustWrapper.Robustly(() => driver.Click(node));
		}

		public Node FindButton(string locator)
		{
			return robustWrapper.Robustly(() => driver.FindButton(locator));
		}

		public Node FindLink(string locator)
		{
			return robustWrapper.Robustly(() => driver.FindLink(locator));
		}
	}
}