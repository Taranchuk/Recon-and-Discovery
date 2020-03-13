using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ReconAndDiscovery.Things;
using RimWorld;
using Verse;
using Verse.AI;

namespace ReconAndDiscovery
{
	public class WorkGiver_LoadIntoEmitter : WorkGiver_Scanner
	{
		public WorkGiver_LoadIntoEmitter()
		{
		}

		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDef.Named("HoloDisk"));
			}
		}

		private HoloEmitter FindEmitter(Pawn p, Thing corpse)
		{
			IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
			where typeof(HoloEmitter).IsAssignableFrom(def.thingClass)
			select def;
			foreach (ThingDef singleDef in enumerable)
			{
				Predicate<Thing> validator = (Thing x) => ((HoloEmitter)x).GetComp<CompHoloEmitter>().SimPawn == null && p.CanReserve(x, 1, -1, null, false);
				HoloEmitter holoEmitter = (HoloEmitter)GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForDef(singleDef), PathEndMode.InteractionCell, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
				if (holoEmitter != null)
				{
					return holoEmitter;
				}
			}
			return null;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job result;
			if (t.def.defName != "HoloDisk")
			{
				result = null;
			}
			else if (!pawn.CanReserveAndReach(t, PathEndMode.Touch, Danger.Deadly, 1, 1, null, false))
			{
				result = null;
			}
			else
			{
				HoloEmitter holoEmitter = this.FindEmitter(pawn, t);
				if (holoEmitter == null)
				{
					result = null;
				}
				else if (holoEmitter.GetComp<CompHoloEmitter>().SimPawn != null)
				{
					result = null;
				}
				else
				{
					result = new Job(JobDefOfReconAndDiscovery.LoadIntoEmitter, t, holoEmitter)
					{
						count = 1
					};
				}
			}
			return result;
		}

		public virtual bool ShouldSkip(Pawn pawn)
		{
			return pawn.Map.listerThings.ThingsOfDef(ThingDef.Named("HoloDisk")).Count == 0;
		}

		[CompilerGenerated]
		private static bool <FindEmitter>m__0(ThingDef def)
		{
			return typeof(HoloEmitter).IsAssignableFrom(def.thingClass);
		}

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache0;

		[CompilerGenerated]
		private sealed class <FindEmitter>c__AnonStorey0
		{
			public <FindEmitter>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing x)
			{
				return ((HoloEmitter)x).GetComp<CompHoloEmitter>().SimPawn == null && this.p.CanReserve(x, 1, -1, null, false);
			}

			internal Pawn p;
		}
	}
}
