using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace SM_HackDesignator
{
    [StaticConstructorOnStartup]
    static public class HarmonyPatches
    {
        public static Harmony harmonyInstance;


        static HarmonyPatches()
        {
            harmonyInstance = new Harmony("rimworld.rwmods.SM_HackDesignator");
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

			//m_HudIcon = ContentFinder<Texture2D>.Get("UI/Buttons/ShowOreOverlay");
			//m_DeepHudIcon = ContentFinder<Texture2D>.Get("UI/Buttons/ShowDeepOverlay");

			// Add the designator to the def list
			//DefGenerator.AddImpliedDef<DesignationDef>(HackDesignatorDefOf.SM_Designator_Hack, false);			
		}

    }

    [HarmonyPatch(typeof(WorkGiver_Hack))]
    [HarmonyPatch("HasJobOnThing")]
    internal static class GetUpGraphicSafe
	{
		private static bool HasDesignationOnThing(Pawn pawn, Thing t, bool forced = false) => pawn.Map.designationManager.DesignationOn(t, HackDesignatorDefOf.SM_Designator_Hack) != null && pawn.CanReserve((LocalTargetInfo)t, ignoreOtherReservations: forced);

		[HarmonyPostfix]
        private static void PostFix(WorkGiver_Hack __instance, ref bool __result, Pawn pawn, Thing t, bool forced)
        {
            if (__result == false)
			{
				CompHackable comp = t.TryGetComp<CompHackable>();
				if ((comp == null) || !HasDesignationOnThing(pawn, t, forced))
					return;
				AcceptanceReport acceptanceReport = comp.CanHackNow(pawn);
				if (!acceptanceReport.Accepted && !acceptanceReport.Reason.NullOrEmpty())
					JobFailReason.Is(acceptanceReport.Reason);

				__result = (forced || pawn.CanReserve((LocalTargetInfo)t)) && (!forced || !ModsConfig.IdeologyActive || t.def != ThingDefOf.AncientEnemyTerminal) && acceptanceReport.Accepted;
			}
        }
    }

	[HarmonyPatch(typeof(CompHackable))]
	[HarmonyPatch("OnHacked")]
	internal static class HandleOnHacked
	{		
		[HarmonyPostfix]
		private static void PostFix(CompHackable __instance, Pawn hacker, bool suppressMessages)
		{
			if (__instance.IsHacked == true)
			{
				Thing hackedThing = __instance.parent;
				if (hackedThing != null)
				{
					if (hackedThing.Map.designationManager.DesignationOn(hackedThing, HackDesignatorDefOf.SM_Designator_Hack) != null)
						hackedThing.Map.designationManager.TryRemoveDesignationOn(hackedThing, HackDesignatorDefOf.SM_Designator_Hack);
				}
			}
		}
	}

	/*
	[HarmonyPatch(typeof(CompHackable))]
	[HarmonyPatch("OnLockedOut")]
	internal static class HandleOnLockedOut
	{
		[HarmonyPostfix]
		private static void PostFix(CompHackable __instance, Pawn hacker)
		{
			Thing hackedThing = __instance.parent;
			if (hackedThing != null)
			{
				if (hackedThing.Map.designationManager.DesignationOn(hackedThing, HackDesignatorDefOf.SM_Designator_Hack) != null)
				{
					// Swap the designation out for an inactive one
					hackedThing.Map.designationManager.TryRemoveDesignationOn(hackedThing, HackDesignatorDefOf.SM_Designator_Hack);
					hackedThing.Map.designationManager.AddDesignation(
				}
			}
		}
	}*/
}
