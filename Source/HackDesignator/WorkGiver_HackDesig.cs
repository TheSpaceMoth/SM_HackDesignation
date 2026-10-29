using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace SM_HackDesignator
{
	/*
	public class WorkGiver_HackDesignation : WorkGiver_Scanner
	{
		public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForGroup(ThingRequestGroup.Hackable);

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompHackable comp = t.TryGetComp<CompHackable>();
			if ((comp == null) || !HasDesignationOnThing(pawn, t, forced))
				return false;
			AcceptanceReport acceptanceReport = comp.CanHackNow(pawn);
			if (!acceptanceReport.Accepted && !acceptanceReport.Reason.NullOrEmpty())
				JobFailReason.Is(acceptanceReport.Reason);
			return (forced || pawn.CanReserve((LocalTargetInfo)t)) && (!forced || !ModsConfig.IdeologyActive || t.def != ThingDefOf.AncientEnemyTerminal) && acceptanceReport.Accepted;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.TryGetComp<CompHackable>() == null)
				return (Job)null;
			Job job = JobMaker.MakeJob(JobDefOf.Hack, (LocalTargetInfo)t);
			job.playerForced = forced;
			return job;
		}


		private bool HasDesignationOnThing(Pawn pawn, Thing t, bool forced = false) => pawn.Map.designationManager.DesignationOn(t, HackDesignatorDefOf.SM_HackDesignation) != null && pawn.CanReserve((LocalTargetInfo)t, ignoreOtherReservations: forced);

		//public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false) => JobMaker.MakeJob(JobDefOf.Open, (LocalTargetInfo)t);
	}*/
}