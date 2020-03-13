using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ReconAndDiscovery.Things;
using RimWorld;
using Verse;

namespace ReconAndDiscovery
{
	[StaticConstructorOnStartup]
	public class CompHoloEmitter : ThingComp
	{
		public CompHoloEmitter()
		{
		}

		public Pawn SimPawn
		{
			get
			{
				return this.pawn;
			}
			set
			{
				this.pawn = value;
			}
		}

		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.pawn != null)
			{
				DamageInfo value = new DamageInfo(DamageDefOf.Blunt, 1000, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
				this.SimPawn.Kill(new DamageInfo?(value));
				this.SimPawn.Corpse.Destroy(DestroyMode.Vanish);
			}
		}

		public override void PostDeSpawn(Map map)
		{
			if (this.pawn != null && this.pawn.Spawned)
			{
				this.pawn.DeSpawn();
			}
			base.PostDeSpawn(map);
		}

		private HoloEmitter Emitter
		{
			get
			{
				return this.parent as HoloEmitter;
			}
		}

		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			if (this.pawn != null)
			{
				FloatMenuOption floatMenuOption = new FloatMenuOption(MenuOptionPriority.Default);
				floatMenuOption.Label = "Format Occupant";
				floatMenuOption.action = delegate()
				{
					DamageInfo value = new DamageInfo(DamageDefOf.ExecutionCut, 1000, -1f, selPawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
					this.pawn.Kill(new DamageInfo?(value));
					this.pawn.Corpse.Destroy(DestroyMode.Vanish);
					this.pawn = null;
				};
				if (selPawn != this.pawn)
				{
					list.Add(floatMenuOption);
				}
			}
			else if (selPawn.story.traits.HasTrait(TraitDef.Named("Holographic")))
			{
				FloatMenuOption floatMenuOption2 = new FloatMenuOption(MenuOptionPriority.Default);
				floatMenuOption2.Label = "Transfer to this emitter";
				floatMenuOption2.action = delegate()
				{
					foreach (Thing thing in this.parent.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDef.Named("HolographicEmitter")))
					{
						HoloEmitter holoEmitter = thing as HoloEmitter;
						if (holoEmitter == null)
						{
							break;
						}
						if (holoEmitter.GetComp<CompHoloEmitter>().SimPawn == selPawn)
						{
							holoEmitter.GetComp<CompHoloEmitter>().SimPawn = null;
							this.pawn = selPawn;
							this.parent.Map.areaManager.AllAreas.Remove(this.pawn.playerSettings.AreaRestriction);
							this.MakeValidAllowedZone();
							break;
						}
					}
				};
				if (this.pawn == null)
				{
					list.Add(floatMenuOption2);
				}
			}
			return list;
		}

		public override void PostExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.pawn, "simulatedPawn", false);
		}

		public void SetUpPawn()
		{
			if (this.pawn.Spawned)
			{
				this.pawn.DeSpawn();
			}
			this.pawn.health.Reset();
			this.pawn.story.traits.GainTrait(new Trait(TraitDef.Named("Holographic"), 0, false));
			GenSpawn.Spawn(this.pawn, this.parent.Position, this.parent.Map);
			this.MakeValidAllowedZone();
		}

		private void HoloTickPawn()
		{
			if (this.pawn != null)
			{
				if (this.pawn.Dead)
				{
					Log.Message(string.Format("{0} is dead.", this.pawn.NameStringShort));
					if (this.pawn.Corpse.holdingOwner != this.Emitter.GetDirectlyHeldThings())
					{
						if (this.Emitter.TryAcceptThing(this.pawn.Corpse, true))
						{
						}
					}
				}
				else
				{
					if (!this.pawn.story.traits.HasTrait(TraitDef.Named("Holographic")))
					{
						this.SetUpPawn();
					}
					if (!this.pawn.Spawned)
					{
						GenSpawn.Spawn(this.pawn, this.parent.Position, this.parent.Map);
					}
					this.pawn.needs.food.CurLevel = 1f;
					if (!this.pawn.Position.InHorDistOf(this.parent.Position, 12f) || !GenSight.LineOfSight(this.parent.Position, this.pawn.Position, this.parent.Map, true, null, 0, 0))
					{
						this.pawn.inventory.DropAllNearPawn(this.pawn.Position, false, false);
						this.pawn.equipment.DropAllEquipment(this.pawn.Position, false);
						this.pawn.DeSpawn();
						GenSpawn.Spawn(this.pawn, this.parent.Position, this.parent.Map);
					}
					this.pawn.health.Reset();
				}
			}
		}

		public void Scan(Corpse c)
		{
			if (this.Emitter.TryAcceptThing(c, true))
			{
				c.InnerPawn.story.traits.GainTrait(new Trait(TraitDef.Named("Holographic"), 0, false));
			}
		}

		public void MakeValidAllowedZone()
		{
			IEnumerable<IntVec3> enumerable = from cell in GenRadial.RadialCellsAround(this.parent.Position, 18f, true)
			where cell.InHorDistOf(this.parent.Position, 12f) && GenSight.LineOfSight(this.parent.Position, cell, this.parent.Map, true, null, 0, 0)
			select cell;
			Area_Allowed area_Allowed;
			this.parent.Map.areaManager.TryMakeNewAllowed(1, ref area_Allowed);
			foreach (IntVec3 c in enumerable)
			{
				area_Allowed[this.parent.Map.cellIndices.CellToIndex(c)] = true;
			}
			area_Allowed.SetLabel(string.Format("HoloEmitter area for {0}.", this.pawn.NameStringShort));
			this.pawn.playerSettings.AreaRestriction = area_Allowed;
		}

		public override void CompTickRare()
		{
			base.CompTickRare();
			if (this.pawn != null)
			{
				foreach (Designation designation in this.parent.Map.designationManager.AllDesignationsOn(this.parent))
				{
					if (designation.def == DesignationDefOf.Uninstall)
					{
						if (this.pawn.Spawned)
						{
							this.pawn.DeSpawn();
						}
						return;
					}
				}
				if (this.parent.GetComp<CompPowerTrader>().PowerOn)
				{
					this.HoloTickPawn();
				}
				else if (this.pawn.Spawned)
				{
					this.pawn.DeSpawn();
				}
			}
		}

		[CompilerGenerated]
		private bool <MakeValidAllowedZone>m__0(IntVec3 cell)
		{
			return cell.InHorDistOf(this.parent.Position, 12f) && GenSight.LineOfSight(this.parent.Position, cell, this.parent.Map, true, null, 0, 0);
		}

		private Pawn pawn;

		[CompilerGenerated]
		private sealed class <CompFloatMenuOptions>c__AnonStorey0
		{
			public <CompFloatMenuOptions>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				DamageInfo value = new DamageInfo(DamageDefOf.ExecutionCut, 1000, -1f, this.selPawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
				this.$this.pawn.Kill(new DamageInfo?(value));
				this.$this.pawn.Corpse.Destroy(DestroyMode.Vanish);
				this.$this.pawn = null;
			}

			internal void <>m__1()
			{
				foreach (Thing thing in this.$this.parent.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDef.Named("HolographicEmitter")))
				{
					HoloEmitter holoEmitter = thing as HoloEmitter;
					if (holoEmitter == null)
					{
						break;
					}
					if (holoEmitter.GetComp<CompHoloEmitter>().SimPawn == this.selPawn)
					{
						holoEmitter.GetComp<CompHoloEmitter>().SimPawn = null;
						this.$this.pawn = this.selPawn;
						this.$this.parent.Map.areaManager.AllAreas.Remove(this.$this.pawn.playerSettings.AreaRestriction);
						this.$this.MakeValidAllowedZone();
						break;
					}
				}
			}

			internal Pawn selPawn;

			internal CompHoloEmitter $this;
		}
	}
}
