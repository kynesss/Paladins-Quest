using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 250;

    private PlayerController player;
    private PlayerMovement movement;

    [HideInInspector] public bool canRotate = true;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        movement = FindObjectOfType<PlayerMovement>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if(!player.isTrading && canRotate)
            FollowPlayer();

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (player.isTrading || GameController.instance.isSkillTreeVisible || GameController.instance.isQuestLogVisible
                || GameController.instance.isGameOverPanelVisible || PauseMenuController.instance.isPaused)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                movement.SetPlayerPose();
                canRotate = false;
            }
            else if (DialogueMenu.instance.currentManager != null)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = false;
                movement.SetPlayerPose();
                canRotate = false;
            }
            else if (GameController.instance.isInventoryVisible)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                canRotate = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                movement.canMove = true;
                canRotate = true;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        //Debug.Log("CanRotate = " + canRotate);
        //Debug.Log("IsAlive = " + player.isAlive);
    }

    public void FollowPlayer()
    {
        if (!player.isAlive) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        player.transform.Rotate(Vector3.up, mouseX);
    }

}
