﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace ReconAndDiscovery
{
	public class GameCondition_TargetedStorm : GameCondition
	{
		public override void End()
		{
			base.Map.weatherDecider.DisableRainFor(30000);
			base.End();
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
			Scribe_Values.Look<int>(ref this.nextLightningTicks, "nextLightningTicks", 0, false);
		}

		private void FindNewTarget()
		{
			IEnumerable<Pawn> source = from p in base.Map.mapPawns.AllPawnsSpawned
			where p.HostileTo(Faction.OfPlayer)
			select p;
			if (source.Count<Pawn>() > 0)
			{
				this.target = source.RandomElement<Pawn>();
			}
			else
			{
				this.End();
			}
		}

		public override void GameConditionTick()
		{
			if (this.target == null || !this.target.Spawned)
			{
				this.FindNewTarget();
			}
			else if (Find.TickManager.TicksGame > this.nextLightningTicks)
			{
				Vector2 vector;
				vector..ctor(Rand.Gaussian(0f, 1f), Rand.Gaussian(0f, 1f));
				vector.Normalize();
				vector *= Rand.Range(0f, (float)this.areaRadius);
				IntVec3 intVec = new IntVec3((int)Math.Round((double)vector.x) + this.target.Position.x, 0, (int)Math.Round((double)vector.y) + this.target.Position.z);
				if (this.IsGoodLocationForStrike(intVec))
				{
					base.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(base.Map, intVec));
					this.nextLightningTicks = Find.TickManager.TicksGame + GameCondition_TargetedStorm.TicksBetweenStrikes.RandomInRange;
				}
			}
		}

		private IEnumerable<IntVec3> GetPotentiallyAffectedCells(IntVec2 center)
		{
			return GenRadial.RadialCellsAround(this.target.Position, 5f, true);
		}

		public override void Init()
		{
			base.Init();
		}

		private bool IsGoodLocationForStrike(IntVec3 loc)
		{
			return loc.InBounds(base.Map) && !loc.Roofed(base.Map) && loc.Standable(base.Map);
		}

		private const int RainDisableTicksAfterConditionEnds = 30000;

		private static readonly IntRange TicksBetweenStrikes = new IntRange(250, 600);

		private int nextLightningTicks;

		private int areaRadius = 5;

		public Thing target;
	}
}
