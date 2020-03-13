using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace ReconAndDiscovery.Maps
{
	public class SitePartWorker_RadioactiveDust : SitePartWorker
	{
		public SitePartWorker_RadioactiveDust()
		{
		}

		public override void PostMapGenerate(Map map)
		{
			base.PostMapGenerate(map);
			List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.FoodSource).ToList<Thing>();
			foreach (Thing thing in list)
			{
				thing.Destroy(DestroyMode.Vanish);
			}
			GameCondition cond = GameConditionMaker.MakeCondition(GameConditionDef.Named("Radiation"), 3000000, 100);
			map.gameConditionManager.RegisterCondition(cond);
		}
	}
}
