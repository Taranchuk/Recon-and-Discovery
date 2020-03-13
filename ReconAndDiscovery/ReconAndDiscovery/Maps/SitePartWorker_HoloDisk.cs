using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;

namespace ReconAndDiscovery.Maps
{
	public class SitePartWorker_HoloDisk : SitePartWorker
	{
		public SitePartWorker_HoloDisk()
		{
		}

		public override void PostMapGenerate(Map map)
		{
			base.PostMapGenerate(map);
			IEnumerable<Pawn> enumerable = from p in map.mapPawns.AllPawnsSpawned
			where p.Faction == Find.FactionManager.FirstFactionOfDef(FactionDefOf.Spacer)
			select p;
			foreach (Pawn pawn in enumerable)
			{
				pawn.Destroy(DestroyMode.Vanish);
			}
			IntVec3 intVec;
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map) && x.Fogged(map) && x.GetRoom(map, RegionType.Set_Passable).CellCount <= 30, map, out intVec))
			{
				Thing thing = ThingMaker.MakeThing(ThingDef.Named("HoloDisk"), null);
				GenSpawn.Spawn(thing, intVec, map);
			}
		}

		[CompilerGenerated]
		private static bool <PostMapGenerate>m__0(Pawn p)
		{
			return p.Faction == Find.FactionManager.FirstFactionOfDef(FactionDefOf.Spacer);
		}

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

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
