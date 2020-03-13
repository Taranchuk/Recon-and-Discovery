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
	public class WorkGiver_ScanAtEmitter : WorkGiver_Scanner
	{
		public WorkGiver_ScanAtEmitter()
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
				return ThingRequest.ForGroup(ThingRequestGroup.Blueprint);
			}
		}

		private HoloEmitter FindEmitter(Pawn p, Corpse corpse)
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
			Corpse corpse = t as Corpse;
			Job result;
			if (corpse != null)
			{
				if (corpse.InnerPawn.Faction != Faction.OfPlayer || !corpse.InnerPawn.RaceProps.Humanlike)
				{
					result = null;
				}
				else if (!HaulAIUtility.PawnCanAutomaticallyHaul(pawn, t, forced))
				{
					result = null;
				}
				else
				{
					HoloEmitter holoEmitter = this.FindEmitter(pawn, corpse);
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
						result = new Job(JobDefOfReconAndDiscovery.ScanAtEmitter, t, holoEmitter)
						{
							count = corpse.stackCount
						};
					}
				}
			}
			else
			{
				result = null;
			}
			return result;
		}

		public virtual bool ShouldSkip(Pawn pawn)
		{
			return pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Blueprint).Count == 0;
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
