﻿using System;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;

namespace ReconAndDiscovery.Maps
{
	public class SitePartWorker_PsionicEmanator : SitePartWorker
	{
		public SitePartWorker_PsionicEmanator()
		{
		}

		public override void PostMapGenerate(Map map)
		{
			base.PostMapGenerate(map);
			IntVec3 intVec;
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map) && x.Fogged(map) && x.GetRoom(map, RegionType.Set_Passable).CellCount <= 30, map, out intVec))
			{
				Thing thing = ThingMaker.MakeThing(ThingDef.Named("PsionicEmanator"), null);
				GenSpawn.Spawn(thing, intVec, map);
			}
		}

		[CompilerGenerated]
		private sealed class <PostMapGenerate>c__AnonStorey0
		{
			public <PostMapGenerate>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return x.Standable(this.map) && x.Fogged(this.map) && x.GetRoom(this.map, RegionType.Set_Passable).CellCount <= 30;
			}

			internal Map map;
		}
	}
}
