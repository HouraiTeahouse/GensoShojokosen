﻿using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace HouraiTeahouse.FantasyCrescendo {

public abstract class Match {

  public async Task<MatchResult> RunMatch(MatchConfig config, bool loadScene = true) {
    await DataLoader.LoadTask.Task;
    Task sceneLoad = Task.CompletedTask;
    if (loadScene) {
      var stage = Registry.Get<SceneData>().Get(config.StageID);
      Assert.IsTrue(stage != null && stage.IsStage);
      await stage.GameScene.LoadAsync();
    }
    var additionalScenes = Config.Get<SceneConfig>().AdditionalStageScenes;
    await Task.WhenAll(additionalScenes.Select(s => s.LoadAsync(LoadSceneMode.Additive)));
    var gameManager = Object.FindObjectOfType<MatchManager>();
    gameManager.Config = config;
    await InitializeMatch(gameManager, config);
    return await gameManager.RunMatch();
  }

  protected abstract Task InitializeMatch(MatchManager manager, MatchConfig config);

}

public class DefaultMatch : Match {

  protected override async Task InitializeMatch(MatchManager gameManager, MatchConfig config) {
    var gameView = new GameView();
    var gameSim = CreateSimulation(config);
    var controller = new GameController(config);

    controller.CurrentState = CreateInitialState(config);
    controller.InputSource = new InControlInputSource(config);
    controller.Simulation = gameSim;

    gameManager.MatchController = controller;
    gameManager.View = gameView;
    gameManager.enabled = false;

    var simTask = gameSim.Initialize(config).ContinueWith(task => {
      Debug.Log("Simulation initialized.");
    });
    var viewTask = gameView.Initialize(config).ContinueWith(task => {
      Debug.Log("View initialized.");
    });

    await Task.WhenAll(viewTask, simTask);
    controller.CurrentState = gameSim.ResetState(controller.CurrentState);
    gameManager.enabled = true;
    Debug.Log("Match initialized.");
  }

  IMatchSimulation CreateSimulation(MatchConfig config) {
    var ruleSimulation = new MatchRuleSimulation(MatchRuleFactory.CreateRules(config));
    var playerSimulation = new MatchPlayerSimulation();
    return new MatchSimulation(new IMatchSimulation[] { playerSimulation, ruleSimulation });
  }

  MatchState CreateInitialState(MatchConfig config) {
    var initialState = new MatchState(config);
    for (int i = 0; i < initialState.PlayerStates.Length; i++) {
      initialState.PlayerStates[i].Position = new Vector3(i * 2 - 3, 1, 0);
    }
    return initialState;
  }
}

}