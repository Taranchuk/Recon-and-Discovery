using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace ReconAndDiscovery.Missions
{
	public class IncidentWorker_MuffaloMassInsanity : IncidentWorker
	{
		public static void DriveInsane(Pawn p)
		{
			p.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, true, false, null);
		}

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			return base.CanFireNow(target);
		}

		public override bool TryExecute(IncidentParms parms)
		{
			Map map = parms.target as Map;
			bool result;
			if (map == null)
			{
				result = false;
			}
			else
			{
				PawnKindDef animalDef = PawnKindDef.Named("Muffalo");
				List<Pawn> list = (from p in map.mapPawns.AllPawnsSpawned
				where p.kindDef == animalDef && Rand.Chance(0.5f)
				select p).ToList<Pawn>();
				if (list.Count < 5)
				{
					result = false;
				}
				else
				{
					list.Shuffle<Pawn>();
					foreach (Pawn p2 in list)
					{
						IncidentWorker_MuffaloMassInsanity.DriveInsane(p2);
					}
					string text = "LetterLabelAnimalInsanityMultiple".Translate() + ": " + animalDef.LabelCap;
					string text2 = "AnimalInsanityMultiple".Translate(new object[]
					{
						animalDef.label
					});
					Find.LetterStack.ReceiveLetter(text, text2, LetterDefOf.BadUrgent, null);
					if (map == Find.VisibleMap)
					{
						Find.CameraDriver.shaker.DoShake(1f);
					}
					result = true;
				}
			}
			return result;
		}
	}
}
