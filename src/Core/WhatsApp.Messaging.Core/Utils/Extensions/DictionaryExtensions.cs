//Copyright (c) 2025 Rodrigo de Freitas Oliveira
//Todos os direitos reservados.
//Uso, modificação e distribuição proibidos sem autorização.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsApp.Messaging.Core.Utils.Extensions
{
    using Dict = System.Collections.Generic.IDictionary<string, string>;
    using RDict = System.Collections.Generic.IReadOnlyDictionary<string, string>;

    public static class DictionaryExtensions
    {
        public static string? GetValueOrDefault(this RDict dict, string key, string? defaultValue = null) => dict != null && key != null && dict.TryGetValue(key, out var v) ? v : defaultValue;

        public static TValue GetOrAdd<TKey, TValue>(this System.Collections.Generic.IDictionary<TKey, TValue> dict, TKey key, System.Func<TKey, TValue> factory)
        {
            if (dict.TryGetValue(key, out var existing)) return existing!;
            var created = factory(key);
            dict[key] = created;
            return created;
        }

        public static void AddOrUpdate(this Dict dict, string key, System.Func<string> addFactory, System.Func<string, string> updateFactory)
        {
            if (dict.ContainsKey(key))
                dict[key] = updateFactory(dict[key]);
            else
                dict[key] = addFactory();
        }

        public static Dict Merge(this RDict? left, RDict? right, bool overrideRight = true)
        {
            var result = new System.Collections.Generic.Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);
            if (left != null)
                foreach (var kv in left) result[kv.Key] = kv.Value;
            if (right != null)
                foreach (var kv in right)
                    if (overrideRight || !result.ContainsKey(kv.Key)) result[kv.Key] = kv.Value;
            return result;
        }

        public static Dict With(this RDict? dict, string key, string value)
        {
            var result = new System.Collections.Generic.Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);
            if (dict != null)
                foreach (var kv in dict) result[kv.Key] = kv.Value;
            result[key] = value;
            return result;
        }
    }
}



