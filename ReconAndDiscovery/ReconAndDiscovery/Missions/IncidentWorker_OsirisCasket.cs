using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace ReconAndDiscovery.Missions
{
	public class IncidentWorker_OsirisCasket : IncidentWorker
	{
		public IncidentWorker_OsirisCasket()
		{
		}

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			int num;
			return base.CanFireNowSub(target) && TileFinder.TryFindNewSiteTile(ref num);
		}

		private bool CanFindPsychic(Map map, out Pawn pawn)
		{
			pawn = null;
			IEnumerable<Pawn> source = from p in map.mapPawns.FreeColonistsSpawned
			where p.RaceProps.Humanlike && p.story.traits.DegreeOfTrait(TraitDef.Named("PsychicSensitivity")) > 0
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

		private bool GetHasGoodStoryConditions(Map map)
		{
			Pawn pawn;
			return map != null && this.CanFindPsychic(map, out pawn);
		}

		private Site MakeSite(int days, Map map)
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
				site.core = SiteDefOfReconAndDiscovery.AbandonedCastle;
				IEnumerable<PowerNet> source = from net in map.powerNetManager.AllNetsListForReading
				where net.hasPowerSource
				select net;
				if (source.Count<PowerNet>() > 0)
				{
					site.parts.Add(SiteDefOfReconAndDiscovery.OsirisCasket);
				}
				else
				{
					site.parts.Add(SiteDefOfReconAndDiscovery.WeatherSat);
				}
				site.GetComponent<TimeoutComp>().StartTimeout(days * 60000);
				if (Rand.Value < 0.25f)
				{
					site.parts.Add(SiteDefOfReconAndDiscovery.ScatteredManhunters);
				}
				if (Rand.Value < 0.1f)
				{
					site.parts.Add(SiteDefOfReconAndDiscovery.ScatteredTreasure);
				}
				if (Rand.Value < 1f)
				{
					site.parts.Add(SiteDefOfReconAndDiscovery.EnemyRaidOnArrival);
				}
				if (Rand.Value < 0.9f)
				{
					site.parts.Add(SiteDefOfReconAndDiscovery.EnemyRaidOnArrival);
				}
				if (Rand.Value < 0.6f)
				{
					site.parts.Add(SiteDefOfReconAndDiscovery.EnemyRaidOnArrival);
				}
				Find.WorldObjects.Add(site);
				result = site;
			}
			return result;
		}

		public virtual bool TryExecute(IncidentParms parms)
		{
			Map map = parms.target as Map;
			bool result;
			if (this.GetHasGoodStoryConditions(map))
			{
				Pawn pawn;
				if (!this.CanFindPsychic(map, out pawn))
				{
					result = false;
				}
				else
				{
					int randomInRange = IncidentWorker_OsirisCasket.TimeoutDaysRange.RandomInRange;
					if (this.MakeSite(randomInRange, map) == null)
					{
						result = false;
					}
					else
					{
						QueuedIncident qi = new QueuedIncident(new FiringIncident(IncidentDef.Named("PsychicDrone"), null, parms), Find.TickManager.TicksGame + 1);
						Find.Storyteller.incidentQueue.Add(qi);
						Find.LetterStack.ReceiveLetter("Psychic message", string.Format("{0} has received visions accompanying the drone, showing a battle and crying out for help. Others must have noticed, so the site will probably be dangerous.", pawn.NameStringShort), LetterDefOf.Good, null);
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

		// Note: this type is marked as 'beforefieldinit'.
		static IncidentWorker_OsirisCasket()
		{
		}

		[CompilerGenerated]
		private static bool <CanFindPsychic>m__0(Pawn p)
		{
			return p.RaceProps.Humanlike && p.story.traits.DegreeOfTrait(TraitDef.Named("PsychicSensitivity")) > 0;
		}

		[CompilerGenerated]
		private static bool <MakeSite>m__1(PowerNet net)
		{
			return net.hasPowerSource;
		}

		private static readonly IntRange TimeoutDaysRange = new IntRange(15, 25);

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<PowerNet, bool> <>f__am$cache1;
	}
}
