using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace ReconAndDiscovery
{
	public class IncidentWorker_RaidStargate : IncidentWorker_RaidEnemy
	{
		public IncidentWorker_RaidStargate()
		{
		}

		public virtual bool TryExecute(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IEnumerable<Building> source = map.listerBuildings.AllBuildingsColonistOfDef(ThingDef.Named("Stargate"));
			bool result;
			if (source.Count<Building>() == 0)
			{
				result = false;
			}
			else
			{
				Building building = source.FirstOrDefault<Building>();
				this.ResolveRaidPoints(parms);
				if (!this.TryResolveRaidFaction(parms))
				{
					result = false;
				}
				else
				{
					this.ResolveRaidStrategy(parms);
					this.ResolveRaidArriveMode(parms);
					if (!this.ResolveRaidSpawnCenter(parms))
					{
						result = false;
					}
					else
					{
						PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(parms);
						List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(PawnGroupKindDefOf.Normal, defaultPawnGroupMakerParms, true).ToList<Pawn>();
						if (list.Count == 0)
						{
							Log.Error("Got no pawns spawning raid from parms " + parms);
							result = false;
						}
						else
						{
							TargetInfo target = TargetInfo.Invalid;
							foreach (Pawn pawn in list)
							{
								IntVec3 position = building.Position;
								GenSpawn.Spawn(pawn, position, map, parms.spawnRotation, false);
								target = pawn;
							}
							Lord lord = LordMaker.MakeNewLord(parms.faction, parms.raidStrategy.Worker.MakeLordJob(parms, map), map, list);
							AvoidGridMaker.RegenerateAvoidGridsFor(parms.faction, map);
							LessonAutoActivator.TeachOpportunity(ConceptDefOf.EquippingWeapons, OpportunityType.Critical);
							if (!PlayerKnowledgeDatabase.IsComplete(ConceptDefOf.ShieldBelts))
							{
								for (int i = 0; i < list.Count; i++)
								{
									Pawn pawn2 = list[i];
									if (pawn2.apparel.WornApparel.Any((Apparel ap) => ap is ShieldBelt))
									{
										LessonAutoActivator.TeachOpportunity(ConceptDefOf.ShieldBelts, OpportunityType.Critical);
										break;
									}
								}
							}
							base.SendStandardLetter(target, new string[]
							{
								parms.faction.Name
							});
							Find.TickManager.slower.SignalForceNormalSpeedShort();
							Find.StoryWatcher.statsRecord.numRaidsEnemy++;
							result = true;
						}
					}
				}
			}
			return result;
		}

		[CompilerGenerated]
		private static bool <TryExecute>m__0(Apparel ap)
		{
			return ap is ShieldBelt;
		}

		[CompilerGenerated]
		private static Predicate<Apparel> <>f__am$cache0;
	}
}
