﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HouraiTeahouse.FantasyCrescendo {

/// <summary>
/// A type-safe global object registry.
/// </summary>
/// <remarks> 
/// Only supports types that implement IIdentifiable.
/// Objects are referred to by their IDs, and the ID for a given
/// object is assumed to be globally unique.
/// </remarks>
public static class Registry {

  static Dictionary<Type, object> Registries;

  static Registry() {
    Registries = new Dictionary<Type, object>();
  }

  /// <summary>
  /// Retrieves or creates a new registry for given type.
  /// </summary>
  /// <returns>the retrieved registry for the type.</returns>
  public static Registry<T> Get<T>() where T : IIdentifiable {
    object storedObject = null;
    Registries.TryGetValue(typeof(T), out storedObject);
    var registry = storedObject as Registry<T>;
    if (registry == null) {
      registry = new Registry<T>();
      Registries.Add(typeof(T), registry);
    }
    return registry;
  }

  /// <summary>
  /// Clears all registries of all types and the values they store.
  /// </summary>
  public static void ClearAll() {
    Registries.Clear();
  }

}

public class Registry<T> : ICollection<T> where T : IIdentifiable {

  readonly Dictionary<uint, T> Entries;

  internal Registry() {
    Entries = new Dictionary<uint, T>();
  }

  public int Count {
    get { return Entries.Count; }
  }

  public bool IsReadOnly {
    get { return false; }
  }

  public T Get(uint id) {
    return Entries[id];
  }

  public void Add(T obj) {
    Entries[obj.Id] = obj;
  }

  public bool Remove(T obj) {
    return Entries.Remove(obj.Id);
  }

  public bool Contains(T obj) {
    return Entries.ContainsKey(obj.Id);
  }

  public void Clear() {
    Entries.Clear();
  }

  public void CopyTo(T[] array, int start) {
    Entries.Values.CopyTo(array, start);
  }

  public IEnumerator<T> GetEnumerator() {
    return Entries.Values.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }

}

}

