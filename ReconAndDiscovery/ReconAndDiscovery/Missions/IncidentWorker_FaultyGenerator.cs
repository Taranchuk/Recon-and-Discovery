using System;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace ReconAndDiscovery.Missions
{
	public class IncidentWorker_FaultyGenerator : IncidentWorker
	{
		public IncidentWorker_FaultyGenerator()
		{
		}

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			int num;
			return base.CanFireNowSub(target) && TileFinder.TryFindNewSiteTile(ref num);
		}

		private Site MakeSite(Map map)
		{
			int tile;
			TileFinder.TryFindNewSiteTile(ref tile);
			Site site = (Site)WorldObjectMaker.MakeWorldObject(SiteDefOfReconAndDiscovery.AdventureDestroyThing);
			site.Tile = tile;
			site.core = SiteDefOfReconAndDiscovery.QuakesQuest;
			site.parts.Add(SiteDefOfReconAndDiscovery.SitePart_FaultyGenerator);
			site.GetComponent<QuestComp_DestroyThing>().StartQuest(ThingDefOf.GeothermalGenerator);
			site.GetComponent<QuestComp_DestroyThing>().gameConditionCaused = GameConditionDef.Named("Tremors");
			site.GetComponent<QuestComp_DestroyThing>().worldTileAffected = map.Tile;
			if (Rand.Value < 0.05f)
			{
				site.parts.Add(SiteDefOfReconAndDiscovery.ScatteredTreasure);
			}
			if (Rand.Value < 0.1f)
			{
				site.parts.Add(SiteDefOfReconAndDiscovery.ScatteredManhunters);
			}
			if (Rand.Value < 0.1f)
			{
				site.parts.Add(SiteDefOfReconAndDiscovery.MechanoidForces);
			}
			if (Rand.Value < 0.05f)
			{
				site.parts.Add(SiteDefOfReconAndDiscovery.EnemyRaidOnArrival);
			}
			Find.WorldObjects.Add(site);
			return site;
		}

		public virtual bool TryExecute(IncidentParms parms)
		{
			Map map = parms.target as Map;
			bool result;
			if (map == null)
			{
				result = false;
			}
			else if ((from wo in Find.WorldObjects.AllWorldObjects
			where wo is Site && (wo as Site).core == SiteDefOfReconAndDiscovery.QuakesQuest
			select wo).Count<WorldObject>() > 0)
			{
				result = false;
			}
			else
			{
				Site site = this.MakeSite(map);
				if (site == null)
				{
					result = false;
				}
				else
				{
					int num = 30;
					GameCondition cond = GameConditionMaker.MakeCondition(GameConditionDef.Named("Tremors"), 60000 * num, 100);
					map.gameConditionManager.RegisterCondition(cond);
					site.GetComponent<TimeoutComp>().StartTimeout(num * 60000);
					base.SendStandardLetter(site, new string[0]);
					result = true;
				}
			}
			return result;
		}

		[CompilerGenerated]
		private static bool <TryExecute>m__0(WorldObject wo)
		{
			return wo is Site && (wo as Site).core == SiteDefOfReconAndDiscovery.QuakesQuest;
		}

		[CompilerGenerated]
		private static Func<WorldObject, bool> <>f__am$cache0;
	}
}
