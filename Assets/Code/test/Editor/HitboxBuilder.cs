﻿using HouraiTeahouse.FantasyCrescendo;
using UnityEngine;  

public class HitboxBuilder {

  public readonly Hitbox Hitbox;

  public HitboxBuilder(Hitbox box) {
    Hitbox = box;
  }

  public HitboxBuilder AsPlayer(uint playerID) {
    Hitbox.PlayerID = playerID;
    return this;
  }

  public HitboxBuilder WithKnockbackScaling(float knockbackScaling) {
    Hitbox.KnockbackScaling = knockbackScaling;
    return this;
  }

  public HitboxBuilder WithBaseKnockback(float baseKnockback) {
    Hitbox.BaseKnockback = baseKnockback;
    return this;
  }

  public HitboxBuilder WithKnockbackAngle(float knockbackAngle) {
    Hitbox.KnockbackAngle = knockbackAngle;
    return this;
  }

  public HitboxBuilder WithHitstunScaling(float hitstunScaling) {
    Hitbox.HitstunScaling = hitstunScaling;
    return this;
  }

  public HitboxBuilder WithBaseHitstun(uint baseHitstun) {
    Hitbox.BaseHitstun = baseHitstun;
    return this;
  }

  public HitboxBuilder WithOffset(Vector3 offset) {
    Hitbox.Offset = offset;
    return this;
  }

  public HitboxBuilder WithMirrorDirection(bool mirror) {
    Hitbox.MirrorDirection = mirror;
    return this;
  }

  public HitboxBuilder WithPosition(Vector3 position) {
    Hitbox.transform.position = position;
    return this;
  }

  public HitboxBuilder WithType(HitboxType type) {
    Hitbox.Type = type;
    return this;
  }

  public HitboxBuilder WithRadius(float radius) {
    Hitbox.Radius = radius;
    return this;
  }
  
  public HitboxBuilder WithEnabled(bool enabled) {
    Hitbox.enabled = enabled;
    return this;
  }
  
  public HitboxBuilder WithActive(bool active) {
    Hitbox.gameObject.SetActive(active);
    return this;
  }

  public HitboxBuilder WithPriority(uint priority) {
    Hitbox.Priority = priority;
    return this;
  }

  public Hitbox Build() => Hitbox;

}
