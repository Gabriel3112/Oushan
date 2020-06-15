using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public GameObject ghost;
    public float delayToInstance;
    public float currentDelayTime;
    public bool isGhost;


    // Start is called before the first frame update
    void Start()
    {
        currentDelayTime = delayToInstance;
    }

    // Update is called once per frame
    void Update()
    {
        InstanceGhost();
    }

    public void InstanceGhost()
    {
        if (isGhost)
        {
            if(currentDelayTime > 0)
            {

                currentDelayTime -= Time.deltaTime;
            }
            else
            {
                GameObject ghostInstance = Instantiate(ghost, transform.position, transform.rotation);
                Sprite spFuture = GetComponent<SpriteRenderer>().sprite;
                Player player = GetComponent<Player>();
                ghostInstance.GetComponent<SpriteRenderer>().flipX = player.flip;
                ghostInstance.GetComponent<SpriteRenderer>().sprite = spFuture;
              
                currentDelayTime = delayToInstance;
                Destroy(ghostInstance, 1f);
            }
        }
    }
}
