using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse;

namespace ReconAndDiscovery
{
	public class GameCondition_Radiation : GameCondition
	{
		public GameCondition_Radiation()
		{
			Color sky = new Color(0.8f, 0.8f, 0.3f);
			Color shadow = new Color(0.9f, 0.9f, 1f);
			Color overlay = new Color(0.7f, 0.7f, 0.5f);
			this.SkyColours = new SkyColorSet(sky, shadow, overlay, 9f);
		}

		public override void End()
		{
			base.End();
		}

		public override void ExposeData()
		{
			base.ExposeData();
		}

		private void AssignRadiationSickness(Pawn p)
		{
			if (p.Faction == Faction.OfPlayer)
			{
				Messages.Message(string.Format("{0} has developed radiation sickness", p.NameStringShort), p, 4);
				p.health.AddHediff(HediffDef.Named("RadiationSickness"), null, null);
			}
		}

		private void GiveCarcinoma(Pawn p)
		{
			if (p.RaceProps.IsFlesh)
			{
				List<BodyPartRecord> allParts = p.RaceProps.body.AllParts;
				BodyPartDef partDef = allParts.RandomElement<BodyPartRecord>().def;
				if (p.RaceProps.body == BodyDefOf.Human)
				{
					float value = Rand.Value;
					if (value < 0.1f)
					{
						partDef = BodyPartDefOf.LeftLung;
					}
					else if (value < 0.2f)
					{
						partDef = BodyPartDefOf.RightLung;
					}
					else if (value < 0.4f)
					{
						partDef = BodyPartDefOf.Stomach;
					}
					else if (value < 0.6f)
					{
						partDef = BodyPartDefOf.Liver;
					}
					else if (value < 0.8f)
					{
						partDef = BodyPartDefOf.Brain;
					}
				}
				IEnumerable<BodyPartRecord> source = from part in allParts
				where part.def == partDef
				select part;
				if (source.Count<BodyPartRecord>() != 0)
				{
					BodyPartRecord bodyPartRecord = source.RandomElement<BodyPartRecord>();
					if (allParts.Contains(bodyPartRecord))
					{
						if (!p.health.hediffSet.PartIsMissing(bodyPartRecord))
						{
							p.health.AddHediff(HediffDef.Named("Carcinoma"), bodyPartRecord, null);
							Log.Message(string.Format("Added carcinoma to {0}, part {1}", p.NameStringShort, bodyPartRecord.def.label));
						}
					}
				}
			}
		}

		private void Miscarry(Pawn pawn)
		{
			if (pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Pregnant, true) != null)
			{
				if (pawn.Faction == Faction.OfPlayer)
				{
					Messages.Message(string.Format("{0} has miscarried due to radiation poisoning.", pawn.LabelIndefinite()), pawn, 4);
				}
			}
		}

		private bool IsProtectedAt(IntVec3 c)
		{
			Room room = c.GetRoom(base.Map, RegionType.Set_Passable);
			bool result;
			if (room == null)
			{
				result = false;
			}
			else if (room.PsychologicallyOutdoors)
			{
				result = false;
			}
			else
			{
				foreach (IntVec3 c2 in room.Cells)
				{
					if (!c2.Roofed(base.Map))
					{
						return false;
					}
					if (c2.GetRoof(base.Map) != RoofDefOf.RoofRockThick)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		public override void GameConditionTick()
		{
			if (Rand.Chance(0.006666667f))
			{
				List<Thing> list = base.Map.listerThings.ThingsInGroup(ThingRequestGroup.FoodSource);
				if (list.Count != 0)
				{
					Plant plant = list.RandomElement<Thing>() as Plant;
					if (plant != null)
					{
						if (!this.IsProtectedAt(plant.Position))
						{
							if (plant.def != ThingDef.Named("PlantPsychoid"))
							{
								plant.CropBlighted();
								if (plant.sown)
								{
									Messages.Message("A plant has died due to radiation damage", 4);
								}
							}
						}
					}
				}
			}
			foreach (Pawn pawn in base.Map.mapPawns.AllPawnsSpawned)
			{
				if (!this.IsProtectedAt(pawn.Position))
				{
					float chance = 0.14f * pawn.GetStatValue(StatDefOf.ToxicSensitivity, true) / 60000f;
					float chance2 = 0.04f * pawn.GetStatValue(StatDefOf.ToxicSensitivity, true) / 60000f;
					if (Rand.Chance(chance))
					{
						this.AssignRadiationSickness(pawn);
					}
					if (Rand.Chance(chance2))
					{
						this.GiveCarcinoma(pawn);
					}
					if (Rand.Chance(chance) && pawn.health.hediffSet.HasHediff(HediffDefOf.Pregnant))
					{
						this.Miscarry(pawn);
					}
				}
			}
		}

		public override void Init()
		{
			base.Init();
		}

		public virtual SkyTarget? SkyTarget()
		{
			return new SkyTarget?(new SkyTarget(0.1f, this.SkyColours, 1f, 1f));
		}

		public virtual float SkyTargetLerpFactor()
		{
			return GameConditionUtility.LerpInOutValue((float)base.TicksPassed, (float)base.TicksLeft, 2500f, 0.25f);
		}

		private SkyColorSet SkyColours;

		[CompilerGenerated]
		private sealed class <GiveCarcinoma>c__AnonStorey0
		{
			public <GiveCarcinoma>c__AnonStorey0()
			{
			}

			internal bool <>m__0(BodyPartRecord part)
			{
				return part.def == this.partDef;
			}

			internal BodyPartDef partDef;
		}
	}
}
