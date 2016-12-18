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
        // On se dirige vers une vue a gauche
        if (translateLeftOn)
        {
            if(startPosition.x <= -1)
            {
                // Valeur absolue
                float newTransformPos;
                if (camera.transform.position.x < 0) newTransformPos = (-1 * camera.transform.position.x);
                else newTransformPos = camera.transform.position.x;

                // Addition de start position qui est negative et la valeur absolue de la destination (ex: -5 a -10) 
                if ((newTransformPos + startPosition.x) <= offset)
                {
                    camera.transform.Translate(Vector3.left * (speed * Time.deltaTime));
                }
                else
                {
                    // Instructions a faire une fois que la camera est arrive a destination (pourrait etre mis dans une fonction car il y a repetition)
                    camera.transform.position = new Vector3(startPosition.x-offset,camera.transform.position.y,camera.transform.position.z);
                    translateLeftOn = false;
                    leftbutton.interactable = true;
                    rightbutton.interactable = true;
                }
            } else
            {
                if(camera.transform.position.x <= -1)
                {
                    // Valeur absolue de la soustraction entre la destination negative et la startPosition qui est positive (ex: de 1 a -4)
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
                    // Soustraction entre la startposition qui est positive et la destination qui est positive (ex: de 10 a 5)
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
        }
        // On se dirige vers une vue a Droite
        else if (translateRightOn)
        {
            if (camera.transform.position.x <= -1)
            {
                // Valeur absolue
                float newTransformPos;
                if (startPosition.x < 0) newTransformPos = (-1 * startPosition.x);
                else newTransformPos = startPosition.x;

                // Addition entre la valeur absolue de la position et une destination negative (ex: de 10 a -5)
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
                    // Valeur absolue de la soustraction entre une startposition negative et une destination positive (ex: -4 a 1)
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
                    // Soustraction entre une destination positive et une startposition positive (ex: de 5 a 10)
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

    // Changer de Piece
    public void ChangeRoom(int direction)
    {
        if (direction == -1)
        {
            ActivateLeftRoom(); // Create/Render Canvas of the View to the left
            GotoLeftRoom(); // Rotate camera to the left
        }
        else if(direction == 1)
        {
            ActivateRightRoom(); // Create/Render Canvas of the View to the Right
            GotoRightRoom(); // Rotate camera to the right
        }
    }

    // S'assure que les render pour la piece a gauche est tel qu'on le desire
    void ActivateLeftRoom()
    {
        //RoomManager.ActivateView(RoomManager.FindNextView(currentView,-1));
    }

    // Deplace la camera vers la piece a gauche de celle courante via l'update
    void GotoLeftRoom()
    {
        // Initalisation de la translate vers la droite jusqu'a la prochaine salle
        translateLeftOn = true;
        startPosition = camera.transform.position;
        leftbutton.interactable = false;
        rightbutton.interactable = false;
    }

    // S'assure que les render pour la piece a droite est tel qu'on le desire
    void ActivateRightRoom()
    {
        //RoomManager.ActivateView(RoomManager.FindNextView(currentView, 1));
    }

    // Deplace la camera vers la piece a gauche de celle courante via l'update
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
    // de la position du doigt du joueur mais se lock sur des vues si on souleve un doigt
}
