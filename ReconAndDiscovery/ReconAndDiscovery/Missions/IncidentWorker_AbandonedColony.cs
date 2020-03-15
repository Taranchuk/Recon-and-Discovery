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
                Site site = SiteMaker.MakeSite(SiteDefOfReconAndDiscovery.AbandonedColony,
                    tile, faction);
                site.parts.Add(SiteDefOfReconAndDiscovery.HoloDisk);
                if (Rand.Value < 0.3f)
                {
                    site.parts.Add(SiteDefOfReconAndDiscovery.ScatteredManhunters);
                }
                if (Rand.Value < 0.1f)
                {
                    site.parts.Add(SiteDefOfReconAndDiscovery.MechanoidForces);
                }
                if (Rand.Value < 0.05f)
                {
                    site.parts.Add(SiteDefOfReconAndDiscovery.Stargate);
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
