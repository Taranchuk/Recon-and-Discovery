using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace ReconAndDiscovery.Missions
{
	public class IncidentWorker_WarIdol : IncidentWorker
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

		private bool CanFindPsychic(Map map, out Pawn pawn)
		{
			pawn = null;
			foreach (Pawn pawn2 in map.mapPawns.FreeColonists)
			{
				if (pawn2.story.traits.DegreeOfTrait(TraitDef.Named("PsychicSensitivity")) > 0)
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
				if (this.CanFindPsychic(map, out pawn))
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
			IEnumerable<PowerNet> source = from net in map.powerNetManager.AllNetsListForReading
			where net.hasPowerSource
			select net;
			bool result;
			if (source.Count<PowerNet>() > 0)
			{
				result = false;
			}
			else if (this.GetHasGoodStoryConditions(map))
			{
				Pawn pawn;
				Pawn pawn2;
				if (!this.CanFindVisitor(map, out pawn))
				{
					result = false;
				}
				else if (!this.CanFindPsychic(map, out pawn2))
				{
					result = false;
				}
				else
				{
                    int tile;
					if (TileFinder.TryFindNewSiteTile(out tile))
					{
                        Site site = SiteMaker.MakeSite(SiteDefOfReconAndDiscovery.PsiMachine, tile, Faction.OfInsects);
                        site.parts.Add(SiteDefOfReconAndDiscovery.SitePart_WarIdol);
                        if (Rand.Value < 0.15f)
                        {
                            site.parts.Add(SiteDefOfReconAndDiscovery.ScatteredManhunters);
                        }
                        if (Rand.Value < 0.3f)
                        {
                            site.parts.Add(SiteDefOfReconAndDiscovery.ScatteredTreasure);
                        }
                        if (Rand.Value < 0.3f)
                        {
                            site.parts.Add(SiteDefOfReconAndDiscovery.EnemyRaidOnArrival);
                        }
                        if (Rand.Value < 0.1f)
                        {
                            site.parts.Add(SiteDefOfReconAndDiscovery.MechanoidForces);
                        }
                        Find.WorldObjects.Add(site);
                        if (site == null)
                        {
                            result = false;
                        }
                        base.SendStandardLetter(parms, site, new NamedArgument[]
						{
							pawn.Label,
							pawn2.Label
						});
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
