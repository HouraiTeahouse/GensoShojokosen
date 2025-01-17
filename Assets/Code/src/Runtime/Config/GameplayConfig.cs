﻿using HouraiTeahouse.FantasyCrescendo.Matches;
using HouraiTeahouse.Attributes; 
using System;
using UnityEngine;

namespace HouraiTeahouse.FantasyCrescendo {

[CreateAssetMenu(menuName = "Config/Gameplay Config")]
public class GameplayConfig : ScriptableObject {

	[SerializeField] [Type(typeof(IInputSource<MatchInput>), CommonName="InputSource")] 
	string inputSource;

	[SerializeField] float _maxLedgeHangTime = 10;
	[SerializeField] float _ledgeGrabCooldown = 0.5f;

	public short MaxLedgeHangTime => (short)(_maxLedgeHangTime / Time.fixedDeltaTime);
	public short LedgeGrabCooldown => (short)(_ledgeGrabCooldown / Time.fixedDeltaTime);

	public IInputSource<MatchInput> CreateInputSource(MatchConfig config) {
		var sourceType = Type.GetType(inputSource);
		return (IInputSource<MatchInput>)Activator.CreateInstance(sourceType, config);
	}

}

}