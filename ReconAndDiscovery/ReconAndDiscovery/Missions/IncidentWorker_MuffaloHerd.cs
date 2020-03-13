using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace ReconAndDiscovery.Missions
{
	public class IncidentWorker_MuffaloHerd : IncidentWorker
	{
		public IncidentWorker_MuffaloHerd()
		{
		}

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			int num;
			return base.CanFireNowSub(target) && TileFinder.TryFindNewSiteTile(ref num);
		}

		public virtual bool TryExecute(IncidentParms parms)
		{
			Map map = parms.target as Map;
			int num = IncidentWorker_MuffaloHerd.TimeoutDaysRange.RandomInRange;
			int num2 = -1;
			Site site;
			string text;
			if (map != null)
			{
				IEnumerable<PowerNet> source = from net in map.powerNetManager.AllNetsListForReading
				where net.hasPowerSource
				select net;
				if (source.Count<PowerNet>() > 0)
				{
					return false;
				}
				if (!TileFinder.TryFindNewSiteTile(ref num2))
				{
					return false;
				}
				site = (Site)WorldObjectMaker.MakeWorldObject(SiteDefOfReconAndDiscovery.Adventure);
				text = "A large muffalo migration is due to pass near here in {0} days!";
			}
			else
			{
				List<Caravan> list = (from c in Find.World.worldObjects.Caravans
				where c.Faction == Faction.OfPlayer
				select c).ToList<Caravan>();
				if (list.Count == 0)
				{
					return false;
				}
				Caravan caravan = list.RandomElement<Caravan>();
				num -= 3;
				TileFinder.TryFindPassableTileWithTraversalDistance(caravan.Tile, 1, 2, ref num2, (int t) => !Find.WorldObjects.AnyMapParentAt(t), false);
				if (num2 == 0 || num2 == -1)
				{
					return false;
				}
				site = (Site)WorldObjectMaker.MakeWorldObject(SiteDefOfReconAndDiscovery.Adventure);
				text = "Your caravan has spotted a huge muffalo migration!";
			}
			bool result;
			if (num2 == 0 || num2 == -1)
			{
				result = false;
			}
			else if (site == null)
			{
				result = false;
			}
			else
			{
				site.Tile = num2;
				site.SetFaction(Find.FactionManager.FirstFactionOfDef(FactionDefOf.Insect));
				site.core = SiteDefOfReconAndDiscovery.MuffaloMigration;
				if (Rand.Value < 0.5f)
				{
					site.parts.Add(SiteDefOfReconAndDiscovery.ScatteredTreasure);
				}
				Find.LetterStack.ReceiveLetter("Muffalo migration", text, LetterDefOf.Good, site, null);
				site.GetComponent<TimeoutComp>().StartTimeout(num * 60000);
				Find.WorldObjects.Add(site);
				result = true;
			}
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static IncidentWorker_MuffaloHerd()
		{
		}

		[CompilerGenerated]
		private static bool <TryExecute>m__0(PowerNet net)
		{
			return net.hasPowerSource;
		}

		[CompilerGenerated]
		private static bool <TryExecute>m__1(Caravan c)
		{
			return c.Faction == Faction.OfPlayer;
		}

		[CompilerGenerated]
		private static bool <TryExecute>m__2(int t)
		{
			return !Find.WorldObjects.AnyMapParentAt(t);
		}

		private static readonly IntRange TimeoutDaysRange = new IntRange(7, 12);

		[CompilerGenerated]
		private static Func<PowerNet, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Caravan, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Predicate<int> <>f__am$cache2;
	}
}
