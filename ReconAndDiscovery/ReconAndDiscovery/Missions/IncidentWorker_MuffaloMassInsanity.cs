using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;

namespace ReconAndDiscovery.Missions
{
	public class IncidentWorker_MuffaloMassInsanity : IncidentWorker
	{
		public IncidentWorker_MuffaloMassInsanity()
		{
		}

		public static void DriveInsane(Pawn p)
		{
			p.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, true, false, null);
		}

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			return base.CanFireNow(target);
		}

		public virtual bool TryExecute(IncidentParms parms)
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
					string label = Translator.Translate("LetterLabelAnimalInsanityMultiple") + ": " + animalDef.LabelCap;
					string text = "AnimalInsanityMultiple".Translate(new object[]
					{
						animalDef.label
					});
					Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.BadUrgent, null);
					if (map == Find.VisibleMap)
					{
						Find.CameraDriver.shaker.DoShake(1f);
					}
					result = true;
				}
			}
			return result;
		}

		[CompilerGenerated]
		private sealed class <TryExecute>c__AnonStorey0
		{
			public <TryExecute>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Pawn p)
			{
				return p.kindDef == this.animalDef && Rand.Chance(0.5f);
			}

			internal PawnKindDef animalDef;
		}
	}
}
