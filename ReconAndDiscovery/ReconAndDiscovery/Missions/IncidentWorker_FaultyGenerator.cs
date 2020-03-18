using System;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace ReconAndDiscovery.Missions
{
    public class IncidentWorker_FaultyGenerator : IncidentWorker
    {
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            int num;
            return base.CanFireNowSub(parms) && TileFinder.TryFindNewSiteTile(out num);
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;
            bool result;
            if (map == null)
            {
                result = false;
            }

            else if ((from wo in Find.WorldObjects.Sites
                      where wo is Site && wo.parts.Select(x => x.def) == SiteDefOfReconAndDiscovery.QuakesQuest
                      select wo).Count<WorldObject>() > 0)
            {
                result = false;
            }
            else
            {
                int tile;
                if (TileFinder.TryFindNewSiteTile(out tile))
                {
                    Site site = (Site)WorldObjectMaker.MakeWorldObject(SiteDefOfReconAndDiscovery.AdventureDestroyThing);
                    site.Tile = tile;
                    site.AddPart(new SitePart(site, SiteDefOfReconAndDiscovery.QuakesQuest, 
                        SiteDefOfReconAndDiscovery.QuakesQuest.Worker.GenerateDefaultParams(StorytellerUtility.DefaultSiteThreatPointsNow(), tile, null)));
                    SitePart faultyGenerator = new SitePart(site, SiteDefOfReconAndDiscovery.SitePart_FaultyGenerator, 
                        SiteDefOfReconAndDiscovery.SitePart_FaultyGenerator.Worker.GenerateDefaultParams(StorytellerUtility.DefaultSiteThreatPointsNow(), tile, null));
                    site.parts.Add(faultyGenerator);
                    site.GetComponent<QuestComp_DestroyThing>().StartQuest(ThingDefOf.GeothermalGenerator);
                    site.GetComponent<QuestComp_DestroyThing>().gameConditionCaused = GameConditionDef.Named("Tremors");
                    site.GetComponent<QuestComp_DestroyThing>().worldTileAffected = map.Tile;
                    if (Rand.Value < 0.05f)
                    {
                        SitePart scatteredTreasure = new SitePart(site, SiteDefOfReconAndDiscovery.ScatteredTreasure, SiteDefOfReconAndDiscovery.ScatteredTreasure.Worker.GenerateDefaultParams(StorytellerUtility.DefaultSiteThreatPointsNow(), tile, null));
                        site.parts.Add(scatteredTreasure);
                    }
                    if (Rand.Value < 0.1f)
                    {
                        SitePart scatteredManhunters = new SitePart(site, SiteDefOfReconAndDiscovery.ScatteredManhunters, SiteDefOfReconAndDiscovery.ScatteredManhunters.Worker.GenerateDefaultParams(StorytellerUtility.DefaultSiteThreatPointsNow(), tile, null));
                        site.parts.Add(scatteredManhunters);
                    }
                    if (Rand.Value < 0.1f)
                    {
                        SitePart mechanoidForces = new SitePart(site, SiteDefOfReconAndDiscovery.MechanoidForces, SiteDefOfReconAndDiscovery.MechanoidForces.Worker.GenerateDefaultParams(StorytellerUtility.DefaultSiteThreatPointsNow(), tile, null));
                        site.parts.Add(mechanoidForces);
                    }
                    if (Rand.Value < 0.05f)
                    {
                        SitePart enemyRaidOnArrival = new SitePart(site, SiteDefOfReconAndDiscovery.EnemyRaidOnArrival, SiteDefOfReconAndDiscovery.EnemyRaidOnArrival.Worker.GenerateDefaultParams(StorytellerUtility.DefaultSiteThreatPointsNow(), tile, null));
                        site.parts.Add(enemyRaidOnArrival);
                    }
                    int num = 30;
                    GameCondition gameCondition = GameConditionMaker.MakeCondition(GameConditionDef.Named("Tremors"), 60000 * num);
                    map.gameConditionManager.RegisterCondition(gameCondition);
                    site.GetComponent<TimeoutComp>().StartTimeout(num * 60000);
                    base.SendStandardLetter(parms, site);
                    Find.WorldObjects.Add(site);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }
    }
}









