using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[Serializable]
public class Movement
{
    [Header("Movement Setting")]
    public Vector3 MoveDirection;
    public float MovePower;
    public float TotalJumpPower;
    public float WalkSpeed;
    public float RunMultipiler;
    public float JumpPower;
    public float WallJumpMultiplier;

    [Header("Rotation Setting")]
    public float RotationSpeed;
    public float RotationSmoothSpeed;

    [Header("State Setting")]
    public bool IsWalking;
    public bool IsRunning;
    public bool IsJumping;
    public bool IsWalljump;
    public bool CanJump;
    public bool IsClimbing;
    public bool IsGrounded;
}

[Serializable]
public class PushInteract
{
    public bool IsHold;
    public bool CanHold;
}

[Serializable]
public class ClimbInteract
{
    public bool IsHold;
    public bool CanHold;
    public Vector3 WallDirection;
    public float RayRange;
    public float RayHeight;
    public int ClimbLayer;
}
