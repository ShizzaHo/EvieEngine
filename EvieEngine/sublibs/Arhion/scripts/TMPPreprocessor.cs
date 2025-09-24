using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace EvieEngine.Arhion.TMP
{
    public interface Ipreprocess
    {
        string Format(string text);
    }

    public class Preprocessor : ITextPreprocessor
    {
        private List<Ipreprocess> preprocessors;

        public Preprocessor()
        {
            preprocessors = FindAllImplementations<Ipreprocess>();
        }

        public string PreprocessText(string text)
        {
            string result = text;

            foreach (var preprocessor in preprocessors)
            {
                result = preprocessor.Format(result);
            }

            return result;
        }

        private List<T> FindAllImplementations<T>()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();

            var implementations = types
                .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .ToList();

            var instances = implementations
                .Select(type => (T)Activator.CreateInstance(type))
                .ToList();

            return instances;
        }
    }

    public class tmp_localizator : Ipreprocess
    {
        public string Format(string text)
        {
            var regex = new Regex(@"\{(.*?)\}");
            var result = regex.Replace(text, match =>
            {
                string key = match.Groups[1].Value;
                string localizedText = Localization.GetLocalizedText(key);
                return localizedText;
            });

            return result;
        }
    }
}
