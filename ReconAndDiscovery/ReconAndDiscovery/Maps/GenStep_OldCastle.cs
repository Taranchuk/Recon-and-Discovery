﻿using System;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.BaseGen;
using Verse;

namespace ReconAndDiscovery.Maps
{
	public class GenStep_OldCastle : GenStep_AdventureGenerator
	{
		public GenStep_OldCastle()
		{
		}

		public override void Generate(Map map)
		{
			if (!map.TileInfo.WaterCovered)
			{
				base.Generate(map);
				int num = Rand.RangeInclusive(55, 80);
				int num2 = Rand.RangeInclusive(55, 80);
				CellRect rect = new CellRect(Rand.RangeInclusive(this.adventureRegion.minX, this.adventureRegion.maxX - num), Rand.RangeInclusive(this.adventureRegion.minZ, this.adventureRegion.maxZ - num2), num, num2);
				ResolveParams baseResolveParams = this.baseResolveParams;
				baseResolveParams.rect = rect;
				BaseGen.globalSettings.map = map;
				BaseGen.symbolStack.Push("oldCastle", baseResolveParams);
				BaseGen.Generate();
				MapGenUtility.MakeDoors(new ResolveParams
				{
					wallStuff = ThingDefOf.WoodLog
				}, map);
				BaseGen.Generate();
				MapGenUtility.ScatterWeaponsWhere(baseResolveParams.rect, Rand.RangeInclusive(7, 11), map, (ThingDef thing) => thing.weaponTags != null && thing.equipmentType == EquipmentType.Primary && !thing.destroyOnDrop && !thing.weaponTags.Contains("Gun"));
				MapGenUtility.ResolveCustomGenSteps(map);
			}
		}

		[CompilerGenerated]
		private static bool <Generate>m__0(ThingDef thing)
		{
			return thing.weaponTags != null && thing.equipmentType == EquipmentType.Primary && !thing.destroyOnDrop && !thing.weaponTags.Contains("Gun");
		}

		[CompilerGenerated]
		private static Predicate<ThingDef> <>f__am$cache0;
	}
}
