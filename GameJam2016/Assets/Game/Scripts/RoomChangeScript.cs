using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using CCC.Manager;
using UnityEngine.UI;

public class RoomChangeScript : MonoBehaviour
{
    public Button leftbutton;
    public Button rightbutton;
    public Scene currentView;
    public Camera camera;
    public int speed = 10;
    public int offset = 10;

    private bool translateLeftOn = false;
    private bool translateRightOn = false;

    private Vector3 startPosition;

    public void Update()
    {
        if (translateLeftOn)
        {
            if(startPosition.x <= -1)
            {
                // Valeur absolue
                float newTransformPos;
                if (camera.transform.position.x < 0) newTransformPos = (-1 * camera.transform.position.x);
                else newTransformPos = camera.transform.position.x;

                if ((newTransformPos + startPosition.x) <= offset)
                {
                    camera.transform.Translate(Vector3.left * (speed * Time.deltaTime));
                }
                else
                {
                    camera.transform.position = new Vector3(startPosition.x-offset,camera.transform.position.y,camera.transform.position.z);
                    translateLeftOn = false;
                    leftbutton.interactable = true;
                    rightbutton.interactable = true;
                }
            } else
            {
                if(camera.transform.position.x <= -1)
                {
                    if ((-1 * (camera.transform.position.x - startPosition.x)) <= offset)
                    {
                        camera.transform.Translate(Vector3.left * (speed * Time.deltaTime));
                    }
                    else
                    {
                        camera.transform.position = new Vector3(startPosition.x - offset, camera.transform.position.y, camera.transform.position.z);
                        translateLeftOn = false;
                        leftbutton.interactable = true;
                        rightbutton.interactable = true;
                    }
                } else
                {
                    if ((startPosition.x - camera.transform.position.x) <= offset)
                    {
                        camera.transform.Translate(Vector3.left * (speed * Time.deltaTime));
                    }
                    else
                    {
                        camera.transform.position = new Vector3(startPosition.x - offset, camera.transform.position.y, camera.transform.position.z);
                        translateLeftOn = false;
                        leftbutton.interactable = true;
                        rightbutton.interactable = true;
                    }
                }
            }
        } else if (translateRightOn)
        {
            if (camera.transform.position.x <= -1)
            {
                // Valeur absolue
                float newTransformPos;
                if (startPosition.x < 0) newTransformPos = (-1 * startPosition.x);
                else newTransformPos = startPosition.x;

                if ((newTransformPos + camera.transform.position.x) <= offset)
                {
                    camera.transform.Translate(Vector3.right * (speed * Time.deltaTime));
                }
                else
                {
                    camera.transform.position = new Vector3(startPosition.x + offset, camera.transform.position.y, camera.transform.position.z);
                    translateRightOn = false;
                    leftbutton.interactable = true;
                    rightbutton.interactable = true;
                }
            }
            else
            {
                if(startPosition.x <= -1)
                {
                    if ((-1 *(startPosition.x - camera.transform.position.x)) <= offset)
                    {
                        camera.transform.Translate(Vector3.right * (speed * Time.deltaTime));
                    }
                    else
                    {
                        camera.transform.position = new Vector3(startPosition.x + offset, camera.transform.position.y, camera.transform.position.z);
                        translateRightOn = false;
                        leftbutton.interactable = true;
                        rightbutton.interactable = true;
                    }
                } else
                {
                    if ((camera.transform.position.x - startPosition.x) <= offset)
                    {
                        camera.transform.Translate(Vector3.right * (speed * Time.deltaTime));
                    }
                    else
                    {
                        camera.transform.position = new Vector3(startPosition.x + offset, camera.transform.position.y, camera.transform.position.z);
                        translateRightOn = false;
                        leftbutton.interactable = true;
                        rightbutton.interactable = true;
                    }
                }
            }
        }
    }

    public void ChangeRoom(int direction)
    {
        if (direction == -1)
        {
            ActivateLeftRoom(); // Create/Render Scene of the View to the left
            GotoLeftRoom(); // Rotate camera to the left
        }
        else if(direction == 1)
        {
            ActivateRightRoom(); // Create/Render Scene of the View to the Right
            GotoRightRoom(); // Rotate camera to the right
        }
    }

    void ActivateLeftRoom()
    {
        //RoomManager.ActivateView(RoomManager.FindNextView(currentView,-1));
    }

    void GotoLeftRoom()
    {
        // Initalisation de la translate vers la droite jusqu'a la prochaine salle
        translateLeftOn = true;
        startPosition = camera.transform.position;
        leftbutton.interactable = false;
        rightbutton.interactable = false;
    }

    void ActivateRightRoom()
    {
        //RoomManager.ActivateView(RoomManager.FindNextView(currentView, 1));
    }

    void GotoRightRoom()
    {
        // Initalisation de la translate vers la droite jusqu'a la prochaine salle
        translateRightOn = true;
        startPosition = camera.transform.position;
        leftbutton.interactable = false;
        rightbutton.interactable = false;
    }

    // TODO:
    // Fonction specifique au touch screen, positionnement de la camera change en fonction
    // de la position du doigt du joueur
}
