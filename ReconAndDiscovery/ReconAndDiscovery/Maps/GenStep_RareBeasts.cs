using System;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;

namespace ReconAndDiscovery.Maps
{
	public class GenStep_RareBeasts : GenStep
	{
		public GenStep_RareBeasts()
		{
		}

		public virtual void Generate(Map map)
		{
			PawnKindDef pawnKindDef = ThingDefOfReconAndDiscovery.Devillo;
			if (Rand.Chance(0.4f))
			{
				pawnKindDef = ThingDefOfReconAndDiscovery.Nitralope;
			}
			PawnGenerationRequest request = new PawnGenerationRequest(pawnKindDef, null, PawnGenerationContext.NonPlayer, -1, true, true, false, false, false, false, 1f, false, false, false, false, false, null, new float?(0f), new float?(1f), new Gender?(Gender.Male), null, null);
			PawnGenerationRequest request2 = new PawnGenerationRequest(pawnKindDef, null, PawnGenerationContext.NonPlayer, -1, true, true, false, false, false, false, 1f, false, false, false, false, false, null, new float?(0f), new float?(1f), new Gender?(Gender.Female), null, null);
			IntVec3 root;
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map) && x.Fogged(map) && x.GetRoom(map, RegionType.Set_Passable).CellCount >= 4, map, out root))
			{
				Pawn pawn = PawnGenerator.GeneratePawn(request);
				Pawn pawn2 = PawnGenerator.GeneratePawn(request2);
				IntVec3 intVec = CellFinder.RandomSpawnCellForPawnNear(root, map, 10);
				GenSpawn.Spawn(pawn, intVec, map, Rot4.Random, false);
				intVec = CellFinder.RandomSpawnCellForPawnNear(root, map, 10);
				GenSpawn.Spawn(pawn2, intVec, map, Rot4.Random, false);
			}
		}

		[CompilerGenerated]
		private sealed class <Generate>c__AnonStorey0
		{
			public <Generate>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return x.Standable(this.map) && x.Fogged(this.map) && x.GetRoom(this.map, RegionType.Set_Passable).CellCount >= 4;
			}

			internal Map map;
		}
	}
}
