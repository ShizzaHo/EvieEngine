using Unity.VisualScripting;
using UnityEngine;

namespace EvieEngine.Einferia.Nodes
{
    [UnitTitle("Stop Dialog")]
    [UnitCategory("EinferiaDialogManager\\Control")]
    public class EinfeariaDialogStop : Unit
    {
        [DoNotSerialize]
        public ControlInput inputTrigger;

        [DoNotSerialize]
        public ControlOutput outputTrigger;

        protected override void Definition()
        {
            //Making the ControlInput port visible, setting its key and running the anonymous action method to pass the flow to the outputTrigger port.
            inputTrigger = ControlInput("inputTrigger", (flow) =>
            {
                EinferiaDialogManager.Instance.AnimatorTrigger("hide");
                return outputTrigger;
            });
            //Making the ControlOutput port visible and setting its key.
            outputTrigger = ControlOutput("outputTrigger");
        }
    }
}