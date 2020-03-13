using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ReconAndDiscovery.Triggers
{
	public class ActivatedAction_Seraphites : ActivatedAction
	{
		public override bool TryAction(Pawn activatedBy, Map map, Thing trigger)
		{
			return base.TryAction(activatedBy, map, trigger);
		}

		protected override void DisplayDialog(Pawn activatedBy, Map map, Thing trigger)
		{
			bool flag = Rand.Value > 0.4f;
			int level = activatedBy.skills.GetSkill(SkillDefOf.Intellectual).Level;
			DiaNode diaNode = new DiaNode("");
			DiaOption diaOption = new DiaOption(string.Format("Log off {0}", activatedBy.NameStringShort));
			diaOption.resolveTree = true;
			DiaOption diaOption2 = new DiaOption(string.Format("OK", activatedBy.NameStringShort));
			diaOption2.resolveTree = true;
			diaNode.options.Add(diaOption);
			if (level < 5)
			{
				diaNode.text = string.Format("There seem to be files pertanent to a luciferium cure, but {0} lacks the intellectual skills to access them.", activatedBy.NameStringShort);
			}
			else
			{
				diaNode.text = this.def.dialogText;
				if (level >= 12)
				{
					if (flag)
					{
						DiaNode diaNode2 = diaNode;
						diaNode2.text += string.Format("\n{0} thinks that the science underlying the project is sound, and though the process has been lost, any remaining samples of the cure probably work.", activatedBy.NameStringShort);
					}
					else
					{
						DiaNode diaNode3 = diaNode;
						diaNode3.text += string.Format("\n{0} thinks that the research is dubious, and is concerned that any remaining samples will be dangerous.", activatedBy.NameStringShort);
					}
				}
				DiaOption diaOption3 = new DiaOption("Dispense the trial cure");
				if (flag)
				{
					diaOption3.link = new DiaNode("A small, golden foil packet drops out of a nearby hatch!")
					{
						options = 
						{
							diaOption2
						}
					};
					diaOption3.action = delegate()
					{
						Thing thing = ThingMaker.MakeThing(ThingDefOfReconAndDiscovery.Seraphites, null);
						thing.stackCount = Rand.RangeInclusive(1, 3);
						GenSpawn.Spawn(thing, activatedBy.Position, map);
						List<Thing> list = new List<Thing>();
						list.AddRange(map.listerThings.ThingsOfDef(ThingDef.Named("QuestComputerTerminal")));
						foreach (Thing thing2 in list)
						{
							thing2.Destroy(DestroyMode.Deconstruct);
						}
					};
				}
				else
				{
					diaOption3.action = delegate()
					{
						foreach (IntVec3 c in this.GetEffectArea(activatedBy.Position))
						{
							foreach (Thing thing in c.GetThingList(map))
							{
								if (Rand.Chance(0.9f) && thing.def.category == ThingCategory.Pawn && (thing as Pawn).RaceProps.Humanlike)
								{
									Pawn pawn = thing as Pawn;
									if (pawn != null)
									{
										Hediff hediff = HediffMaker.MakeHediff(HediffDef.Named("FibrousMechanites"), pawn, null);
										pawn.health.AddHediff(hediff, null, null);
									}
								}
							}
						}
						List<Thing> list = new List<Thing>();
						list.AddRange(map.listerThings.ThingsOfDef(ThingDef.Named("QuestComputerTerminal")));
						foreach (Thing thing2 in list)
						{
							thing2.Destroy(DestroyMode.Deconstruct);
						}
					};
					diaOption3.link = new DiaNode("The mechanites in the cure have corrupted their programming and escaped their packaging!")
					{
						options = 
						{
							diaOption2
						}
					};
				}
				diaNode.options.Add(diaOption3);
			}
			Find.WindowStack.Add(new Dialog_NodeTree(diaNode, true, false, null));
		}
	}
}
