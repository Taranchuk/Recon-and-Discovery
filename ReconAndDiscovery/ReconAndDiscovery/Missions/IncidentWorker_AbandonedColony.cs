using System;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace ReconAndDiscovery.Missions
{
	public class IncidentWorker_AbandonedColony : IncidentWorker
	{
		public IncidentWorker_AbandonedColony()
		{
		}

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			return base.CanFireNowSub(target);
		}

		private Site MakeSite(Caravan c)
		{
			int tile;
			TileFinder.TryFindPassableTileWithTraversalDistance(c.Tile, 1, 2, ref tile, (int t) => !Find.WorldObjects.AnyMapParentAt(t), false);
			Site site = (Site)WorldObjectMaker.MakeWorldObject(SiteDefOfReconAndDiscovery.Adventure);
			site.Tile = tile;
			site.SetFaction(Find.FactionManager.FirstFactionOfDef(FactionDefOf.Spacer));
			site.core = SiteDefOfReconAndDiscovery.AbandonedColony;
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
			return site;
		}

		public virtual bool TryExecute(IncidentParms parms)
		{
			Caravan caravan = parms.target as Caravan;
			bool result;
			if (caravan == null)
			{
				result = false;
			}
			else
			{
				Site site = this.MakeSite(caravan);
				if (site == null)
				{
					result = false;
				}
				else
				{
					base.SendStandardLetter(site, new string[0]);
					result = true;
				}
			}
			return result;
		}

		[CompilerGenerated]
		private static bool <MakeSite>m__0(int t)
		{
			return !Find.WorldObjects.AnyMapParentAt(t);
		}

		[CompilerGenerated]
		private static Predicate<int> <>f__am$cache0;
	}
}
