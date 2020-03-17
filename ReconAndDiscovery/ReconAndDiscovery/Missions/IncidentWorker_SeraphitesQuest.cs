﻿using System;
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
			where p.RaceProps.Humanlike && p.Faction != Faction.OfPlayer
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

		protected override bool TryExecuteWorker(IncidentParms parms)
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
                    int tile;
                    if (TileFinder.TryFindNewSiteTile(out tile))
                    {
                        Site site = (Site)WorldObjectMaker.MakeWorldObject(SiteDefOfReconAndDiscovery.Adventure);
                        Faction faction = Faction.OfInsects;
                        site.AddPart(new SitePart(site, SiteDefOfReconAndDiscovery.SeraphitesQuest,
SiteDefOfReconAndDiscovery.SeraphitesQuest.Worker.GenerateDefaultParams(StorytellerUtility.DefaultSiteThreatPointsNow(), tile, faction)));
                        SitePart sitePart_Computer = new SitePart(site, SiteDefOfReconAndDiscovery.SitePart_Computer, SiteDefOfReconAndDiscovery.SitePart_Computer.Worker.GenerateDefaultParams(StorytellerUtility.DefaultSiteThreatPointsNow(), tile, faction));
                        site.parts.Add(sitePart_Computer);
                        foreach (SitePartDef sitePartDef in site.parts.Select(x => x.def))
                        {
                            if (sitePartDef.Worker is SitePartWorker_Computer)
                            {
                                (sitePartDef.Worker as SitePartWorker_Computer).action = ActionDefOfReconAndDiscovery.ActionSeraphites;
                            }
                        }
                        if (Rand.Value < 0.15f)
                        {
                            SitePart scatteredManhunters = new SitePart(site, SiteDefOfReconAndDiscovery.ScatteredManhunters, SiteDefOfReconAndDiscovery.ScatteredManhunters.Worker.GenerateDefaultParams(StorytellerUtility.DefaultSiteThreatPointsNow(), tile, faction));
                            site.parts.Add(scatteredManhunters);
                        }
                        if (Rand.Value < 0.3f)
                        {
                            SitePart scatteredTreasure = new SitePart(site, SiteDefOfReconAndDiscovery.ScatteredTreasure, SiteDefOfReconAndDiscovery.ScatteredTreasure.Worker.GenerateDefaultParams(StorytellerUtility.DefaultSiteThreatPointsNow(), tile, faction));
                            site.parts.Add(scatteredTreasure);
                        }
                        if (Rand.Value < 0.1f)
                        {
                            SitePart enemyRaidOnArrival = new SitePart(site, SiteDefOfReconAndDiscovery.EnemyRaidOnArrival, SiteDefOfReconAndDiscovery.EnemyRaidOnArrival.Worker.GenerateDefaultParams(StorytellerUtility.DefaultSiteThreatPointsNow(), tile, faction));
                            site.parts.Add(enemyRaidOnArrival);
                        }
                        if (Rand.Value < 0.1f)
                        {
                            SitePart mechanoidForces = new SitePart(site, SiteDefOfReconAndDiscovery.MechanoidForces, SiteDefOfReconAndDiscovery.MechanoidForces.Worker.GenerateDefaultParams(StorytellerUtility.DefaultSiteThreatPointsNow(), tile, faction));
                            site.parts.Add(mechanoidForces);
                        }
                        base.SendStandardLetter(parms, site, new NamedArgument[]
                        {
                            pawn.Label,
                            pawn2.Label
                        });
                        Find.WorldObjects.Add(site);
                        result = true;
                    }
					else
					{
						result = false;
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







