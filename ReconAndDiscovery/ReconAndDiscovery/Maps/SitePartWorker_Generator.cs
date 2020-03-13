using System;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;

namespace ReconAndDiscovery.Maps
{
	public class SitePartWorker_Generator : SitePartWorker
	{
		public SitePartWorker_Generator()
		{
		}

		public override void PostMapGenerate(Map map)
		{
			base.PostMapGenerate(map);
			IntVec3 intVec;
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map) && !x.Fogged(map), map, out intVec))
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.GeothermalGenerator, null);
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
				return x.Standable(this.map) && !x.Fogged(this.map);
			}

			internal Map map;
		}
	}
}
