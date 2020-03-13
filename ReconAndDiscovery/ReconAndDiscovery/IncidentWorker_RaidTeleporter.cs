using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;
using Verse.AI.Group;

namespace ReconAndDiscovery
{
	public class IncidentWorker_RaidTeleporter : IncidentWorker_RaidEnemy
	{
		public IncidentWorker_RaidTeleporter()
		{
		}

		public virtual bool TryExecute(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IEnumerable<Pawn> source = from p in map.mapPawns.AllPawnsSpawned
			where p.Faction == Faction.OfMechanoids && !p.Downed
			select p;
			bool result;
			if (source.Count<Pawn>() == 0)
			{
				result = false;
			}
			else
			{
				IEnumerable<Building> enumerable = from b in map.listerBuildings.AllBuildingsColonistOfDef(ThingDef.Named("Teleporter"))
				where b.GetComp<CompPowerTrader>().PowerOn && b.GetComp<CompTeleporter>().ReadyToTransport
				select b;
				if (enumerable.Count<Building>() == 0)
				{
					result = false;
				}
				else
				{
					List<Pawn> list = new List<Pawn>();
					Pawn p2 = source.RandomElement<Pawn>();
					foreach (Building building in enumerable)
					{
						Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDef.Named("Scyther"), Faction.OfMechanoids);
						GenSpawn.Spawn(pawn, building.Position, building.Map);
						building.GetComp<CompTeleporter>().ResetCharge();
						list.Add(pawn);
						p2.GetLord().AddPawn(pawn);
					}
					base.SendStandardLetter(list.FirstOrDefault<Pawn>(), new string[0]);
					Find.TickManager.slower.SignalForceNormalSpeedShort();
					Find.StoryWatcher.statsRecord.numRaidsEnemy++;
					result = true;
				}
			}
			return result;
		}

		[CompilerGenerated]
		private static bool <TryExecute>m__0(Pawn p)
		{
			return p.Faction == Faction.OfMechanoids && !p.Downed;
		}

		[CompilerGenerated]
		private static bool <TryExecute>m__1(Building b)
		{
			return b.GetComp<CompPowerTrader>().PowerOn && b.GetComp<CompTeleporter>().ReadyToTransport;
		}

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Building, bool> <>f__am$cache1;
	}
}
