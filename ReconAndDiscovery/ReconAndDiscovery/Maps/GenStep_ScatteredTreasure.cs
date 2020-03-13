using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace ReconAndDiscovery.Maps
{
	public class GenStep_ScatteredTreasure : GenStep
	{
		public override void Generate(Map map)
		{
			float num = Mathf.Min(18000f, Mathf.Max(new float[]
			{
				Mathf.Exp(Rand.Gaussian(8f, 0.65f))
			}));
			ItemCollectionGenerator_Rewards itemCollectionGenerator_Rewards = new ItemCollectionGenerator_Rewards();
			ItemCollectionGeneratorParams itemCollectionGeneratorParams = default(ItemCollectionGeneratorParams);
			itemCollectionGeneratorParams.techLevel = TechLevel.Spacer;
			itemCollectionGeneratorParams.totalMarketValue = num;
			itemCollectionGeneratorParams.count = Rand.RangeInclusive(2, 10);
			if (num > 10000f)
			{
				itemCollectionGeneratorParams.count = 1;
			}
			itemCollectionGeneratorParams.validator = ((ThingDef t) => t.defName != "Silver");
			List<Thing> list = itemCollectionGenerator_Rewards.Generate(itemCollectionGeneratorParams);
			foreach (Thing thing in list)
			{
				if (thing.stackCount > thing.def.stackLimit)
				{
					thing.stackCount = thing.def.stackLimit;
				}
				IntVec3 intVec;
				if (CellFinderLoose.TryGetRandomCellWith((IntVec3 x) => x.Standable(map) && x.Fogged(map) && GridsUtility.GetRoom(x, map, 6).CellCount >= 2, map, 1000, out intVec))
				{
					GenSpawn.Spawn(thing, intVec, map, Rot4.Random, false);
				}
			}
		}
	}
}
