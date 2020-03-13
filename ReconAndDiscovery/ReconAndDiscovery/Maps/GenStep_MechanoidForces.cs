using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;
using Verse.AI.Group;

namespace ReconAndDiscovery.Maps
{
	public class GenStep_MechanoidForces : GenStep
	{
		public GenStep_MechanoidForces()
		{
		}

		public virtual void Generate(Map map)
		{
			IntVec3 root;
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map) && !x.Fogged(map) && x.GetRoom(map, RegionType.Set_Passable).CellCount >= 4, map, out root))
			{
				float num = this.pointsRange.RandomInRange;
				List<Pawn> list = new List<Pawn>();
				for (int i = 0; i < 50; i++)
				{
					PawnKindDef pawnKindDef = (from kind in DefDatabase<PawnKindDef>.AllDefsListForReading
					where kind.RaceProps.IsMechanoid
					select kind).RandomElementByWeight((PawnKindDef kind) => 1f / kind.combatPower);
					list.Add(PawnGenerator.GeneratePawn(pawnKindDef, Faction.OfMechanoids));
					num -= pawnKindDef.combatPower;
					if (num <= 0f)
					{
						break;
					}
				}
				IntVec3 point = default(IntVec3);
				for (int j = 0; j < list.Count; j++)
				{
					IntVec3 intVec = CellFinder.RandomSpawnCellForPawnNear(root, map, 10);
					point = intVec;
					GenSpawn.Spawn(list[j], intVec, map, Rot4.Random, false);
				}
				LordMaker.MakeNewLord(Faction.OfMechanoids, new LordJob_DefendPoint(point), map, list);
			}
		}

		[CompilerGenerated]
		private static bool <Generate>m__0(PawnKindDef kind)
		{
			return kind.RaceProps.IsMechanoid;
		}

		[CompilerGenerated]
		private static float <Generate>m__1(PawnKindDef kind)
		{
			return 1f / kind.combatPower;
		}

		public FloatRange pointsRange = new FloatRange(450f, 700f);

		[CompilerGenerated]
		private static Func<PawnKindDef, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<PawnKindDef, float> <>f__am$cache1;

		[CompilerGenerated]
		private sealed class <Generate>c__AnonStorey0
		{
			public <Generate>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return x.Standable(this.map) && !x.Fogged(this.map) && x.GetRoom(this.map, RegionType.Set_Passable).CellCount >= 4;
			}

			internal Map map;
		}
	}
}
