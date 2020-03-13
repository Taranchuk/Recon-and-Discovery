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
		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			int num;
			return base.CanFireNowSub(target) && TileFinder.TryFindNewSiteTile(ref num);
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
				site.core = SiteDefOfReconAndDiscovery.PsiMachine;
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
				result = site;
			}
			return result;
		}

		public override bool TryExecute(IncidentParms parms)
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
