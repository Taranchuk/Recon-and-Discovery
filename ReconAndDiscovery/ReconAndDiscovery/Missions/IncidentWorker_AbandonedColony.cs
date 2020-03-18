using System;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace ReconAndDiscovery.Missions
{
	public class IncidentWorker_AbandonedColony : IncidentWorker
	{
		protected override bool CanFireNowSub(IncidentParms parms)
		{
            return base.CanFireNowSub(parms);
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Caravan caravan = parms.target as Caravan;
			bool result;
			if (caravan == null)
			{
				result = false;
			}
			else
			{
                int tile;
                Site site = (Site)WorldObjectMaker.MakeWorldObject(SiteDefOfReconAndDiscovery.Adventure);
                TileFinder.TryFindPassableTileWithTraversalDistance(caravan.Tile, 1, 2, out tile, (int t) => !Find.WorldObjects.AnyMapParentAt(t), false);
                site.Tile = tile;
                Faction faction = Find.FactionManager.RandomEnemyFaction(true, false, true, TechLevel.Spacer);
                site.SetFaction(faction);

                site.AddPart(new SitePart(site, SiteDefOfReconAndDiscovery.AbandonedColony, SiteDefOfReconAndDiscovery.AbandonedColony.Worker.GenerateDefaultParams(StorytellerUtility.DefaultSiteThreatPointsNow(), tile, faction)));

                SitePart holoDisk = new SitePart(site, SiteDefOfReconAndDiscovery.HoloDisk, SiteDefOfReconAndDiscovery.HoloDisk.Worker.GenerateDefaultParams(StorytellerUtility.DefaultSiteThreatPointsNow(), tile, faction));
                site.parts.Add(holoDisk);
                if (Rand.Value < 0.3f)
                {
                    SitePart scatteredManhunters = new SitePart(site, SiteDefOfReconAndDiscovery.ScatteredManhunters, SiteDefOfReconAndDiscovery.ScatteredManhunters.Worker.GenerateDefaultParams
                    (StorytellerUtility.DefaultSiteThreatPointsNow(), tile, faction));
                    site.parts.Add(scatteredManhunters);
                }
                if (Rand.Value < 0.1f)
                {
                    SitePart mechanoidForces = new SitePart(site, SiteDefOfReconAndDiscovery.MechanoidForces, SiteDefOfReconAndDiscovery.MechanoidForces.Worker.GenerateDefaultParams
                    (StorytellerUtility.DefaultSiteThreatPointsNow(), tile, faction));
                    site.parts.Add(mechanoidForces);
                }
                if (Rand.Value < 0.05f)
                {
                    SitePart stargate = new SitePart(site, SiteDefOfReconAndDiscovery.Stargate, SiteDefOfReconAndDiscovery.Stargate.Worker.GenerateDefaultParams
                    (StorytellerUtility.DefaultSiteThreatPointsNow(), tile, faction));

                    site.parts.Add(stargate);
                }
                Find.WorldObjects.Add(site);
                if (site == null)
				{
					result = false;
				}
				else
				{
					base.SendStandardLetter(parms, caravan);
					result = true;
				}
			}
			return result;
		}
	}
}










