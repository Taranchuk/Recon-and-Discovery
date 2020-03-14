using System;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace ReconAndDiscovery.Missions
{
	public class IncidentWorker_QuestRadiation : IncidentWorker
	{
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			int num;
			return base.CanFireNowSub(parms) && TileFinder.TryFindNewSiteTile(out num);
		}

		private Site MakeSite(Map map)
		{
			int tile;
			TileFinder.TryFindNewSiteTile(out tile);
			Site site = (Site)WorldObjectMaker.MakeWorldObject(SiteDefOfReconAndDiscovery.AdventureThingCounter);
			site.Tile = tile;
			site.core = SiteDefOfReconAndDiscovery.SiteRadiationQuest;
			site.parts.Add(SiteDefOfReconAndDiscovery.SitePart_RadioactiveDust);
			QuestComp_CountThings component = site.GetComponent<QuestComp_CountThings>();
			component.targetNumber = 200;
			component.ticksTarget = 60000;
			component.ticksHeld = 0;
			component.worldTileAffected = map.Tile;
			component.gameConditionCaused = GameConditionDef.Named("Radiation");
			component.StartQuest(ThingDef.Named("PlantPsychoid"));
			if (Rand.Value < 0.1f)
			{
				site.parts.Add(SiteDefOfReconAndDiscovery.ScatteredTreasure);
			}
			if (Rand.Value < 0.05f)
			{
				site.parts.Add(SiteDefOfReconAndDiscovery.ScatteredManhunters);
			}
			if (Rand.Value < 0.05f)
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

		public override bool TryExecute(IncidentParms parms)
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
				else if (Find.World.tileTemperatures.GetSeasonalTemp(site.Tile) < 10f || Find.World.tileTemperatures.GetSeasonalTemp(site.Tile) > 40f)
				{
					result = false;
				}
				else
				{
					int num = 30;
					GameCondition gameCondition = GameConditionMaker.MakeCondition(GameConditionDef.Named("Radiation"), 60000 * num, 100);
					map.gameConditionManager.RegisterCondition(gameCondition);
					site.GetComponent<TimeoutComp>().StartTimeout(num * 60000);
					base.SendStandardLetter(site, new string[0]);
					result = true;
				}
			}
			return result;
		}
	}
}
