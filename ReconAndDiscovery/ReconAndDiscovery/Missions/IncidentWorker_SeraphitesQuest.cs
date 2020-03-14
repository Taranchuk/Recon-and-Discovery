using System;
using System.Collections.Generic;
using System.Linq;
using ReconAndDiscovery.Maps;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace ReconAndDiscovery.Missions
{
	public class IncidentWorker_SeraphitesQuest : IncidentWorker
	{
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			int num;
			return base.CanFireNowSub(parms) && TileFinder.TryFindNewSiteTile(out num);
		}

		private bool CanFindVisitor(Map map, out Pawn pawn)
		{
			pawn = null;
			IEnumerable<Pawn> source = from p in map.mapPawns.AllPawnsSpawned
			where p.RaceProps.Humanlike && p.Faction != Faction.OfPlayer && p.Faction.PlayerGoodwill > 0f
			select p;
			bool result;
			if (source.Count<Pawn>() == 0)
			{
				result = false;
			}
			else
			{
				pawn = source.RandomElement<Pawn>();
				result = true;
			}
			return result;
		}

		private bool CanFindLuciferiumAddict(Map map, out Pawn pawn)
		{
			pawn = null;
			foreach (Pawn pawn2 in map.mapPawns.FreeColonists)
			{
				if (AddictionUtility.IsAddicted(pawn2, ThingDefOfReconAndDiscovery.Luciferium))
				{
					pawn = pawn2;
					return true;
				}
			}
			return false;
		}

		private bool GetHasGoodStoryConditions(Map map)
		{
			bool result;
			if (map == null)
			{
				result = false;
			}
			else
			{
				Pawn pawn;
				if (this.CanFindLuciferiumAddict(map, out pawn))
				{
					Pawn pawn2;
					if (this.CanFindVisitor(map, out pawn2))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		private Site MakeSite()
		{
			int tile;
			Site result;
			if (!TileFinder.TryFindNewSiteTile(ref tile))
			{
				result = null;
			}
			else
			{
				Site site = (Site)WorldObjectMaker.MakeWorldObject(SiteDefOfReconAndDiscovery.Adventure);
				site.Tile = tile;
				site.SetFaction(Faction.OfInsects);
				site.core = SiteDefOfReconAndDiscovery.SeraphitesQuest;
				site.parts.Add(SiteDefOfReconAndDiscovery.SitePart_Computer);
				foreach (SitePartDef sitePartDef in site.parts)
				{
					if (sitePartDef.Worker is SitePartWorker_Computer)
					{
						(sitePartDef.Worker as SitePartWorker_Computer).action = ActionDefOfReconAndDiscovery.ActionSeraphites;
					}
				}
				if (Rand.Value < 0.15f)
				{
					site.parts.Add(SiteDefOfReconAndDiscovery.ScatteredManhunters);
				}
				if (Rand.Value < 0.3f)
				{
					site.parts.Add(SiteDefOfReconAndDiscovery.ScatteredTreasure);
				}
				if (Rand.Value < 0.1f)
				{
					site.parts.Add(SiteDefOfReconAndDiscovery.EnemyRaidOnArrival);
				}
				if (Rand.Value < 0.1f)
				{
					site.parts.Add(SiteDefOfReconAndDiscovery.MechanoidForces);
				}
				Find.WorldObjects.Add(site);
				result = site;
			}
			return result;
		}

		public override bool TryExecute(IncidentParms parms)
		{
			Map map = parms.target as Map;
			bool result;
			if (this.GetHasGoodStoryConditions(map))
			{
				Pawn pawn;
				Pawn pawn2;
				if (!this.CanFindVisitor(map, out pawn))
				{
					result = false;
				}
				else if (!this.CanFindLuciferiumAddict(map, out pawn2))
				{
					result = false;
				}
				else
				{
					Site site = this.MakeSite();
					if (site == null)
					{
						result = false;
					}
					else
					{
						base.SendStandardLetter(site, new string[]
						{
							pawn.NameStringShort,
							pawn2.NameStringShort
						});
						result = true;
					}
				}
			}
			else
			{
				result = false;
			}
			return result;
		}
	}
}
