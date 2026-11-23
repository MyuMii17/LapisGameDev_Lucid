using System;
using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore;

[Serializable]
public class CameraPosition
{
    public Transform Position;
    public float Speed;
    public float Smooth;
    public bool CanMove;
    public Transform NextPosition;
}

[Serializable]
public class Movement
{
    [Header("Movement Setting")]
    public Vector3 MoveDirection;
    public float MovePower;
    public float WalkSpeed;
    public float RunMultiplier;
    public float SlideMultiplier;
    public float TotalJumpPower;
    public float JumpPower;
    public float WallJumpMultiplier;

    [Header("Rotation Setting")]
    public float RotationSpeed;
    public float RotationSmoothSpeed;

    [Header("Crouch Setting")]
    public float CurruntCrouchSizeY;
    public float CrouchSizeModifier;

    [Header("State Setting")]
    public bool IsWalking;
    public bool CanMove;
    public bool IsRunning;
    public bool IsJumping;
    public bool IsWalljump;
    public bool CanJump;
    public bool IsCrouching;
    public bool OnSlide;
    public bool CanSlide;
    public bool IsSliding;
    public bool IsGrounded;
}

[Serializable]
public class PushInteract
{
    public bool IsHold;
    public bool CanHold;
    public bool IsInvert;
    public bool IsAtLimit;
    public Vector3 PushDirection;
    public float RayRange;
    public float RayHeight;

    public bool IsPull;
    public float PullTime;
    public float PullInterval;
}
[Serializable]
public class PullObject
{
    public bool CanPullOnly;
    public bool IsEndPull;
    public float PullInterval;
}

[Serializable]
public class ClimbInteract
{
    public bool IsHold;
    public bool CanHold;
    public bool IsUp;
    public Vector3 WallDirection;
    public float RayRange;
    public float RayHeight;
}

[Serializable]
public class PickInteract
{
    public bool IsHold;
    public bool CanHold;
}

[Serializable]
public class ObjectInteract
{
    public bool IsHold;
    public bool CanHold;
}

[System.Serializable]
public class ItemSlots
{
    // [SerializeReference]
    // public ItemData item;
    public GameObject self;
    public Sprite sprite;
    public int count;
    public int maxStack;
}

[System.Serializable]
public class ItemCondition
{
    public bool CanIteract;
    public bool HasCondition;
    public bool CanDetect;
    public bool IsAdd;
    // public ItemData Item;
    public int Count;
    public int MaxStack;
    public float CircleRadius;
}

[System.Serializable]
public class CutsceneSetting
{
    public bool HasCutscene;
    public int CutsceneNumber;
}