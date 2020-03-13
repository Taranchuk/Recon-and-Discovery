using System;
using RimWorld;
using RimWorld.BaseGen;

namespace ReconAndDiscovery.Maps
{
	public class SymbolResolver_OldMilitaryBase : SymbolResolver
	{
		public SymbolResolver_OldMilitaryBase()
		{
		}

		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp);
		}

		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.rect = rp.rect.ContractedBy(1);
			resolveParams.wallStuff = ThingDefOf.BlocksGranite;
			resolveParams.SetCustom<int>("minRoomDimension", 6, false);
			BaseGen.symbolStack.Push("nestedRoomMaze", resolveParams);
			BaseGen.symbolStack.Push("edgeWalls", resolveParams);
			rp.wallStuff = ThingDefOf.Steel;
			BaseGen.symbolStack.Push("edgeWalls", rp);
			BaseGen.symbolStack.Push("floor", rp);
			BaseGen.symbolStack.Push("clear", rp);
		}
	}
}
