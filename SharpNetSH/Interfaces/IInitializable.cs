﻿using System;

namespace Ignite.SharpNetSH
{
	public interface IInitializable
	{
		void Initialize(String priorText, IConsoleHarness harness);
	}
}