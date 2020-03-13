using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ReconAndDiscovery.Missions;
using RimWorld;
using Verse;
using Verse.AI.Group;

namespace ReconAndDiscovery.Maps
{
	public class SitePartWorker_MedicalEmergency : SitePartWorker
	{
		public SitePartWorker_MedicalEmergency()
		{
		}

		public override void PostMapGenerate(Map map)
		{
			base.PostMapGenerate(map);
			Faction faction = Find.FactionManager.FirstFactionOfDef(FactionDefOf.Spacer);
			int maxPawns = Find.World.worldObjects.MapParentAt(map.Tile).GetComponent<QuestComp_MedicalEmergency>().maxPawns;
			List<Pawn> list = new List<Pawn>();
			IntVec3 baseCenter;
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map) && x.Fogged(map) && x.GetRoom(map, RegionType.Set_Passable).CellCount >= 2, map, out baseCenter))
			{
				for (int i = 0; i < maxPawns; i++)
				{
					IntVec3 root;
					if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map) && x.Fogged(map) && x.GetRoom(map, RegionType.Set_Passable).CellCount >= 2, map, out root))
					{
						PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, faction, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 20f, false, true, true, false, false, null, null, null, null, null, null);
						Pawn pawn = PawnGenerator.GeneratePawn(request);
						list.Add(pawn);
						HealthUtility.DamageUntilDowned(pawn);
						IntVec3 intVec = CellFinder.RandomSpawnCellForPawnNear(root, map, 18);
						GenSpawn.Spawn(pawn, intVec, map, Rot4.Random, false);
					}
				}
				LordJob_DefendBase lordJob = new LordJob_DefendBase(faction, baseCenter);
				LordMaker.MakeNewLord(faction, lordJob, map, list);
			}
		}

		public FloatRange casualtiesRange = new FloatRange(400f, 1000f);

		[CompilerGenerated]
		private sealed class <PostMapGenerate>c__AnonStorey0
		{
			public <PostMapGenerate>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return x.Standable(this.map) && x.Fogged(this.map) && x.GetRoom(this.map, RegionType.Set_Passable).CellCount >= 2;
			}

			internal bool <>m__1(IntVec3 x)
			{
				return x.Standable(this.map) && x.Fogged(this.map) && x.GetRoom(this.map, RegionType.Set_Passable).CellCount >= 2;
			}

			internal Map map;
		}
	}
}
