using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;

namespace ReconAndDiscovery
{
	public class IncidentWorker_MalevolentAI : IncidentWorker
	{
		public IncidentWorker_MalevolentAI()
		{
		}

		public virtual bool TryExecute(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			bool result;
			if (map == null)
			{
				result = false;
			}
			else
			{
				IEnumerable<Pawn> source = from p in map.mapPawns.AllPawnsSpawned
				where p.Faction.HostileTo(Faction.OfPlayer) && GenHostility.IsActiveThreat(p)
				select p;
				if (source.Count<Pawn>() == 0)
				{
					result = false;
				}
				else
				{
					Pawn pawn = source.RandomElement<Pawn>();
					List<Building> list = (from b in map.listerBuildings.allBuildingsColonist
					where b.def.building.ai_combatDangerous && b.GetComp<CompPowerTrader>() != null && b.GetComp<CompPowerTrader>().PowerOn
					select b).ToList<Building>();
					if (list.Count<Building>() == 0)
					{
						result = false;
					}
					else
					{
						foreach (Building building in list)
						{
							building.SetFaction(pawn.Faction, null);
						}
						base.SendStandardLetter(list.FirstOrDefault<Building>(), new string[]
						{
							pawn.Faction.Name
						});
						result = true;
					}
				}
			}
			return result;
		}

		[CompilerGenerated]
		private static bool <TryExecute>m__0(Pawn p)
		{
			return p.Faction.HostileTo(Faction.OfPlayer) && GenHostility.IsActiveThreat(p);
		}

		[CompilerGenerated]
		private static bool <TryExecute>m__1(Building b)
		{
			return b.def.building.ai_combatDangerous && b.GetComp<CompPowerTrader>() != null && b.GetComp<CompPowerTrader>().PowerOn;
		}

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Building, bool> <>f__am$cache1;
	}
}
