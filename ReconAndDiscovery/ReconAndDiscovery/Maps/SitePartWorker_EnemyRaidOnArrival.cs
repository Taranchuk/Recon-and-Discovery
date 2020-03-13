using System;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;

namespace ReconAndDiscovery.Maps
{
	public class SitePartWorker_EnemyRaidOnArrival : SitePartWorker
	{
		public SitePartWorker_EnemyRaidOnArrival()
		{
		}

		public override void PostMapGenerate(Map map)
		{
			IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(Find.Storyteller.def, 3, map);
			incidentParms.forced = true;
			IntVec3 spawnCenter;
			if (RCellFinder.TryFindRandomPawnEntryCell(ref spawnCenter, map, 0f, (IntVec3 v) => v.Standable(map)))
			{
				incidentParms.spawnCenter = spawnCenter;
			}
			Faction faction;
			if ((from f in Find.FactionManager.AllFactions
			where !f.def.hidden && f.HostileTo(Faction.OfPlayer)
			select f).TryRandomElement(out faction))
			{
				IntVec3 spawnCenter2;
				if (CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c), map, CellFinder.EdgeRoadChance_Neutral, out spawnCenter2))
				{
					incidentParms.faction = faction;
					incidentParms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
					incidentParms.raidArrivalMode = 1;
					incidentParms.spawnCenter = spawnCenter2;
					incidentParms.points *= 20f;
					incidentParms.points = Math.Max(incidentParms.points, 250f);
					QueuedIncident qi = new QueuedIncident(new FiringIncident(ThingDefOfReconAndDiscovery.RaidEnemyQuest, null, incidentParms), Find.TickManager.TicksGame + Rand.RangeInclusive(5000, 15000));
					Find.Storyteller.incidentQueue.Add(qi);
				}
			}
		}

		[CompilerGenerated]
		private static bool <PostMapGenerate>m__0(Faction f)
		{
			return !f.def.hidden && f.HostileTo(Faction.OfPlayer);
		}

		[CompilerGenerated]
		private static Func<Faction, bool> <>f__am$cache0;

		[CompilerGenerated]
		private sealed class <PostMapGenerate>c__AnonStorey0
		{
			public <PostMapGenerate>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 v)
			{
				return v.Standable(this.map);
			}

			internal bool <>m__1(IntVec3 c)
			{
				return this.map.reachability.CanReachColony(c);
			}

			internal Map map;
		}
	}
}
