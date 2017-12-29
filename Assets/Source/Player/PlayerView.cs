using UnityEngine;

namespace HouraiTeahouse.FantasyCrescendo {

public class PlayerView : IStateView<PlayerState> {

  GameObject View;

  IStateView<PlayerState>[] ViewComponents;

  public PlayerView(PlayerConfig config) {
    var selection = config.Selection;
    var character = Registry.Get<CharacterData>().Get(selection.CharacterID);
    View = Object.Instantiate(character.Prefab);
    View.name = string.Format("Player {0} View ({1}, {2})",
                              config.PlayerID + 1, character.name,
                              selection.Pallete);

    PlayerUtil.DestroyAll(View, typeof(Collider));

    foreach (var component in View.GetComponentsInChildren<ICharacterComponent>()) {
      component.Initialize(config);
    }

    ViewComponents = View.GetComponentsInChildren<IStateView<PlayerState>>();
  }

  public void ApplyState(PlayerState state) {
    foreach (var component in ViewComponents) {
      component.ApplyState(state);
    }
  }

}

}