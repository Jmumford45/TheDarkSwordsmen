using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollisions
{
    private Transform feet;
    private LayerMask ground;
    private Transform wallCheck;
    private float checkRadius;
    private float wallCheckDist;

   public CheckCollisions(Transform colPoint)
    {
        feet = colPoint;
        ground = LayerMask.GetMask("Ground");
        checkRadius = 0.2f;
        wallCheckDist = 0.2f;

        Debug.LogWarning("Collision class has been instantiated");
        Debug.LogWarning(feet.position);
        Debug.LogWarning(feet.name);
    }

    public CheckCollisions(Transform groundCol, Transform walCol)
    {
        feet = groundCol;
        wallCheck = walCol;
        ground = LayerMask.GetMask("Ground");

        checkRadius = 0.2f;
        wallCheckDist = 0.2f;

        Debug.LogWarning("Collision class has been instantiated");
        Debug.LogWarning(wallCheck.name);
        Debug.LogWarning(feet.name);
    }

    public bool GCollision()
    {
        bool groundCol;
        groundCol = Physics2D.OverlapCircle(feet.position, checkRadius, ground);

       //Debug.Log(groundCol);

        return groundCol;
    }

    public bool WCollision()
    {
        bool wallCol;
        wallCol = Physics2D.Raycast(wallCheck.position, wallCheck.right, wallCheckDist, ground);

        Debug.DrawRay(wallCheck.position, wallCheck.right, Color.blue);
        //Debug.Log(wallCol);

        return wallCol;
    }
}
