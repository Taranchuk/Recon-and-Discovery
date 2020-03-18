﻿using System;
using RimWorld;
using Verse;

namespace ReconAndDiscovery
{
	public class JobDriver_MandatoryMilk : JobDriver_GatherAnimalBodyResources
	{
		protected override float WorkTotal
		{
			get
			{
				return 800f;
			}
		}

		protected override CompHasGatherableBodyResource GetComp(Pawn animal)
		{
			return animal.TryGetComp<CompMandatoryMilkable>();
		}
	}
}










