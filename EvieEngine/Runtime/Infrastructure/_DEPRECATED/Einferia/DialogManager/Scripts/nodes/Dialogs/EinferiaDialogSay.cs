using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace EvieEngine.Einferia.Nodes
{
    [UnitTitle("Say")]
    [UnitCategory("EinferiaDialogManager\\Dialogs")]
    public class EinfeariaDialogSay : WaitUnit
    {
        public ValueInput character;
        public ValueInput text;

        public ValueInput audioFileName;
        public ValueInput skipAfterAudio;

        public ValueInput typewriterEffect;

        //public ValueInput dubbing;

        protected override void Definition()
        {
            base.Definition();

            character = ValueInput<string>(nameof(character), "");
            text = ValueInput<string>(nameof(text), "");
            
            audioFileName = ValueInput<string>(nameof(audioFileName), "");
            skipAfterAudio = ValueInput<bool>(nameof(skipAfterAudio), false); 

            typewriterEffect = ValueInput<bool>(nameof(typewriterEffect), true);
        }

        protected override IEnumerator Await(Flow flow)
        {
            EinferiaDialogManager manager = EinferiaDialogManager.Instance;
            manager.defaultDialogName.text = flow.GetValue<string>(character);
            
            manager.ChangeDialogSkipVisible(false);

            if (flow.GetValue<bool>(typewriterEffect))
            {
                manager.typewriterEffect.Write(manager.defaultDialogText, manager.typewriterTime, flow.GetValue<string>(text));
            }
            else
            {
                manager.defaultDialogText.text = flow.GetValue<string>(text);
            }

            if (!string.IsNullOrEmpty(flow.GetValue<string>(audioFileName)))
            {
                manager.PlayDialogSound(flow.GetValue<string>(audioFileName));
            }

            while (true)
            {
                if (flow.GetValue<bool>(typewriterEffect))
                {
                    if (flow.GetValue<bool>(skipAfterAudio) && !manager.GetCurrentAudioSourceIsPlaying())
                    {
                        manager.typewriterEffect.Stop();
                        manager.defaultDialogText.text = flow.GetValue<string>(text);
                        manager.DialogSoundStopActive();
                        
                        yield return new WaitForSeconds(0.3f);
                        
                        break;
                    }
                    
                    if (!manager.typewriterEffect.isTypewriterWork)
                    {
                        manager.ChangeDialogSkipVisible(true);
                    }
                    
                    if (manager.typewriterEffect.isTypewriterWork)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            manager.typewriterEffect.Stop();
                            manager.defaultDialogText.text = flow.GetValue<string>(text);
                            manager.DialogSoundStopActive();
                        }
                    }
                    else if (Input.GetMouseButtonDown(0))
                    {
                        break;
                    }
                }
                else
                {
                    if (flow.GetValue<bool>(skipAfterAudio) && !manager.GetCurrentAudioSourceIsPlaying())
                    {
                        manager.DialogSoundStopActive();
                        
                        yield return new WaitForSeconds(0.3f);
                        
                        break;
                    }
                    
                    if (Input.GetMouseButtonDown(0))
                    {
                        break;
                    }
                }

                yield return null;
            }
            
            yield return new WaitForSeconds(0.05f);

            yield return exit;
        }
    }
}