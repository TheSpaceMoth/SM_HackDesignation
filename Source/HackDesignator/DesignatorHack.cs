using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;
using UnityEngine;

namespace SM_HackDesignator
{
	public class SM_Designator_Hack : Designator
	{
		protected override DesignationDef Designation => HackDesignatorDefOf.SM_Designator_Hack;

		public override DrawStyleCategoryDef DrawStyleCategory => DrawStyleCategoryDefOf.Orders;

		public SM_Designator_Hack()
		{
			this.defaultLabel = (string)"SM_HackDesignator.DesignatorHack".Translate();
			this.defaultDesc = (string)"SM_HackDesignator.DesignatorHackDesc".Translate();
			this.icon = (Texture)ContentFinder<Texture2D>.Get("UI/Commands/Hack");
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			//this.hotKey = KeyBindingDefOf.Misc5;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Claim;
		}

		protected override void FinalizeDesignationFailed()
		{
			base.FinalizeDesignationFailed();
			Messages.Message((string)"SM_HackDesignator.MessageMustDesignateHackables".Translate(), MessageTypeDefOf.RejectInput, false);
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(this.Map))
				return (AcceptanceReport)false;
			return !this.HackablesInCell(c).Any<Thing>() ? (AcceptanceReport)false : (AcceptanceReport)true;
		}

		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Thing t in this.HackablesInCell(c))
				this.DesignateThing(t);
		}

		public override AcceptanceReport CanDesignateThing(Thing t) => !(t.TryGetComp<CompHackable>() is CompHackable hackable) || hackable.Props.onlyRemotelyHackable || hackable.IsHacked || this.Map.designationManager.DesignationOn(t, this.Designation) != null ? (AcceptanceReport)false : (AcceptanceReport)true;

		public override void DesignateThing(Thing t)
		{
			
			this.Map.designationManager.AddDesignation(new Verse.Designation((LocalTargetInfo)t, this.Designation));
		}

		private IEnumerable<Thing> HackablesInCell(IntVec3 c)
		{
			SM_Designator_Hack designatorHack = this;
			if (!c.Fogged(designatorHack.Map))
			{
				List<Thing> thingList = c.GetThingList(designatorHack.Map);
				for (int i = 0; i < thingList.Count; ++i)
				{
					if (designatorHack.CanDesignateThing(thingList[i]).Accepted)
						yield return thingList[i];
				}
			}
		}
	}
}
