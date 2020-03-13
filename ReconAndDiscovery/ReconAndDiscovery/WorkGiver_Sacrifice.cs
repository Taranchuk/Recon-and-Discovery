using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace ReconAndDiscovery
{
	public class WorkGiver_Sacrifice : WorkGiver_Scanner
	{
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
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

		public virtual bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
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
			else if (pawn.story != null && pawn.story.WorkTagIsDisabled(WorkTags.Social))
			{
				JobFailReason.Is("IsIncapableOfViolenceShort".Translate());
				result = false;
			}
			else
			{
				IEnumerable<Building> source = pawn.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDef.Named("PsionicEmanator"));
				result = (source.Count<Building>() != 0 && !source.All((Building b) => !ReservationUtility.CanReserve(pawn, b, 1, -1, null, false)) && GenHostility.AnyHostileActiveThreat(pawn.Map) && pawn2 != null && ReservationUtility.CanReserve(pawn, t, 1, -1, null, forced));
			}
			return result;
		}

		public virtual Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			IEnumerable<Building> source = from a in pawn.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDef.Named("PsionicEmanator"))
			where ReservationUtility.CanReserveAndReach(pawn, a, PathEndMode.ClosestTouch, Danger.Some, 1, -1, null, false)
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
	}
}
