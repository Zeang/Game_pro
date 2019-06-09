using UnityEngine;
using System.Collections;
using System;

// 子弹类型枚举
public enum CharacterType
{
    Maoxian = 0,
    Cike,
    Shengtu
}

/// <summary>
/// 可序列化
/// </summary>
[Serializable]
public class Character : ScriptableObject
{

    public CharacterType characType = CharacterType.Maoxian;

    public GameObject fbx;
    public GameObject weapon;
    public AnimationClip walk_forward;
    public AnimationClip walk_backward;
    public AnimationClip walk_left;
    public AnimationClip walk_right;
    public AnimationClip walk_to_dye;
    public AnimationClip crouch_forward;
    public AnimationClip crouch_to_dye;
}
