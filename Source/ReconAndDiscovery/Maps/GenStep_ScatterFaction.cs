﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.BaseGen;
using Verse;

namespace ReconAndDiscovery.Maps
{
	public class GenStep_ScatterFaction : GenStep
	{
        public override int SeedPart
        {
            get
            {
                return 349640510;
            }
        }
        public override void Generate(Map map, GenStepParams parms)
		{
		}

		private void SetAllStructuresToFaction(Faction f, Map m)
		{
			IEnumerable<Thing> enumerable = from thing in m.listerThings.AllThings
			where thing.def.IsDoor
			select thing;
			foreach (Thing thing2 in enumerable)
			{
				thing2.SetFaction(f, null);
			}
			BaseGen.Generate();
		}
	}
}












