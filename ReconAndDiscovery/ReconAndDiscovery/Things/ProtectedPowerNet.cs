using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Harmony;
using RimWorld;
using Verse;

namespace ReconAndDiscovery.Things
{
	[HarmonyPatch(typeof(PowerNet), "PowerNetTick", null)]
	public static class ProtectedPowerNet
	{
		[HarmonyPrefix]
		private static bool PrePowerTick(PowerNet __instance)
		{
			Map map = __instance.powerNetManager.map;
			ProtectedPowerNet.deflectors.Clear();
			ProtectedPowerNet.deflectors = map.listerThings.ThingsOfDef(ThingDef.Named("DeflectorArray"));
			ProtectedPowerNet.deflectors = (from thing in ProtectedPowerNet.deflectors
			where (thing as Building).GetComp<CompPowerTrader>().PowerOn
			select thing).ToList<Thing>();
			bool result;
			if (ProtectedPowerNet.deflectors.NullOrEmpty<Thing>())
			{
				result = true;
			}
			else
			{
				ProtectedPowerNet.ProtectedPowerTick(__instance);
				result = false;
			}
			return result;
		}

		public static void ProtectedPowerTick(PowerNet net)
		{
			float num = net.CurrentEnergyGainRate();
			float num2 = net.CurrentStoredEnergy();
			if (num2 + num >= -1E-07f && !net.powerNetManager.map.gameConditionManager.ConditionIsActive(GameConditionDefOf.SolarFlare))
			{
				float num3;
				if (net.batteryComps.Count > 0 && num2 >= 0.1f)
				{
					num3 = num2 - 5f;
				}
				else
				{
					num3 = num2;
				}
				if (num3 + num >= 0f)
				{
					ProtectedPowerNet.PartsWantingPowerOn.Clear();
					for (int i = 0; i < net.powerComps.Count; i++)
					{
						if (!net.powerComps[i].PowerOn && FlickUtility.WantsToBeOn(net.powerComps[i].parent) && !net.powerComps[i].parent.IsBrokenDown())
						{
							ProtectedPowerNet.PartsWantingPowerOn.Add(net.powerComps[i]);
						}
					}
					if (ProtectedPowerNet.PartsWantingPowerOn.Count > 0)
					{
						int num4 = 200 / ProtectedPowerNet.PartsWantingPowerOn.Count;
						if (num4 < 30)
						{
							num4 = 30;
						}
						if (Find.TickManager.TicksGame % num4 == 0)
						{
							CompPowerTrader compPowerTrader = ProtectedPowerNet.PartsWantingPowerOn.RandomElement<CompPowerTrader>();
							if (num + num2 >= -(compPowerTrader.EnergyOutputPerTick + 1E-07f))
							{
								compPowerTrader.PowerOn = true;
								num += compPowerTrader.EnergyOutputPerTick;
							}
						}
					}
				}
				MethodInfo method = typeof(PowerNet).GetMethod("ChangeStoredEnergy", BindingFlags.Instance | BindingFlags.NonPublic);
				method.Invoke(net, new object[]
				{
					num
				});
			}
			else if (Find.TickManager.TicksGame % 20 == 0)
			{
				ProtectedPowerNet.PotentialPartsToShutDown.Clear();
				for (int j = 0; j < net.powerComps.Count; j++)
				{
					bool flag = false;
					for (int k = 0; k < ProtectedPowerNet.deflectors.Count; k++)
					{
						if (net.powerComps[j].parent.Position.InHorDistOf(ProtectedPowerNet.deflectors[k].Position, 13f))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						if (net.powerComps[j].PowerOn && net.powerComps[j].EnergyOutputPerTick < 0f)
						{
							ProtectedPowerNet.PotentialPartsToShutDown.Add(net.powerComps[j]);
						}
					}
				}
				if (ProtectedPowerNet.PotentialPartsToShutDown.Count > 0)
				{
					ProtectedPowerNet.PotentialPartsToShutDown.RandomElement<CompPowerTrader>().PowerOn = false;
				}
			}
		}

		private static List<CompPowerTrader> PartsWantingPowerOn = new List<CompPowerTrader>();

		private static List<CompPowerTrader> PotentialPartsToShutDown = new List<CompPowerTrader>();

		private static List<Thing> deflectors = new List<Thing>();
	}
}
