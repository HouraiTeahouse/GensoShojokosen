using UnityEngine;
using UnityEngine.Assertions;

namespace HouraiTeahouse.FantasyCrescendo.Matches {

/// <summary>
/// An default IMatchController implementation that runs a normal local game.
/// </summary>
public class MatchController : IMatchController {

  public virtual uint Timestep { get; set; }
  public virtual MatchState CurrentState { get; set; }
  public virtual ISimulation<MatchState, MatchInputContext> Simulation { get; set; }
  public virtual IInputSource<MatchInput> InputSource { get; set; }

  public virtual void Update() {
    if (CurrentState.StateID == MatchProgressionState.Intro) return;

    var input = new MatchInputContext {
      Previous = CurrentState.LastInput,
      Current = InputSource.SampleInput()
    };
    
    var state = CurrentState;
    Simulation.Simulate(ref state, input);
    CurrentState = state;

    Timestep++;
  }

  public virtual void Dispose() => Simulation?.Dispose();

}

}