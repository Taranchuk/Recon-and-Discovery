using System;
using RimWorld;
using RimWorld.BaseGen;
using Verse;

namespace ReconAndDiscovery.Maps
{
	public class SymbolResolver_EdgeShields : SymbolResolver
	{
		public SymbolResolver_EdgeShields()
		{
		}

		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp);
		}

		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			CellRect rect = rp.rect;
			if (rp.wallStuff == null)
			{
				rp.wallStuff = BaseGenUtility.RandomCheapWallStuff(Faction.OfPlayer, false);
			}
			int num = 1;
			foreach (IntVec3 intVec in rect.EdgeCells)
			{
				ThingDef def = ThingDefOf.Wall;
				Thing thing = ThingMaker.MakeThing(def, rp.wallStuff);
				if (num % 3 == 0)
				{
					def = ThingDefOf.Sandbags;
					thing = ThingMaker.MakeThing(def, null);
				}
				num++;
				GenSpawn.Spawn(thing, intVec, map);
			}
		}
	}
}
