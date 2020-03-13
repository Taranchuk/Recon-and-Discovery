using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;

namespace ReconAndDiscovery.Maps
{
	public class GenStep_ScatteredManhunters : GenStep
	{
		public GenStep_ScatteredManhunters()
		{
		}

		public virtual void Generate(Map map)
		{
			float num = this.pointsRange.RandomInRange;
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < 50; i++)
			{
				PawnKindDef pawnKindDef;
				if (!ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(this.pointsRange.RandomInRange, map.Tile, out pawnKindDef))
				{
					return;
				}
				list.Add(PawnGenerator.GeneratePawn(pawnKindDef, null));
				num -= pawnKindDef.combatPower;
				if (num <= 0f)
				{
					break;
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				IntVec3 root;
				if (CellFinderLoose.TryGetRandomCellWith((IntVec3 x) => x.Standable(map) && x.Fogged(map) && x.GetRoom(map, RegionType.Set_Passable).CellCount >= 4, map, 1000, out root))
				{
					IntVec3 intVec = CellFinder.RandomSpawnCellForPawnNear(root, map, 10);
					GenSpawn.Spawn(list[j], intVec, map, Rot4.Random, false);
					list[j].mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, false, false, null);
				}
			}
		}

		public FloatRange pointsRange = new FloatRange(250f, 700f);

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
