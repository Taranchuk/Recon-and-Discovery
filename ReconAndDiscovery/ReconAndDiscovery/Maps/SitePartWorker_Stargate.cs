using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;

namespace ReconAndDiscovery.Maps
{
	public class SitePartWorker_Stargate : SitePartWorker
	{
		public SitePartWorker_Stargate()
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
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map) && !x.Fogged(map), map, out intVec))
			{
				Thing thing = ThingMaker.MakeThing(ThingDef.Named("Stargate"), null);
				GenSpawn.Spawn(thing, intVec, map);
				foreach (Pawn pawn2 in SitePartWorker_Stargate.tmpPawnsToSpawn)
				{
					if (pawn2.Spawned)
					{
						pawn2.DeSpawn();
					}
					GenSpawn.Spawn(pawn2, intVec, map);
				}
				SitePartWorker_Stargate.tmpPawnsToSpawn.Clear();
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static SitePartWorker_Stargate()
		{
		}

		[CompilerGenerated]
		private static bool <PostMapGenerate>m__0(Pawn p)
		{
			return p.Faction == Find.FactionManager.FirstFactionOfDef(FactionDefOf.Spacer);
		}

		public static List<Pawn> tmpPawnsToSpawn = new List<Pawn>();

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
				return x.Standable(this.map) && !x.Fogged(this.map);
			}

			internal Map map;
		}
	}
}
