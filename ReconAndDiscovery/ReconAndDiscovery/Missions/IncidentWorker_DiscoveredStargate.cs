using System;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace ReconAndDiscovery.Missions
{
	public class IncidentWorker_DiscoveredStargate : IncidentWorker
	{
		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			int num;
			return base.CanFireNowSub(target) && TileFinder.TryFindNewSiteTile(ref num);
		}

		private Site MakeSite(int days)
		{
			int num = -1;
			for (int i = 0; i < 20; i++)
			{
				num = TileFinder.RandomStartingTile();
				if (TileFinder.IsValidTileForNewSettlement(num, null))
				{
					break;
				}
				num = -1;
			}
			Site result;
			if (num == -1)
			{
				result = null;
			}
			else
			{
				Site site = (Site)WorldObjectMaker.MakeWorldObject(SiteDefOfReconAndDiscovery.Adventure);
				site.Tile = num;
				site.SetFaction(Find.FactionManager.RandomEnemyFaction(false, false, true));
				float value = Rand.Value;
				if ((double)value < 0.2)
				{
					site.core = SiteDefOfReconAndDiscovery.AbandonedCastle;
				}
				else if ((double)value < 0.4)
				{
					site.core = SiteDefOfReconAndDiscovery.AbandonedColony;
				}
				else if ((double)value < 0.6)
				{
					site.core = SiteCoreDefOf.PreciousLump;
				}
				else if ((double)value < 0.8)
				{
					site.core = SiteCoreDefOf.ItemStash;
				}
				else
				{
					site.core = SiteCoreDefOf.Nothing;
					site.parts.Add(SitePartDefOf.Outpost);
					site.parts.Add(SitePartDefOf.Turrets);
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
				site.GetComponent<TimeoutComp>().StartTimeout(days * 60000);
				Find.WorldObjects.Add(site);
				result = site;
			}
			return result;
		}

		public override bool TryExecute(IncidentParms parms)
		{
			Site site = this.MakeSite(10);
			bool result;
			if (site == null)
			{
				result = false;
			}
			else
			{
				base.SendStandardLetter(site, new string[0]);
				result = true;
			}
			return result;
		}
	}
}
