using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera mainCam;
    Transform playerPos;
    private float _camSpd;

    [SerializeField] private float xDistanceFromCam;
    [SerializeField] private float yDistanceFromCam;
    [SerializeField] private float _deadzoneX;
    [SerializeField] private float _deadzoneY;

    private void Start()
    {
        mainCam = Camera.main;
        GameObject temp = GameObject.FindWithTag("Player");
        playerPos = temp.transform;
        _camSpd = temp.GetComponent<PlayerController>().getSpeed();
        mainCam.transform.position = new Vector3(playerPos.position.x, playerPos.position.y, mainCam.transform.position.z);
    }

    //Think about splitting all 4 directions into own deadzones and camer speed follow based on x or y follow
    private IEnumerator Follow(Transform _pos)
    {
        float step = (_camSpd * 2f) * Time.deltaTime;
        float newPosX = playerPos.position.x;
        float newPosY = playerPos.position.y;
        float camZPos = mainCam.transform.position.z;
        Vector3 newPos = new Vector3(newPosX, newPosY, camZPos);

        mainCam.transform.position = Vector3.MoveTowards(mainCam.transform.position , newPos, step);
        yield return null;
    }

    //check the player position relative to the origin point of the camera (where it would stop when not moving).
    //if the player is a greater distance away on either the x or y axis than specified call camera follow (greater than DeadZonebounds)
    private void DeadZone()
    {
        xDistanceFromCam = Mathf.Abs(playerPos.position.x - mainCam.transform.position.x);
        yDistanceFromCam = Mathf.Abs(playerPos.position.y - mainCam.transform.position.y);

        if (xDistanceFromCam > _deadzoneX || yDistanceFromCam > _deadzoneY)
        {
            StartCoroutine(Follow(playerPos));
        }
    }

    //add recenter behaviour if the player pushes a button or doesn't move for x seconds move to the current player position 

    private void Update()
    {
        DeadZone();
    }
}
