using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace ReconAndDiscovery
{
	[StaticConstructorOnStartup]
	public class CompTeleporter : ThingComp
	{
		public CompTeleporter()
		{
		}

		public bool ReadyToTransport
		{
			get
			{
				return this.fCharge >= 1f;
			}
		}

		public void ResetCharge()
		{
			this.fCharge = 0f;
		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.ReadyToTransport)
			{
				yield return this.TeleportCommand();
			}
			yield break;
		}

		public Command TeleportCommand()
		{
			return new Command_Action
			{
				defaultLabel = "Teleport.",
				defaultDesc = "Teleport a person or animal.",
				icon = CompTeleporter.teleSym,
				action = delegate()
				{
					this.StartChoosingTarget();
				}
			};
		}

		public override void CompTick()
		{
			if (this.parent.GetComp<CompPowerTrader>().PowerOn)
			{
				this.fCharge += 0.0001f;
				if (this.fCharge > 1f)
				{
					this.fCharge = 1f;
				}
			}
			else
			{
				this.fCharge -= 0.0004f;
				if (this.fCharge < 0f)
				{
					this.fCharge = 0f;
				}
			}
		}

		private void StartChoosingTarget()
		{
			CameraJumper.TryJump(CameraJumper.GetWorldTarget(this.parent));
			Find.WorldSelector.ClearSelection();
			int tile = this.parent.Map.Tile;
			Find.WorldTargeter.BeginTargeting(new Func<GlobalTargetInfo, bool>(this.ChoseWorldTarget), true, CompTeleporter.TargeterMouseAttachment, true, null, null);
		}

		private bool ChoseWorldTarget(GlobalTargetInfo target)
		{
			bool result;
			if (!this.ReadyToTransport)
			{
				result = true;
			}
			else if (!target.IsValid)
			{
				Messages.Message(Translator.Translate("MessageTransportPodsDestinationIsInvalid"), 2);
				result = false;
			}
			else
			{
				MapParent mapParent = target.WorldObject as MapParent;
				if (mapParent != null && mapParent.HasMap)
				{
					Map myMap = this.parent.Map;
					Map map = mapParent.Map;
					Current.Game.VisibleMap = map;
					Targeter targeter = Find.Targeter;
					Action actionWhenFinished = delegate()
					{
						if (Find.Maps.Contains(myMap))
						{
							Current.Game.VisibleMap = myMap;
						}
					};
					TargetingParameters targetParams = new TargetingParameters
					{
						canTargetPawns = true,
						canTargetItems = false,
						canTargetSelf = false,
						canTargetLocations = false,
						canTargetBuildings = false,
						canTargetFires = false
					};
					targeter.BeginTargeting(targetParams, delegate(LocalTargetInfo x)
					{
						if (this.ReadyToTransport)
						{
							this.TryTransport(x.ToGlobalTargetInfo(map));
						}
					}, null, actionWhenFinished, CompTeleporter.TargeterMouseAttachment);
					result = true;
				}
				else
				{
					Messages.Message("You cannot lock onto anything there.", 2);
					result = false;
				}
			}
			return result;
		}

		private static void AddAnaphylaxisIfAppropriate(Pawn pawn)
		{
			Rand.PushState();
			Rand.Seed = pawn.thingIDNumber * 724;
			float value = Rand.Value;
			Rand.PopState();
			if (value < 0.12f)
			{
				pawn.health.AddHediff(HediffDef.Named("Anaphylaxis"), null, null);
			}
		}

		public void TryTransport(GlobalTargetInfo target)
		{
			Map map = this.parent.Map;
			IntVec3 position = this.parent.Position;
			Map map2 = target.Map;
			Pawn pawn = target.Thing as Pawn;
			IntVec3 position2 = pawn.Position;
			if (map2.roofGrid.Roofed(position2) && map2.roofGrid.RoofAt(position2) == RoofDefOf.RoofRockThick)
			{
				Messages.Message("Teleporter cannot lock on through the thick rock overhead!", 2);
			}
			else
			{
				MoteMaker.ThrowMetaPuff(position2.ToVector3(), map2);
				MoteMaker.ThrowMetaPuff(position.ToVector3(), map);
				pawn.DeSpawn();
				Fire fire = (Fire)GenSpawn.Spawn(ThingDefOf.Fire, position2, map2);
				fire.fireSize = 1f;
				GenSpawn.Spawn(pawn, position, map, this.parent.Rotation, false);
				CompTeleporter.AddAnaphylaxisIfAppropriate(pawn);
				this.fCharge = 0f;
			}
		}

		protected string SaveKey
		{
			get
			{
				return "transCharge";
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.fCharge, this.SaveKey, 0f, false);
		}

		public override string CompInspectStringExtra()
		{
			return "Charge: " + this.fCharge.ToStringPercent();
		}

		// Note: this type is marked as 'beforefieldinit'.
		static CompTeleporter()
		{
		}

		[CompilerGenerated]
		private void <TeleportCommand>m__0()
		{
			this.StartChoosingTarget();
		}

		private float fCharge = 0f;

		private static readonly Texture2D TargeterMouseAttachment = ContentFinder<Texture2D>.Get("UI/TeleportMouseAttachment", true);

		private static readonly Texture2D teleSym = ContentFinder<Texture2D>.Get("UI/TeleportSymbol", true);

		[CompilerGenerated]
		private sealed class <CompGetGizmosExtra>c__Iterator0 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			[DebuggerHidden]
			public <CompGetGizmosExtra>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					if (base.ReadyToTransport)
					{
						this.$current = base.TeleportCommand();
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1U:
					break;
				default:
					return false;
				}
				this.$PC = -1;
				return false;
			}

			Gizmo IEnumerator<Gizmo>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Gizmo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				CompTeleporter.<CompGetGizmosExtra>c__Iterator0 <CompGetGizmosExtra>c__Iterator = new CompTeleporter.<CompGetGizmosExtra>c__Iterator0();
				<CompGetGizmosExtra>c__Iterator.$this = this;
				return <CompGetGizmosExtra>c__Iterator;
			}

			internal CompTeleporter $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;
		}

		[CompilerGenerated]
		private sealed class <ChoseWorldTarget>c__AnonStorey1
		{
			public <ChoseWorldTarget>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				if (Find.Maps.Contains(this.myMap))
				{
					Current.Game.VisibleMap = this.myMap;
				}
			}

			internal void <>m__1(LocalTargetInfo x)
			{
				if (this.$this.ReadyToTransport)
				{
					this.$this.TryTransport(x.ToGlobalTargetInfo(this.map));
				}
			}

			internal Map myMap;

			internal Map map;

			internal CompTeleporter $this;
		}
	}
}
