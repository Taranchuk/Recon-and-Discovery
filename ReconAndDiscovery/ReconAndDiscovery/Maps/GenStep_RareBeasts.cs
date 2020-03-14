using System;
using RimWorld;
using Verse;

namespace ReconAndDiscovery.Maps
{
	public class GenStep_RareBeasts : GenStep
	{
        public override int SeedPart
        {
            get
            {
                return 349231510;
            }
        }
        public override void Generate(Map map, GenStepParams parms)
		{
			PawnKindDef pawnKindDef = ThingDefOfReconAndDiscovery.Devillo;
			if (Rand.Chance(0.4f))
			{
				pawnKindDef = ThingDefOfReconAndDiscovery.Nitralope;
			}

            //public PawnGenerationRequest(PawnKindDef kind, Faction faction = null,
            //    PawnGenerationContext context = PawnGenerationContext.NonPlayer, int tile = -1, 
            //    bool forceGenerateNewPawn = false, bool newborn = false, bool allowDead = false,
            //    bool allowDowned = false, bool canGeneratePawnRelations = true, bool mustBeCapableOfViolence = false,
            //
            //
            //    float colonistRelationChanceFactor = 1, bool forceAddFreeWarmLayerIfNeeded = false,
            //    bool allowGay = true, 
            //    
            //    bool allowFood = true, bool allowAddictions = true, bool inhabitant = false,
            //
            //    bool certainlyBeenInCryptosleep = false, bool forceRedressWorldPawnIfFormerColonist = false,
            //    bool worldPawnFactionDoesntMatter = false, float biocodeWeaponChance = 0,
            //    Pawn extraPawnForExtraRelationChance = null, float relationWithExtraPawnChanceFactor = 1,
            //    Predicate<Pawn> validatorPreGear = null, Predicate<Pawn> validatorPostGear = null,
            //    IEnumerable<TraitDef> forcedTraits = null, IEnumerable<TraitDef> prohibitedTraits = null,
            //    float? minChanceToRedressWorldPawn = null, float? fixedBiologicalAge = null,
            //    float? fixedChronologicalAge = null, Gender? fixedGender = null, float? fixedMelanin = null,
            //    string fixedLastName = null, string fixedBirthName = null, RoyalTitleDef fixedTitle = null);

            PawnGenerationRequest request = new PawnGenerationRequest(pawnKindDef, null, 
                PawnGenerationContext.NonPlayer, -1, true, true, false, false, false, false, 
                1f, false, false,
                false, true, true, true, 
                false, false, 0f, null, 1f, null, null, null,
                null, new float?(0f), new float?(1f), new float?(1f), new Gender?(Gender.Male), null, null);

			PawnGenerationRequest request2 = new PawnGenerationRequest(pawnKindDef, null, 
                
                PawnGenerationContext.NonPlayer, -1, true, true, false, false, false, false, 1f, false, false, 
                false, false, false, false, false, false, 0f, null, 1f, null, null, null, 
                null, new float? (0f), new float?(0f), new float?(1f), new Gender?(Gender.Female), null, null);

			IntVec3 intVec;
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map) && x.Fogged(map) && GridsUtility.GetRoom(x, map, RegionType.Set_Passable).CellCount >= 4, map, out intVec))
			{
				Pawn pawn = PawnGenerator.GeneratePawn(request);
				Pawn pawn2 = PawnGenerator.GeneratePawn(request2);
				IntVec3 intVec2 = CellFinder.RandomSpawnCellForPawnNear(intVec, map, 10);
				GenSpawn.Spawn(pawn, intVec2, map, Rot4.Random, WipeMode.Vanish, false);
				intVec2 = CellFinder.RandomSpawnCellForPawnNear(intVec, map, 10);
				GenSpawn.Spawn(pawn2, intVec2, map, Rot4.Random, WipeMode.Vanish, false);
			}
		}
	}
}
