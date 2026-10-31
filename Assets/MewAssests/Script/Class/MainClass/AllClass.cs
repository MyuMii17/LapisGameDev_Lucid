using System;
using UnityEngine;

[Serializable]
public class Movement
{
    public float RotationSpeed;
    public float RotationSmoothSpeed;
    public Vector3 MoveDirection;
    public float MovePower;
    public float WalkSpeed;
    public float RunMultipiler;
    public float JumpPower;
    public bool IsWalking;
    public bool IsRunning;
    public bool IsJumping;
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
