using System;
using RimWorld;
using Verse;

namespace ReconAndDiscovery.Maps
{
	public class GenStep_RareBeasts : GenStep
	{
		public override void Generate(Map map)
		{
			PawnKindDef pawnKindDef = ThingDefOfReconAndDiscovery.Devillo;
			if (Rand.Chance(0.4f))
			{
				pawnKindDef = ThingDefOfReconAndDiscovery.Nitralope;
			}
			PawnGenerationRequest request = new PawnGenerationRequest(pawnKindDef, null, PawnGenerationContext.NonPlayer, -1, true, true, false, false, false, false, 1f, false, false, false, false, false, null, new float?(0f), new float?(1f), new Gender?(Gender.Male), null, null);
			PawnGenerationRequest request2 = new PawnGenerationRequest(pawnKindDef, null, PawnGenerationContext.NonPlayer, -1, true, true, false, false, false, false, 1f, false, false, false, false, false, null, new float?(0f), new float?(1f), new Gender?(Gender.Female), null, null);
			IntVec3 intVec;
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map) && x.Fogged(map) && GridsUtility.GetRoom(x, map, 6).CellCount >= 4, map, out intVec))
			{
				Pawn pawn = PawnGenerator.GeneratePawn(request);
				Pawn pawn2 = PawnGenerator.GeneratePawn(request2);
				IntVec3 intVec2 = CellFinder.RandomSpawnCellForPawnNear(intVec, map, 10);
				GenSpawn.Spawn(pawn, intVec2, map, Rot4.Random, false);
				intVec2 = CellFinder.RandomSpawnCellForPawnNear(intVec, map, 10);
				GenSpawn.Spawn(pawn2, intVec2, map, Rot4.Random, false);
			}
		}
	}
}
