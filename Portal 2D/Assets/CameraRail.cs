using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRail : MonoBehaviour {

    /*
     * Created by Jackson Benfer
     * 
     *  Camera Tracker for rail movement. This camera will move horizontlaly at a constant speed, but still track the player vertically.
     *  
     *  TODO: Allow tracker to change modes
     * 
     * 
     * 
     * 
     * 
     * 
     */

    public Transform player;


    public Transform ground;
    public Transform rightMost;
 

    public float speed;
    public float offsetY;
    public float frameOffsetX;
    public float frameOffsetY;


    private Transform trans;


    private float minimumX;
    private float maximumX;
    private float minimumY;
    private Vector3 target;

    private Transform rail;


	void Start () {

        minimumX = ground.position.x + frameOffsetX;
        minimumY = ground.position.y + frameOffsetY;

        maximumX = rightMost.position.x - frameOffsetX;


        trans = GetComponent<Transform>();

        float targetX, targetY;

        if (rail == null)
        {
            if (player.position.x > minimumX)
                targetX = player.position.x;
            else
                targetX = minimumX;
        }
        else
        {
            targetX = rail.position.x;
        }

        if (player.position.y > minimumY)
            targetY = player.position.y;
        else
            targetY = minimumY;



        trans.position = new Vector3(targetX, targetY, trans.position.z);

    }
	


	void Update () {

        float targetX, targetY;

   //   minimumX = -10;

        if (rail == null)
        {
            if (player.position.x < minimumX)
                targetX = minimumX;
            else if (player.position.x > maximumX)
                targetX = maximumX;
            else
                targetX = player.position.x;
        }
        else
        {
            targetX = rail.position.x;
        }

        if (player.position.y > minimumY)
            targetY = player.position.y;
        else
            targetY = minimumY;




        //TODO: just move towards it


        float dx = targetX - trans.position.x;
        float dy = targetY - trans.position.y;

        float d = Mathf.Sqrt(dx * dx + dy * dy);

        if (d < speed)
            trans.position = new Vector3(targetX, targetY, trans.position.z);
        else
        {

            float vx = dx / d * speed;
            float vy = dy / d * speed;



            trans.position = new Vector3(trans.position.x + vx, trans.position.y + vy, trans.position.z);
        }
        
    }


    public void setRail(Transform rail)
    {
        this.rail = rail;
    }

}
