using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassScript : MonoBehaviour
{
    private bool takeable;                  //variable that determines whether an object can be held or not
    public int massIndex;

    public int massNumber;                  //A variable created to determine the position of the mass object

    private GameObject Player;              //Player Object

    private bool barrierBool;               //Barrier Object

    private GatherScript _gatherScript;     //script interacting with objects

    UISystem UIscript;                      //interface script

    private void Awake() 
    {
        Player = GameObject.FindGameObjectWithTag("Player");                    //Get script Player
        _gatherScript = GameObject.Find("Gather").GetComponent<GatherScript>(); //Get gather script
        UIscript = GameObject.Find("Canvas").GetComponent<UISystem>();          //Get ui script

    }

    private void Start() {
        TakeAble = false;
        barrierBool = GameObject.Find("GameManager").GetComponent<MapDesign.GameManager>().barrierBool = true; //While creating the map, a bug was being created. To switch to it, we make a bug fix with the bool variable.
    }




    ///<summary>
    ///Function used to make changes with the variable takeAble
    ///</summary
    public bool TakeAble
    {
        get { return takeable; }
        set { takeable = value; }
    }





    ///<summary>
    ///function that checks whether we can hold the object or not
    ///<param name="grab"> Is the object held?
    ///</summary>
    public void take(bool grab)
    {
        if(TakeAble == false)
        {
            if (grab)
            {
                TakeAble = true;
                this.gameObject.transform.SetParent(Player.transform);
                massIndex = 1;
            }
            else
            {
                this.transform.parent = null;
                massIndex = -1;
                Destroy(this.gameObject, 5f);
            }
        }
    }




    ///<summary>
    ///system that works when the game is lost
    ///</summary>
    public void GameOverSystem()
    {
        if(Player.transform.childCount < 2)
        {
            UIscript.MenuSystem();
        }
    }




    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "barrier" && barrierBool)
        {
            this.transform.parent = null;
            this.GetComponent<BoxCollider>().enabled = false;
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(transformCorotine());
            Destroy(this.gameObject, 3f);
            GameOverSystem();
        }
    }

    IEnumerator transformCorotine()
    {
        yield return new WaitForSecondsRealtime(0.4f);
        _gatherScript.Arrangement = true;
    }
}
