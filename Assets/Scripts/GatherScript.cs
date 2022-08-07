using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherScript : MonoBehaviour
{
    GameObject Player;
    GameObject GatherObject;
    public List<Transform> myMassObjects = new List<Transform>();

    [HideInInspector] private int massNumber = 0;
    [HideInInspector] public bool Arrangement = false;

    UISystem uiScript;

    AudioSource massSoundEffect;



    private void Awake() {
        Player = GameObject.FindGameObjectWithTag("Player");
        GatherObject = GameObject.Find("Gather");
        uiScript = GameObject.Find("Canvas").GetComponent<UISystem>();
        massSoundEffect = GameObject.Find("mass_sound").GetComponent<AudioSource>();
    }

    private void Start() {
        myMassListRefresh();
    }

    public void myMassListRefresh()
    {
        myMassObjects.Clear();
        foreach (Transform myMass in Player.transform)
        {
            if(myMass.tag == "mass")
            {
                myMassObjects.Add(myMass);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "mass" && other.GetComponent<MassScript>().TakeAble == false)
        {
            MassScript _massScript;
            _massScript = other.gameObject.GetComponent<MassScript>();
            _massScript.take(true);
            massNumber++;
            other.GetComponent<MassScript>().massNumber = massNumber;
            TransformSystem(other.gameObject);
            massSoundEffect.Play();
        }


        if(other.tag == "exp")
        {
            int expValue = Random.Range(3, 11);
            expValue = expValue * 5;
            uiScript.ExpSystem(expValue);
            Destroy(other.gameObject);
        }
    }


   


    private void Update() 
    {
        if (Arrangement)
            massArrangement();
    }


    ///<summary>
    ///Function used to get other 'mass' objects back in line when 'mass' objects get stuck in an obstacle
    ///</summary
    public void massArrangement()
    {
        myMassListRefresh();

        var playerPos = Player.transform.position;
        playerPos.y = myMassObjects.Count + 1.5f;
        Player.transform.localPosition = playerPos;

        var gatherPos = GatherObject.transform.position;
        gatherPos.y = 1;
        GatherObject.transform.position = gatherPos;

        for (int i = 0; i < myMassObjects.Count; i++)
        {
            Transform massTransform;
            massTransform = myMassObjects[i].transform;
            var pos = massTransform.position;
            pos.y = i + 1;
            massTransform.GetComponent<Transform>().transform.position = pos;
        }
        
        
        Arrangement = false;
    }






    ///<summary>
    ///As the "mass" object increases, it will be added under the "Player" object in a regular order.
    ///<param name = "other"> added 'mass' object
    ///</summary>
    public void TransformSystem (GameObject other)
    {
        MassScript _massScript;
        _massScript = other.GetComponent<MassScript>();
        var pos = Player.transform.localPosition;
        pos.y += _massScript.massIndex;
        Player.transform.localPosition = pos;
        
        pos = other.transform.position; 
        pos = new Vector3(Player.transform.position.x, _massScript.massIndex, Player.transform.position.z);
        other.transform.position = pos;

        pos = GatherObject.transform.position;
        pos.y = _massScript.massIndex;
        GatherObject.transform.position = pos;
    }
}
