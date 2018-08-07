﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HouraiTeahouse.FantasyCrescendo.Characters {

public class StateTransitionAsset : ScriptableObject {

  public enum ConditionRequirement {
    TransitionIfAll,
    TransitionIfAny,
    TransitionIfNone
  }

  public BaseStateAsset SourceState;
  public BaseStateAsset DestinationState;
  public ConditionRequirement TransitionRequirement;
  public List<StateTransitionCondition> Conditions;
  public bool Muted;

  public State.Transition BuildTransition(State targetState) {
    Func<CharacterContext, bool>[] predicates = BuildPredicates(Conditions);
    switch (TransitionRequirement) {
      default:
      case ConditionRequirement.TransitionIfAll:
        return (context) => {
          if (Muted) return null;
          foreach (var predicate in predicates) {
            if (!predicate(context)) return null;
          }
          return targetState;
        };
      case ConditionRequirement.TransitionIfAny:
        return (context) => {
          if (Muted) return null;
          foreach (var predicate in predicates) {
            if (predicate(context)) return targetState;
          }
          return null;
        };
      case ConditionRequirement.TransitionIfNone:
        return (context) => {
          if (Muted) return null;
          foreach (var predicate in predicates) {
            if (predicate(context)) return null;
          }
          return targetState;
        };
    }
  }

  static Func<CharacterContext, bool>[] BuildPredicates(IEnumerable<StateTransitionCondition> conditions) {
    return conditions.Select(cond => cond.BuildPredicate()).Where(cond => cond != null).ToArray();
  }

  /// <summary>
  /// Checks if the transition is related to a given state.
  /// </summary>
  /// <param name="state">the state to check against</param>
  /// <returns>true if the state is involved in the transition, false otherwise.</returns>
  public bool Involves(StateAsset state) => SourceState == state || DestinationState == state;

  internal static StateTransitionAsset Create(StateAsset src, StateAsset dst) {
    Argument.NotNull(src);
    Argument.NotNull(dst);
    var transition = ScriptableObject.CreateInstance<StateTransitionAsset>();
    transition.SourceState = src;
    transition.DestinationState = dst;
    transition.Conditions = new List<StateTransitionCondition>();
    return transition;
  }

}

}
