using System;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;

namespace ReconAndDiscovery
{
	public class GameCondition_Tremors : GameCondition
	{
		public GameCondition_Tremors()
		{
		}

		public override void End()
		{
			base.End();
		}

		public override void ExposeData()
		{
			base.ExposeData();
		}

		private void CollapseRandomRoof()
		{
			IntVec3 intVec;
			if (CellFinderLoose.TryGetRandomCellWith((IntVec3 c) => c.Standable(base.Map) && base.Map.roofGrid.Roofed(c), base.Map, 500, out intVec))
			{
				base.Map.roofCollapseBuffer.MarkToCollapse(intVec);
				IntVec3[] array = new IntVec3[]
				{
					intVec + IntVec3.West,
					intVec + IntVec3.East,
					intVec + IntVec3.South,
					intVec + IntVec3.North
				};
				foreach (IntVec3 c2 in array)
				{
					if (c2.Standable(base.Map) && base.Map.roofGrid.Roofed(c2))
					{
						base.Map.roofCollapseBuffer.MarkToCollapse(c2);
					}
				}
			}
		}

		public override void GameConditionTick()
		{
			if (Rand.Chance(8.333333E-05f))
			{
				this.CollapseRandomRoof();
			}
		}

		public override void Init()
		{
			this.CollapseRandomRoof();
			base.Init();
		}

		[CompilerGenerated]
		private bool <CollapseRandomRoof>m__0(IntVec3 c)
		{
			return c.Standable(base.Map) && base.Map.roofGrid.Roofed(c);
		}
	}
}
