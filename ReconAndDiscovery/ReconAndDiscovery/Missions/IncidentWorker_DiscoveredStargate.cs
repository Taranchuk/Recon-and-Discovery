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
            int num = -1;
			bool result;
            for (int i = 0; i < 20; i++)
            {
                num = TileFinder.RandomStartingTile();
                if (TileFinder.IsValidTileForNewSettlement(num, null))
                {
                    break;
                }
                num = -1;
            }
            if (num != -1)
            {
                Site site;
                Faction faction = Find.FactionManager.RandomEnemyFaction(false, false, true);
                float value = Rand.Value;
                if ((double)value < 0.2)
                {
                    site = SiteMaker.MakeSite(SiteDefOfReconAndDiscovery.AbandonedCastle, num, faction);
                }
                else if ((double)value < 0.4)
                {
                    site = SiteMaker.MakeSite(SiteDefOfReconAndDiscovery.AbandonedColony, num, faction);
                }
                else if ((double)value < 0.6)
                {
                    site = SiteMaker.MakeSite(SitePartDefOf.PreciousLump, num, faction);
                }
                // TODO: figure out how to convert this to 1.1 code
                //else if ((double)value < 0.8)
                //{
                //	site.def = WorldObjectDefOf.ItemStash;
                //}
                else
                {
                    site = (Site)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Site);
                    // TODO: check if this works correctly
                    SitePart outpost = new SitePart(site, SitePartDefOf.Outpost, null);
                    site.parts.Add(outpost);
                    SitePart turrets = new SitePart(site, SitePartDefOf.Turrets, null);
                    site.parts.Add(outpost);
                }
                site.parts.Add(SiteDefOfReconAndDiscovery.Stargate);
                if (Rand.Value < 0.2f)
                {
                    site.parts.Add(SiteDefOfReconAndDiscovery.ScatteredManhunters);
                }
                if (Rand.Value < 0.85f)
                {
                    site.parts.Add(SiteDefOfReconAndDiscovery.ScatteredTreasure);
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
