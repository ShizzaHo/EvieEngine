using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.Rendering.DebugUI;

namespace EvieEngine.Einferia.Nodes
{
    [UnitTitle("Answer")]
    [UnitCategory("EinferiaDialogManager\\Dialogs")]
    public class EinfeariaDialogAnswer : WaitUnit
    {
        public ValueInput AnswerList;
        public bool isAnswerSelected = false;
        [DoNotSerialize]
        private string selectedAnswer;
        public ValueOutput AnswerSelectedValue;

        protected override void Definition()
        {
            base.Definition();

            AnswerList = ValueInput<List<string>>(nameof(AnswerList), new List<string>());

            AnswerSelectedValue = ValueOutput<string>("Answer value", (flow) => selectedAnswer);

            isAnswerSelected = false;
        }

        protected override IEnumerator Await(Flow flow)
        {
            EinferiaDialogManager manager = EinferiaDialogManager.Instance;

            isAnswerSelected = false;
            selectedAnswer = null;
            
            manager.AnimatorTrigger("answerShow");

            manager.GenerateAnswerListUI(flow.GetValue<List<string>>(AnswerList), (string value) =>
            {
                selectedAnswer = value;
                isAnswerSelected = true;
            });

            yield return new WaitUntil(() => isAnswerSelected);
            
            manager.AnimatorTrigger("answerHide");

            manager.ClearAnswerListUI();

            yield return exit;
        }
    }
}