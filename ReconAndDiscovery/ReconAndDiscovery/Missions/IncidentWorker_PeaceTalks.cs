﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace ReconAndDiscovery.Missions
{
	public class IncidentWorker_PeaceTalks : IncidentWorker
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
			Site site = (Site)WorldObjectMaker.MakeWorldObject(SiteDefOfReconAndDiscovery.AdventurePeaceTalks);
			site.Tile = tile;
			site.core = SiteDefOfReconAndDiscovery.PeaceTalks;
			Find.WorldObjects.Add(site);
			return site;
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

		public override bool TryExecute(IncidentParms parms)
		{
			bool result;
			Faction faction;
			if ((from wo in Find.WorldObjects.AllWorldObjects
			where wo.def == SiteDefOfReconAndDiscovery.AdventurePeaceTalks
			select wo).Count<WorldObject>() > 0)
			{
				result = false;
			}
			else if (!this.TryFindFaction(out faction, (Faction f) => f != Faction.OfPlayer && f.PlayerGoodwill < 0f && f.def.CanEverBeNonHostile && f.def.humanlikeFaction))
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
					site.parts.Add(SiteDefOfReconAndDiscovery.PeaceTalksFaction);
					site.SetFaction(faction);
					site.GetComponent<QuestComp_PeaceTalks>().StartQuest(faction);
					int num = 5;
					site.GetComponent<TimeoutComp>().StartTimeout(num * 60000);
					base.SendStandardLetter(site, new string[]
					{
						faction.leader.NameStringShort,
						faction.Name,
						num.ToString()
					});
					result = true;
				}
			}
			return result;
		}
	}
}
