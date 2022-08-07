using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MapDesign
{
    public class GameManager : MonoBehaviour
    {
        [Header("Map Properties")]

        [Tooltip("distance between two floors")]
        public float GroundDistance = 5f;

        [Tooltip("average path length")]
        public int MeanPath = 300;
        public int path;


        [Header ("Finish Design")]

        [Tooltip ("Finish Max height")]
        public int finishMaxHeight;

        [Tooltip ("Finish Min Height")]
        public int finishMinHeight;

        private Material FinishGroundMaterial;
        private List<Color> FinishColorList = new List<Color>();






    [Space(20f)]



        [Header("Barrier Properties")]

        [Tooltip("distance between obstacles")]
        public float BarrierDistance = 2f;




    [Space(20f)]



        [Header ("Mass Properties")]
        public float massDistance = 2f;
        public int massValue = 50;

        public List<GameObject> _massObjects = new List<GameObject>();





        [Space(20f)]




        [Header("EXP Properties")]

        [Tooltip("average number of 'exp' to be added to the scene")]
        public float EXPValue;

        [Tooltip("'exp' object to be added to the scene")]
        public GameObject ExpObject;
        public int ExpDistance;







        [Space(20f)]



        [Header("Objects")]
        
        [Tooltip("Barrier Object - This is the object that will prevent our character from progressing.")]
        public List<GameObject> BarrierObject = new List<GameObject>();

        private GameObject _barrierParentObject;
        


        [Tooltip("Mass Object - With the help of this object, our character will be able to avoid obstacles.")]
        public GameObject MassObject;



        [Tooltip("Ground Object - this object is the map floor")]
        public GameObject GroundObject;

        private List<GameObject> _barriers = new List<GameObject>();
        private List<GameObject> _grounds = new List<GameObject>();
        private List<GameObject> _finishObjects = new List<GameObject>();
        private List<GameObject> _expObjects = new List<GameObject>();

        [Space(10)]


        [Tooltip("Empty Object - We create an empty object to parent the objects in the scene")]
        public GameObject EmptyObject;




        private GameObject _groundParentObject; //ground parent object
        

        

        [HideInInspector] public bool barrierBool = false;


        private void Awake()
        {
            MapDesign();
        }




        private void Start()
        {
            
        }




        private void Update()
        {

        }



        ///<summary>
        ///Colors that finish floors can take
        ///</summary>
        public void ColorListAdd()
        {
            FinishColorList.Add(Color.blue);
            FinishColorList.Add(Color.cyan);
            FinishColorList.Add(Color.gray);
            FinishColorList.Add(Color.yellow);
            FinishColorList.Add(Color.red);
            FinishColorList.Add(Color.magenta);
        }




        ///<summary>
        ///function that designs the map when the game starts
        ///</summary>
        public void MapDesign()
        {
            ColorListAdd();
            GroundSystem();
            BarrierSystem();
            MassSystem();
            FinishDesign();
            ExpSystem();
        }




        ///<summary>
        ///Finish object Group
        ///<summary>
        public void CreateEmptyOBjectFinish()
        {
            if (_finishObjects.Count > 0)
            {
                GameObject _finishEmptyObect = Instantiate (EmptyObject, Vector3.zero, Quaternion.identity);
                _finishEmptyObect.name = "FinishObjects";

                foreach (GameObject _finishObject in _finishObjects)
                {
                    _finishObject.transform.SetParent(_finishEmptyObect.transform);
                }
            }
        }


        ///<summary>
        ///function that designs the finish part of the map
        ///<summary>
        public void FinishDesign()
        {
            int _finishHeight = Random.Range(finishMinHeight, finishMaxHeight);

            for (int i = 0; i < _finishHeight; i++)
            {
                GameObject _finishGround;
                var _finishPos = new Vector3 (0, i, (path * 5) - 5);

                _finishGround = Instantiate (GroundObject, _finishPos, Quaternion.identity);

                _finishPos.z += i*4;
                _finishGround.transform.position = _finishPos;


                FinishGroundMaterial = _finishGround.GetComponent<MeshRenderer>().material;
                int colorIndex = Random.Range(0, FinishColorList.Count - 1);
                FinishGroundMaterial.color = FinishColorList[colorIndex];

                BoxCollider _finishCollider;
                _finishCollider = _finishGround.GetComponent<BoxCollider>();
                _finishCollider.size = new Vector3(0.9f, 0.9f, 1f);

                _finishGround.tag = "barrier";

                _finishObjects.Add(_finishGround);
            }
            _finishObjects[_finishHeight - 1].GetComponent<BoxCollider>().size = new Vector3(5, 30, 0.2f);

            CreateEmptyOBjectFinish();
        }






        ///<summary>
        ///Mass object Group
        ///<summary>
        public void CreateEmptyObjectMass()
        {
            if (_massObjects.Count > 0)
            {
                GameObject emptyObject = Instantiate (this.EmptyObject, Vector3.zero, Quaternion.identity);
                emptyObject.name = "MassObjects";
                foreach (GameObject _massObject in _massObjects)
                {
                    _massObject.transform.SetParent (emptyObject.transform);
                }
            }
        }


        ///<summary>
        ///places 'mass' on the map
        ///</summary>
        public void MassSystem()
        {
            Vector3 _massPos = Vector3.zero;

            GameObject _newMass;
            int xAxis;
            for (int i = 1; i < path * 0.80f; i++)
            {
                xAxis = Random.Range (-2, 3);
                _massPos = new Vector3 (xAxis, 1, (massDistance * i) * 3);
                _newMass = Instantiate (MassObject, _massPos, Quaternion.identity);
                foreach(GameObject _barrier in _barriers)
                {
                    if (_barrier.transform.position.z == _newMass.transform.position.z)
                    {
                        Destroy(_newMass);
                        _newMass = null;
                        break;
                    }
                }

                if(_newMass != null)
                    _massObjects.Add(_newMass);
            }

            CreateEmptyObjectMass();
        }


        ///<summary>
        ///EXP object Group
        ///<summary>
        public void CreateEmptyObjectExp()
        {
            if(_expObjects.Count > 0)
            {
                GameObject emptyObject = Instantiate (this.EmptyObject, Vector3.zero, Quaternion.identity);
                emptyObject.name = "Exp_Objects";

                foreach(GameObject _exp in _expObjects)
                {
                    _exp.transform.SetParent(emptyObject.transform);
                }
            }
        }

        ///<summary>
        ///places 'exp's on the map
        ///</summary>
        public void ExpSystem()
        {
            Vector3 _expPos = Vector3.zero;

            GameObject _expObject;
            int xAxis;
            int _averageValue = Random.Range((int)(EXPValue - (EXPValue / 1.5f)), (int)(EXPValue + (EXPValue * 1.5f)));
            for(int i = 1; i < path * 0.80f; i++)
            {
                xAxis = Random.Range(-2, 3);
                _expPos = new Vector3(xAxis, 1, (ExpDistance * i) *2);
                _expObject = Instantiate (ExpObject, _expPos, Quaternion.identity);

                _expObjects.Add(_expObject);
            }

            CreateEmptyObjectExp();
        }



        ///<summary>
        ///Ground object Group
        ///<summary>
        private void CreateEmptyObjectGround()
        {
            if (_grounds.Count > 0)
            {
                GameObject emptyObject = Instantiate(this.EmptyObject, Vector3.zero, Quaternion.identity);
                emptyObject.name = "GroundObjects";

                foreach (GameObject ground in _grounds)
                {
                    ground.transform.SetParent(emptyObject.transform);
                    
                }
            }
        }



        ///<summary>
        ///creates the map background
        ///</summary
        public void GroundSystem()
        {
            path = Random.Range((MeanPath - 20), (MeanPath + 50)) / 5;

            Vector3 groundPos = Vector3.zero;

            for (int i = 0; i < path; i++)
            {
                GameObject _ground = Instantiate (GroundObject, groundPos, Quaternion.identity);
                groundPos = new Vector3(0,0, GroundDistance * i);

                _grounds.Add(_ground);
            }
            CreateEmptyObjectGround();
        }





        ///<summary>
        ///Barrier object Group
        ///<summary>
        private void CreateEmptyObjectBarrier()
        {
            if(_barriers.Count > 0)
            {
                GameObject emptyObject = Instantiate (this.EmptyObject, Vector3.zero, Quaternion.identity);
                emptyObject.name = "BarrierObjects";

                foreach (GameObject barrier in _barriers)
                {
                    barrier.transform.SetParent(emptyObject.transform);
                }
            }
        }



        ///<summary>
        ///adds barriers to the map
        ///</summary>
        public void BarrierSystem()
        {
            Vector3 _barrierPos = Vector3.zero;

            int barrierIndex;

            for (int i = 2; i < path / 4; i++)
            {
                barrierIndex = Random.Range(0, BarrierObject.Count);

                _barrierPos = new Vector3 (0, 1, (BarrierDistance * i) * 2);
                GameObject _newBarrier = Instantiate (BarrierObject[barrierIndex], _barrierPos, Quaternion.identity);

                _barriers.Add(_newBarrier);
            }

            CreateEmptyObjectBarrier();
        }

    }
}
