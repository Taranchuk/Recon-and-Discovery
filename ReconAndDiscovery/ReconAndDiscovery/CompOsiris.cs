using System;
using System.Collections.Generic;
using System.Reflection;
using ReconAndDiscovery.Things;
using RimWorld;
using Verse;
using Verse.AI;

namespace ReconAndDiscovery
{
	[StaticConstructorOnStartup]
	public class CompOsiris : ThingComp
	{
		public Building_Casket Casket
		{
			get
			{
				return this.parent as Building_Casket;
			}
		}

		private bool ReadyToHeal
		{
			get
			{
				return this.Casket.ContainedThing is Pawn || (this.Casket.ContainedThing is Corpse && !(this.Casket.ContainedThing as Corpse).IsNotFresh() && this.parent.GetComp<CompPowerTrader>().PowerOn && this.parent.GetComp<CompRefuelable>().Fuel >= 50f);
			}
		}

		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (mode == DestroyMode.Deconstruct)
			{
				GenSpawn.Spawn(ThingDef.Named("OsirisAI"), this.parent.Position, previousMap);
			}
		}

		public static void AddComponentsForResurrection(Pawn pawn)
		{
			pawn.carryTracker = new Pawn_CarryTracker(pawn);
			pawn.needs = new Pawn_NeedsTracker(pawn);
			pawn.mindState = new Pawn_MindState(pawn);
			if (pawn.RaceProps.Humanlike)
			{
				pawn.workSettings = new Pawn_WorkSettings(pawn);
				pawn.workSettings.EnableAndInitialize();
			}
			PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, false);
		}

		public static void Ressurrect(Pawn pawn, Thing thing)
		{
			pawn.health.Reset();
			if (pawn.Corpse != null && pawn.Corpse.Spawned)
			{
				pawn.Corpse.DeSpawn();
			}
			CompOsiris.AddComponentsForResurrection(pawn);
			Type typeFromHandle = typeof(Thing);
			FieldInfo field = typeFromHandle.GetField("mapIndexOrState", BindingFlags.Instance | BindingFlags.NonPublic);
			field.SetValue(pawn, -1);
			CompOsiris.FixPawnRelationships(pawn);
			if (thing is HoloEmitter)
			{
				if (!pawn.Dead)
				{
				}
				if (pawn.Corpse.holdingOwner != null)
				{
					pawn.Corpse.GetDirectlyHeldThings().TryTransferToContainer(pawn, pawn.Corpse.holdingOwner, true);
				}
				else if (pawn.Corpse.Spawned)
				{
					GenSpawn.Spawn(pawn, pawn.Corpse.Position, pawn.Corpse.Map);
				}
				if (pawn.Corpse != null)
				{
					pawn.Corpse.Destroy(DestroyMode.Vanish);
				}
			}
			else
			{
				GenSpawn.Spawn(pawn, thing.Position, thing.Map);
				Building_Casket building_Casket = thing as Building_Casket;
				if (building_Casket != null)
				{
					building_Casket.GetDirectlyHeldThings().Clear();
				}
			}
		}

		public void HealContained()
		{
			if (this.Casket.ContainedThing != null)
			{
				Corpse corpse = this.Casket.ContainedThing as Corpse;
				Pawn pawn;
				if (corpse != null)
				{
					pawn = corpse.InnerPawn;
				}
				else
				{
					pawn = (this.Casket.ContainedThing as Pawn);
				}
				if (pawn != null)
				{
					if (pawn.Dead)
					{
						CompOsiris.Ressurrect(pawn, this.parent);
					}
					else
					{
						pawn.health.Reset();
					}
					if (pawn.RaceProps.Humanlike)
					{
						pawn.ageTracker.AgeBiologicalTicks = 90000000L;
						if (Rand.Value < 0.65f)
						{
							pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDef.Named("ReturnedFromTheDeadBad"), null);
						}
						else
						{
							pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDef.Named("ReturnedFromTheDeadGood"), null);
						}
					}
					else if (pawn.RaceProps.Animal)
					{
						pawn.ageTracker.AgeBiologicalTicks = (long)(pawn.RaceProps.lifeStageAges[2].minAge * 3600000f);
					}
					pawn.health.AddHediff(HediffDef.Named("LuciferiumAddiction"), null, null);
					pawn.health.AddHediff(HediffDef.Named("LuciferiumHigh"), null, null);
				}
			}
		}

		private static void FixPawnRelationships(Pawn p)
		{
            foreach (Pawn pawn in PawnsFinder.AllCaravansAndTravelingTransportPods_Alive)
			{
				if (pawn != p)
				{
					if (pawn != null && pawn.needs != null && pawn.needs.mood != null && pawn.needs.mood.thoughts != null && pawn.needs.mood.thoughts.memories != null)
					{
						pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.KnowColonistDied, p);
						pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.PawnWithBadOpinionDied, p);
						pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.PawnWithGoodOpinionDied, p);
						pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef.Named("BondedAnimalDied"), p);
						pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef.Named("MySonDied"), p);
						pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef.Named("MyDaughterDied"), p);
						pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef.Named("MyHusbandDied"), p);
						pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef.Named("MyWifeDied"), p);
						pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef.Named("MyFianceDied"), p);
						pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef.Named("MyFianceeDied"), p);
						pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef.Named("MyLoverDied"), p);
						pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef.Named("MyBrotherDied"), p);
						pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef.Named("MySisterDied"), p);
						pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef.Named("MyGrandchildDied"), p);
						pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef.Named("MyFatherDied"), p);
						pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef.Named("MyMotherDied"), p);
						pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef.Named("MyNieceDied"), p);
						pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef.Named("MyNephewDied"), p);
						pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef.Named("MyHalfSiblingDied"), p);
						pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef.Named("MyAuntDied"), p);
						pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef.Named("MyUncleDied"), p);
						pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef.Named("MyGrandparentDied"), p);
						pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef.Named("MyCousinDied"), p);
						pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef.Named("MyKinDied"), p);
					}
				}
			}
		}

		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
            FloatMenuOption floatMenuOption = new FloatMenuOption("ResurrectContained".Translate(), delegate ()
            {
                Job job = new Job(JobDefOfReconAndDiscovery.ActivateOsirisCasket, this.parent);
                job.playerForced = true;
                selPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
            }); ;
			if (this.ReadyToHeal)
			{
				list.Add(floatMenuOption);
			}
			return list;
		}
	}
}
