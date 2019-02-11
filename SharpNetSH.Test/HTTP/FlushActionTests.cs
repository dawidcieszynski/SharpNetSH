﻿using SharpNetSH.Test.Spike;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharpNetSH.Test.HTTP
{
	[TestClass]
	public class FlushActionTests
	{
		[TestMethod]
		public void VerifyLogBufferOutput()
		{
			var harness = new StringHarness();
			new NetSH(harness).Http.Flush.LogBuffer();
			Assert.AreEqual("netsh http flush logbuffer", harness.Value);
		}
	}
}
