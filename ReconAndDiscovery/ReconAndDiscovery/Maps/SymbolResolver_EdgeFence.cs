﻿using System;
using RimWorld;
using RimWorld.BaseGen;
using Verse;

namespace ReconAndDiscovery.Maps
{
	public class SymbolResolver_EdgeFence : SymbolResolver
	{
		public SymbolResolver_EdgeFence()
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
			int num = -1;
			foreach (IntVec3 intVec in rect.EdgeCells)
			{
				num++;
				if (num % 3 == 0)
				{
					ThingDef wall = ThingDefOf.Wall;
					Thing thing = ThingMaker.MakeThing(wall, rp.wallStuff);
					GenSpawn.Spawn(thing, intVec, map);
				}
			}
		}
	}
}
