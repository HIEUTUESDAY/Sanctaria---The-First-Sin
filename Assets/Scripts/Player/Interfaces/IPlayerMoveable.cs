using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerMoveable
{
    Vector2 HorizontalInput { get; set; }
    Vector2 VerticalInput { get; set; }
    Rigidbody2D RB { get; set; }
    bool IsFacingRight { get; set; }
    void TurnPlayer();
    void SetFacing(Vector2 moveInput);
}
