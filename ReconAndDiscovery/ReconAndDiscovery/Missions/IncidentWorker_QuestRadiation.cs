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
			site.def = SiteDefOfReconAndDiscovery.SiteRadiationQuest;
            SitePart radioactiveDust = new SitePart(site, SiteDefOfReconAndDiscovery.SitePart_RadioactiveDust, null);
            site.parts.Add(radioactiveDust);
			QuestComp_CountThings component = site.GetComponent<QuestComp_CountThings>();
			component.targetNumber = 200;
			component.ticksTarget = 60000;
			component.ticksHeld = 0;
			component.worldTileAffected = map.Tile;
			component.gameConditionCaused = GameConditionDef.Named("Radiation");
			component.StartQuest(ThingDef.Named("PlantPsychoid"));
			if (Rand.Value < 0.1f)
			{
                SitePart scatteredTreasure = new SitePart(site, SiteDefOfReconAndDiscovery.ScatteredTreasure, null);
                site.parts.Add(scatteredTreasure);
			}
			if (Rand.Value < 0.05f)
			{
                SitePart scatteredManhunters = new SitePart(site, SiteDefOfReconAndDiscovery.ScatteredManhunters, null);
                site.parts.Add(scatteredManhunters);
			}
			if (Rand.Value < 0.05f)
			{
                SitePart mechanoidForces = new SitePart(site, SiteDefOfReconAndDiscovery.MechanoidForces, null);
                site.parts.Add(mechanoidForces);
			}
			if (Rand.Value < 0.05f)
			{
                SitePart enemyRaidOnArrival = new SitePart(site, SiteDefOfReconAndDiscovery.EnemyRaidOnArrival, null);
                site.parts.Add(enemyRaidOnArrival);
			}
			Find.WorldObjects.Add(site);
			return site;
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = parms.target as Map;
			bool result;
			if (map == null)
			{
				result = false;
			}
			else if ((from wo in Find.WorldObjects.AllWorldObjects
			where wo is Site && (wo as Site).def == SiteDefOfReconAndDiscovery.QuakesQuest
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
					GameCondition gameCondition = GameConditionMaker.MakeCondition(GameConditionDef.Named("Radiation"), 60000 * num);
					map.gameConditionManager.RegisterCondition(gameCondition);
					site.GetComponent<TimeoutComp>().StartTimeout(num * 60000);
					base.SendStandardLetter(parms, site);
					result = true;
				}
			}
			return result;
		}
	}
}
