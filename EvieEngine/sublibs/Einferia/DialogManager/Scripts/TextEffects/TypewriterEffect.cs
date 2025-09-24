using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace EvieEngine.Einferia.Effects
{
    public class TypewriterEffect : MonoBehaviour
    {
        [HideInInspector]
        public bool isTypewriterWork = false;

        public void Write(TMP_Text textMeshPro, float typingSpeed = 0.05f, string text = "")
        {
            StartCoroutine(WriteTypeEffect(textMeshPro, typingSpeed, text));
        }

        public void Stop()
        {
            isTypewriterWork = false;
            StopAllCoroutines();
        }

        private IEnumerator WriteTypeEffect(TMP_Text textMeshPro, float typingSpeed, string text)
        {
            string preFormattedText = text;

            preFormattedText = new Arhion.TMP.tmp_localizator().Format(preFormattedText);

            textMeshPro.text = "";

            int charIndex = 0;
            string currentText = "";
            isTypewriterWork = true;

            while (charIndex < preFormattedText.Length)
            {
                if (preFormattedText[charIndex] == '<')
                {
                    int tagEndIndex = preFormattedText.IndexOf('>', charIndex);
                    if (tagEndIndex != -1)
                    {
                        currentText += preFormattedText.Substring(charIndex, tagEndIndex - charIndex + 1);
                        charIndex = tagEndIndex + 1;
                        continue;
                    }
                }

                currentText += preFormattedText[charIndex];
                textMeshPro.SetText(currentText);
                charIndex++;

                yield return new WaitForSeconds(typingSpeed);
            }

            isTypewriterWork = false;
        }
    }
}