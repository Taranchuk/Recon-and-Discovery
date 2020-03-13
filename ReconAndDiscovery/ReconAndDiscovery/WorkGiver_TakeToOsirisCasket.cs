using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;
using Verse.AI;

namespace ReconAndDiscovery
{
	public class WorkGiver_TakeToOsirisCasket : WorkGiver_Scanner
	{
		public WorkGiver_TakeToOsirisCasket()
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

		private Building_CryptosleepCasket FindCasket(Pawn p, Corpse corpse)
		{
			IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
			where typeof(OsirisCasket).IsAssignableFrom(def.thingClass)
			select def;
			foreach (ThingDef singleDef in enumerable)
			{
				Predicate<Thing> validator = (Thing x) => !((Building_CryptosleepCasket)x).HasAnyContents && p.CanReserve(x, 1, -1, null, false);
				Building_CryptosleepCasket building_CryptosleepCasket = (Building_CryptosleepCasket)GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForDef(singleDef), PathEndMode.InteractionCell, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
				if (building_CryptosleepCasket != null)
				{
					return building_CryptosleepCasket;
				}
			}
			enumerable = from def in DefDatabase<ThingDef>.AllDefs
			where typeof(Building_CryptosleepCasket).IsAssignableFrom(def.thingClass)
			select def;
			foreach (ThingDef singleDef2 in enumerable)
			{
				Predicate<Thing> validator2 = (Thing x) => !((Building_CryptosleepCasket)x).HasAnyContents && p.CanReserve(x, 1, -1, null, false);
				Building_CryptosleepCasket building_CryptosleepCasket2 = (Building_CryptosleepCasket)GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForDef(singleDef2), PathEndMode.InteractionCell, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator2, null, 0, -1, false, RegionType.Set_Passable, false);
				if (building_CryptosleepCasket2 != null)
				{
					return building_CryptosleepCasket2;
				}
			}
			return null;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Corpse corpse = t as Corpse;
			Job result;
			if (corpse == null)
			{
				result = null;
			}
			else if (corpse.InnerPawn.Faction != Faction.OfPlayer)
			{
				result = null;
			}
			else if (corpse.IsNotFresh())
			{
				result = null;
			}
			else if (!HaulAIUtility.PawnCanAutomaticallyHaul(pawn, t, forced))
			{
				result = null;
			}
			else
			{
				Building_CryptosleepCasket building_CryptosleepCasket = this.FindCasket(pawn, corpse);
				if (building_CryptosleepCasket == null)
				{
					result = null;
				}
				else if (building_CryptosleepCasket.ContainedThing != null)
				{
					result = null;
				}
				else
				{
					result = new Job(JobDefOfReconAndDiscovery.TakeToOsirisCasket, t, building_CryptosleepCasket)
					{
						count = corpse.stackCount
					};
				}
			}
			return result;
		}

		public virtual bool ShouldSkip(Pawn pawn)
		{
			return pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Blueprint).Count == 0;
		}

		[CompilerGenerated]
		private static bool <FindCasket>m__0(ThingDef def)
		{
			return typeof(OsirisCasket).IsAssignableFrom(def.thingClass);
		}

		[CompilerGenerated]
		private static bool <FindCasket>m__1(ThingDef def)
		{
			return typeof(Building_CryptosleepCasket).IsAssignableFrom(def.thingClass);
		}

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache1;

		[CompilerGenerated]
		private sealed class <FindCasket>c__AnonStorey0
		{
			public <FindCasket>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing x)
			{
				return !((Building_CryptosleepCasket)x).HasAnyContents && this.p.CanReserve(x, 1, -1, null, false);
			}

			internal bool <>m__1(Thing x)
			{
				return !((Building_CryptosleepCasket)x).HasAnyContents && this.p.CanReserve(x, 1, -1, null, false);
			}

			internal Pawn p;
		}
	}
}
