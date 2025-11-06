using System;
using System.Collections.Generic;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace UltraLib.Extensions.DictionaryExtensions;

[PublicAPI]
public static class DictionaryExtensions
{
    /// <summary>
    ///     Adds a key/value pair to the <see cref="T:System.Collections.Generic.IDictionary`2" /> by using the
    ///     specified function if the key doesn't already exist. Returns the new value, or the existing value if the key exists.
    /// </summary>
    /// <param name="dictionary">The dictionary to add the key/value pair to.</param>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="valueFactory">The function used to generate a value for the key.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///     <paramref name="key" /> or <paramref name="valueFactory" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="T:System.NotSupportedException">The dictionary is read-only.</exception>
    /// <returns>
    ///     The value for the key. This will be either the existing value for the key if the key is already
    ///     in the dictionary, or the new value if the key wasn't in the dictionary.
    /// </returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent | CollectionAccessType.None | CollectionAccessType.Read | CollectionAccessType.UpdatedContent)]
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueFactory)
    {
        if (dictionary.TryGetValue(key, out var value)) return value;
        value = valueFactory(key);
        dictionary[key] = value;
        return value;
    }
}