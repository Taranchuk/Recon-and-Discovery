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
                TileFinder.TryFindPassableTileWithTraversalDistance(caravan.Tile, 1, 2, out tile, (int t) => !Find.WorldObjects.AnyMapParentAt(t), false);
                Faction faction = Find.FactionManager.RandomEnemyFaction(true, false, true, TechLevel.Spacer);
                Site site = SiteMaker.MakeSite(SiteDefOfReconAndDiscovery.AbandonedColony, tile, faction);

                SitePart holoDisk = new SitePart(site, SiteDefOfReconAndDiscovery.HoloDisk, null);
                site.parts.Add(holoDisk);
                if (Rand.Value < 0.3f)
                {
                    SitePart scatteredManhunters = new SitePart(site, SiteDefOfReconAndDiscovery.ScatteredManhunters, null);
                    site.parts.Add(scatteredManhunters);
                }
                if (Rand.Value < 0.1f)
                {
                    SitePart mechanoidForces = new SitePart(site, SiteDefOfReconAndDiscovery.MechanoidForces, null);
                    site.parts.Add(mechanoidForces);
                }
                if (Rand.Value < 0.05f)
                {
                    SitePart stargate = new SitePart(site, SiteDefOfReconAndDiscovery.Stargate, null);

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
