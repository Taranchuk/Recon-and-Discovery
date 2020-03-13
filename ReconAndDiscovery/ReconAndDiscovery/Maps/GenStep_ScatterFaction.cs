using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.BaseGen;
using Verse;

namespace ReconAndDiscovery.Maps
{
	public class GenStep_ScatterFaction : GenStep
	{
		public GenStep_ScatterFaction()
		{
		}

		public virtual void Generate(Map map)
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

		[CompilerGenerated]
		private static bool <SetAllStructuresToFaction>m__0(Thing thing)
		{
			return thing.def.IsDoor;
		}

		[CompilerGenerated]
		private static Func<Thing, bool> <>f__am$cache0;
	}
}
