﻿using System;
using RimWorld.BaseGen;

namespace ReconAndDiscovery.Maps
{
	public class SymbolResolver_PathOfDestruction : SymbolResolver
	{
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp);
		}

		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("clear", rp);
		}
	}
}