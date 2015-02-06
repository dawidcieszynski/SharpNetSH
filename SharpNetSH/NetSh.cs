﻿namespace Ignite.SharpNetSH
{
	public class NetSH : INetSH
	{
		private readonly IExecutionHarness _harness;

		public NetSH(IExecutionHarness harness)
		{
			_harness = harness;
		}

		public IHttpAction Http { get { return HttpAction.CreateAction("netsh", _harness); } }
	}
}