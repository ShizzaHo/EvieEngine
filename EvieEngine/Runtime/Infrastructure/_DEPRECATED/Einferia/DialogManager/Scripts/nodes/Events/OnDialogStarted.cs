using EvieEngine.Einferia;
using Unity.VisualScripting;
using UnityEngine;

namespace EvieEngine.Einferia
{
    [UnitTitle("On Dialog Started")]
    [UnitCategory("Events\\EinferiaDialogManager")]
    public class OnDialogStartedEvent : EventUnit<string>
    {
        [DoNotSerialize]
        protected override bool register => true;

        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput dialogID;

        public override EventHook GetHook(GraphReference reference)
        {
            return new EventHook(EventNames.OnDialogStarted);
        }

        protected override void Definition()
        {
            base.Definition();

            dialogID = ValueInput<string>(nameof(dialogID), string.Empty);
        }

        protected override bool ShouldTrigger(Flow flow, string data)
        {
            string expectedDialogID = flow.GetValue<string>(dialogID);
            if (expectedDialogID == data)
            {
                EinferiaDialogManager.Instance.AnimatorTrigger("show");
                return true;
            }
            return false;
        }
    }
}