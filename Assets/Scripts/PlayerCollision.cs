using UnityEngine;

public enum CollisionX { None, Left, Middle, Right }
public enum CollisionY { None, Up, Middle, Down, LowDown }
public enum CollisionZ { None, Forward, Middle, Backward }

public class PlayerCollision : MonoBehaviour
{
    private PlayerController playerController;
    private CollisionX _collisionX;
    private CollisionY _collisionY;
    private CollisionZ _collisionZ;

    public CollisionX CollisionX { get => _collisionX; set => _collisionX = value; }
    public CollisionY CollisionY { get => _collisionY; set => _collisionY = value; }
    public CollisionZ CollisionZ { get => _collisionZ; set => _collisionZ = value; }

    void Awake()
    {
        playerController = gameObject.GetComponent<PlayerController>();            
    }

    public void OnCharacterCollision(Collider collider)
    {
        CollisionX = GetCollisionX(collider);
        CollisionY = GetCollisionY(collider);
        CollisionZ = GetCollisionZ(collider);

        SetAnimatorByCollision(collider);
    }
    
    private CollisionX GetCollisionX(Collider collider)
    {
        Bounds characterControllerBounds = playerController.MyCharacterController.bounds;
        Bounds colliderBounds = collider.bounds;
        float minX = Mathf.Max(colliderBounds.min.x, characterControllerBounds.min.x);
        float maxX = Mathf.Min(colliderBounds.max.x, characterControllerBounds.max.x);
        float average = (minX+ maxX) / 2 - colliderBounds.min.x;
        CollisionX localColX;

        if(average > colliderBounds.size.x - 0.33f)
        {
            localColX = CollisionX.Right;
        }
        else if(average < 0.33)
        {
            localColX = CollisionX.Left;
        }
        else
        {
            localColX = CollisionX.Middle;
        }
        return localColX;
    }

    private CollisionY GetCollisionY(Collider collider)
    {        
        Bounds characterControllerBounds = playerController.MyCharacterController.bounds;
        Bounds colliderBounds = collider.bounds;
        float minY = Mathf.Max(colliderBounds.min.y, characterControllerBounds.min.y);
        float maxY = Mathf.Min(colliderBounds.max.y, characterControllerBounds.max.y);
        float average = (minY + maxY) / 2 - colliderBounds.min.y;
        CollisionY localColY;

        if (average > colliderBounds.size.y - 0.33f)
        {
            localColY = CollisionY.Up;
        }
        else if (average < 0.17f)
        {
            localColY = CollisionY.LowDown;
        }
        else if (average < 0.33f)
        {
            localColY = CollisionY.Down;
        }
        else
        {
            localColY = CollisionY.Middle;
        }
        
        return localColY;
    }

    private CollisionZ GetCollisionZ(Collider collider)
    {
        Bounds characterControllerBounds = playerController.MyCharacterController.bounds;
        Bounds colliderBounds = collider.bounds;
        float minZ = Mathf.Max(colliderBounds.min.z, characterControllerBounds.min.z);
        float maxZ = Mathf.Min(colliderBounds.max.z, characterControllerBounds.max.z);
        float average = (minZ + maxZ) / 2 - colliderBounds.min.z;
        CollisionZ localColZ;

        if (average > colliderBounds.size.z - 0.33f)
        {
            localColZ = CollisionZ.Forward;
        }
        else if (average < 0.33)
        {
            localColZ = CollisionZ.Backward;
        }
        else
        {
            localColZ = CollisionZ.Middle;
        }
        return localColZ;
    }

    private void SetAnimatorByCollision(Collider collider)
    {       
        if(CollisionZ == CollisionZ.Backward && CollisionX == CollisionX.Middle)
        {
            CollisionZBackward(collider);
        }
        else if(CollisionZ == CollisionZ.Middle)
        {
            CollisionZMiddle();
        }
        else
        {
            CollisionZForward();
        }        
    }

    private void CollisionZBackward(Collider collider)
    {
        switch (CollisionY)
        {
            case CollisionY.None:
                break;
            case CollisionY.Up:
                if (!playerController.IsRolling)
                {
                    Debug.Log("IdDeathUpper");
                    playerController.SetPlayerAnimator(playerController.IdDeathUpper);
                    GameManager.instance.IsGameOver = true;
                }
                break;
            case CollisionY.Middle:
                if (collider.CompareTag("TrainOn"))
                {
                    Debug.Log("IdDeathMovingTrain");
                    playerController.SetPlayerAnimator(playerController.IdDeathMovingTrain);
                    GameManager.instance.IsGameOver = true;
                }
                else if(!collider.CompareTag("Ramp"))
                {
                    Debug.Log("IdDeathBounce");
                    playerController.SetPlayerAnimator(playerController.IdDeathBounce);
                    GameManager.instance.IsGameOver = true;
                }
                break;
            case CollisionY.Down:
                Debug.Log("IdDeathLower");
                playerController.SetPlayerAnimator(playerController.IdDeathLower);
                GameManager.instance.IsGameOver = true;
                break;
            case CollisionY.LowDown:
                collider.enabled = false;
                playerController.SetPlayerAnimator(playerController.IdStumbleLow);
                break;
            default:
                break;
        }        
    }

    private void CollisionZMiddle()
    {
        switch (CollisionX)
        {
            case CollisionX.None:
                break;
            case CollisionX.Left:
                playerController.SetPlayerAnimator(playerController.IdStumbleSideLeft);
                playerController.UpdatePlayerPosition(playerController.PrevPosition);
                break;
            case CollisionX.Middle:
                break;
            case CollisionX.Right:
                playerController.SetPlayerAnimator(playerController.IdStumbleSideRight);
                playerController.UpdatePlayerPosition(playerController.PrevPosition);
                break;
            default:
                break;
        }      
    }

    private void CollisionZForward()
    {
        switch (CollisionX)
        {
            case CollisionX.None:
                break;
            case CollisionX.Left:
                playerController.SetPlayerAnimatorWithLayer(playerController.IdStumbleCornerLeft);
                break;
            case CollisionX.Middle:
                break;
            case CollisionX.Right:
                playerController.SetPlayerAnimatorWithLayer(playerController.IdStumbleCornerRight);
                break;
            default:
                break;
        }      
    }
}