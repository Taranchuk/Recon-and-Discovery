using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;
using Verse.AI;

namespace ReconAndDiscovery
{
	public class WorkGiver_Sacrifice : WorkGiver_Scanner
	{
		public WorkGiver_Sacrifice()
		{
		}

		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.PotentialBillGiver);
			}
		}

		public override bool Prioritized
		{
			get
			{
				return true;
			}
		}

		public override float GetPriority(Pawn pawn, TargetInfo t)
		{
			return 0f;
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			bool result;
			if (pawn2 == null)
			{
				result = false;
			}
			else if (pawn.Map.designationManager.DesignationOn(pawn2, DesignationDefOf.Slaughter) == null)
			{
				result = false;
			}
			else if (pawn.story != null && pawn.story.WorkTagIsDisabled(WorkTags.Commoner))
			{
				JobFailReason.Is(Translator.Translate("IsIncapableOfViolenceShort"));
				result = false;
			}
			else
			{
				IEnumerable<Building> source = pawn.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDef.Named("PsionicEmanator"));
				result = (source.Count<Building>() != 0 && !source.All((Building b) => !pawn.CanReserve(b, 1, -1, null, false)) && GenHostility.AnyHostileActiveThreat(pawn.Map) && pawn2 != null && pawn.CanReserve(t, 1, -1, null, forced));
			}
			return result;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			IEnumerable<Building> source = from a in pawn.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDef.Named("PsionicEmanator"))
			where pawn.CanReserveAndReach(a, PathEndMode.ClosestTouch, Danger.Some, 1, -1, null, false)
			select a;
			Job result;
			if (source.Count<Building>() == 0)
			{
				result = null;
			}
			else
			{
				Building t2 = source.FirstOrDefault<Building>();
				result = new Job(JobDefOfReconAndDiscovery.SacrificeAtAltar, t, t2);
			}
			return result;
		}

		[CompilerGenerated]
		private sealed class <HasJobOnThing>c__AnonStorey0
		{
			public <HasJobOnThing>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Building b)
			{
				return !this.pawn.CanReserve(b, 1, -1, null, false);
			}

			internal Pawn pawn;
		}

		[CompilerGenerated]
		private sealed class <JobOnThing>c__AnonStorey1
		{
			public <JobOnThing>c__AnonStorey1()
			{
			}

			internal bool <>m__0(Building a)
			{
				return this.pawn.CanReserveAndReach(a, PathEndMode.ClosestTouch, Danger.Some, 1, -1, null, false);
			}

			internal Pawn pawn;
		}
	}
}
