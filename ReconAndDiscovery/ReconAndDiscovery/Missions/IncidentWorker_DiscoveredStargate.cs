using System;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace ReconAndDiscovery.Missions
{
	public class IncidentWorker_DiscoveredStargate : IncidentWorker
	{
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			int num;
			return base.CanFireNowSub(parms) && TileFinder.TryFindNewSiteTile(out num);
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
            int tile = -1;
            bool result;
            Faction faction = Find.FactionManager.RandomEnemyFaction(false, false, true);
            for (int i = 0; i < 20; i++)
            {
                tile = TileFinder.RandomSettlementTileFor(faction, false, null);
                if (TileFinder.IsValidTileForNewSettlement(tile, null))
                {
                    break;
                }
                else
                {
                    tile = -1;
                }
            }
            if (tile != -1)
            {
                Site site = (Site)WorldObjectMaker.MakeWorldObject(SiteDefOfReconAndDiscovery.Adventure);
                site.Tile = tile;
                site.SetFaction(faction);
                float value = Rand.Value;
                if ((double)value < 0.2)
                {
                    site.AddPart(new SitePart(site, SiteDefOfReconAndDiscovery.AbandonedCastle,
    SiteDefOfReconAndDiscovery.AbandonedCastle.Worker.GenerateDefaultParams
    (StorytellerUtility.DefaultSiteThreatPointsNow(), tile, faction)));
                }
                else if ((double)value < 0.4)
                {
                    site.AddPart(new SitePart(site, SiteDefOfReconAndDiscovery.AbandonedColony,
SiteDefOfReconAndDiscovery.AbandonedColony.Worker.GenerateDefaultParams
(StorytellerUtility.DefaultSiteThreatPointsNow(), tile, faction)));
                }
                else if ((double)value < 0.6)
                {
                    site.AddPart(new SitePart(site, SitePartDefOf.PreciousLump,
SitePartDefOf.PreciousLump.Worker.GenerateDefaultParams
(StorytellerUtility.DefaultSiteThreatPointsNow(), tile, faction)));
                }
                // TODO: figure out how to convert this to 1.1 code
                //else if ((double)value < 0.8)
                //{
                //	site.def = WorldObjectDefOf.ItemStash;
                //}
                else
                {
                    site = (Site)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Site);
                    site.Tile = tile;
                    site.SetFaction(faction);
                    // TODO: check if this works correctly
                    SitePart outpost = new SitePart(site, SitePartDefOf.Outpost, SitePartDefOf.Outpost.Worker.GenerateDefaultParams
(StorytellerUtility.DefaultSiteThreatPointsNow(), tile, faction));
                    site.parts.Add(outpost);
                    SitePart turrets = new SitePart(site, SitePartDefOf.Turrets, SitePartDefOf.Turrets.Worker.GenerateDefaultParams
(StorytellerUtility.DefaultSiteThreatPointsNow(), tile, faction));
                    site.parts.Add(turrets);
                }
                SitePart starGate = new SitePart(site, SiteDefOfReconAndDiscovery.Stargate, SiteDefOfReconAndDiscovery.Stargate.Worker.GenerateDefaultParams
(StorytellerUtility.DefaultSiteThreatPointsNow(), tile, faction));
                site.parts.Add(starGate);
                if (Rand.Value < 0.2f)
                {
                    SitePart scatteredManhunters = new SitePart(site, SiteDefOfReconAndDiscovery.ScatteredManhunters, SiteDefOfReconAndDiscovery.ScatteredManhunters.Worker.GenerateDefaultParams
(StorytellerUtility.DefaultSiteThreatPointsNow(), tile, faction));

                    site.parts.Add(scatteredManhunters);
                }
                if (Rand.Value < 0.85f)
                {
                    SitePart scatteredTreasure = new SitePart(site, SiteDefOfReconAndDiscovery.ScatteredTreasure,
                    SiteDefOfReconAndDiscovery.ScatteredTreasure.Worker.GenerateDefaultParams
                    (StorytellerUtility.DefaultSiteThreatPointsNow(), tile, faction));

                    site.parts.Add(scatteredTreasure);
                }
                site.GetComponent<TimeoutComp>().StartTimeout(10 * 60000);
                base.SendStandardLetter(parms, site);
                Find.WorldObjects.Add(site);
                result = true;
            }
            else
            {

                result = false;
            }

            return result;
		}
	}
}










