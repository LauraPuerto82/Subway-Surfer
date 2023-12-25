using UnityEngine;

public class SceneController : MonoBehaviour
{
    GameObject player;
    PlayerController myPlayerController;

    Vector3 originalPosition;

    void Start()
    {        
        player = GameObject.Find("Player");
        originalPosition = new Vector3(0,player.transform.position.y, 0);        
        myPlayerController = player.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            myPlayerController.ResetPosition(originalPosition);
        }        
    }    
}