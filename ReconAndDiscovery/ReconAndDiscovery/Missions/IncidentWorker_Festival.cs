using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace ReconAndDiscovery.Missions
{
	public class IncidentWorker_Festival : IncidentWorker
	{
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			int num;
			return base.CanFireNowSub(parms) && TileFinder.TryFindNewSiteTile(out num);
		}

		private Site MakeSite()
		{
			int tile;
			TileFinder.TryFindNewSiteTile(out tile);
			Site site = (Site)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Site);
			site.Tile = tile;
			site.def = SiteDefOfReconAndDiscovery.Festival;
            // TODO: check if this works correctly
            SitePart outpost = new SitePart(site, SitePartDefOf.Outpost, null);
			site.parts.Add(outpost);
			Find.WorldObjects.Add(site);
			return site;
		}

		public List<Faction> GetAllNonPlayerFriends(Faction faction)
		{
			List<Faction> list = Find.FactionManager.AllFactionsVisible.ToList<Faction>();
			if (list.Contains(Faction.OfPlayer))
			{
				list.Remove(Faction.OfPlayer);
			}
			return (from f in list
			where faction.GoodwillWith(f) > 10f
			select f).ToList<Faction>();
		}

		public int FriendsCount(Faction faction)
		{
			List<Faction> list = Find.FactionManager.AllFactionsVisible.ToList<Faction>();
			if (list.Contains(faction))
			{
				list.Remove(faction);
			}
			list = (from f in list
			where faction.GoodwillWith(f) > 10f
			select f).ToList<Faction>();
			return list.Count<Faction>();
		}

		public bool TryFindFaction(out Faction faction, Predicate<Faction> validator)
		{
			faction = null;
			List<Faction> list = Find.FactionManager.AllFactionsVisible.ToList<Faction>();
			if (list.Contains(Faction.OfPlayer))
			{
				list.Remove(Faction.OfPlayer);
			}
			list = (from f in list
			where validator(f)
			select f).ToList<Faction>();
			bool result;
			if (list.Count<Faction>() > 0)
			{
				faction = list.RandomElement<Faction>();
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			bool result;
			Faction faction;
			if ((from wo in Find.WorldObjects.AllWorldObjects
			where wo.def == SiteDefOfReconAndDiscovery.AdventurePeaceTalks
			select wo).Count<WorldObject>() > 0)
			{
				result = false;
			}
			else if (!this.TryFindFaction(out faction, (Faction f) => f != Faction.OfPlayer && f.PlayerGoodwill > 10f && this.FriendsCount(f) >= 2 && f.def.humanlikeFaction))
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
					site.SetFaction(faction);
					int num = 8;
					site.GetComponent<TimeoutComp>().StartTimeout(num * 60000);
					base.SendStandardLetter(parms, site, , new NamedArgument[]
                    {
						faction.Name
					});
					result = true;
				}
			}
			return result;
		}
	}
}
