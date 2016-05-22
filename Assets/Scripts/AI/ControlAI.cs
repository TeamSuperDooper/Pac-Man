using UnityEngine;
using System.Collections;

public class ControlAI : MonoBehaviour {
    private float speed = 0.24f;
    //private float slomoSpeed = 0.01f;
    Vector3 _dest;
    Vector3 _dir = Vector3.zero;
    Vector3 _nextDir = Vector3.zero;
    Vector3 _lastDir = new Vector3(1f, 0, 0);
    //Vector3 _currentDir = new Vector3(-1f, 0, 0);
    private Vector3 DIRECTION_LEFT = new Vector3(0, 0, 1f);
    private Vector3 DIRECTION_RIGHT = new Vector3(0, 0, -1f);
    private Vector3 DIRECTION_UP = new Vector3(1f, 0, 0);
    private Vector3 DIRECTION_DOWN = new Vector3(-1f, 0, 0);

    [SerializeField]
    private GameObject spotLight;

    private GameObject[] floors;
    private GameObject[] walls;

    private const string PELLET_NAME = "Pellet(Clone)";
    private const string WALL_NAME = "Wall(Clone)";
    private const string PACMAN_NAME = "Pacman";

    private Rigidbody rigidBody;
    bool lastMoveUp;
    bool lastMoveDown;
    bool lastMoveLeft;
    bool lastMoveRight;

    void Awake() {
        _dest = transform.position;
    }

    void Start() {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        MoveBasedOnPosition();
    }

    bool Valid(Vector3 direction) {
        // cast line from 'next to pacman' to pacman
        // not from directly the center of next tile but just a little further from center of next tile
        Vector3 pos = transform.position;
        //Debug.DrawLine(pos + direction, pos, Color.red, 5, true);
        direction += new Vector3(direction.x, 0, direction.z);
        //Debug.DrawRay(pos + direction, pos);
        //Debug.DrawLine(pos + direction, pos, Color.green, 1, true);
        //Debug.Log(Physics.Linecast(pos + direction, pos));

        RaycastHit hit;
        Physics.Linecast(pos + direction, pos, out hit);
        //Debug.Log("Hit: " + hit.ToString());
        //Debug.Log("Collider null? ");
        //Debug.Log("Collider null? " + (hit.collider == null));

        if (hit.collider != null) {
            if (hit.collider.name != null) {
                //Debug.Log("collider name null? " + (hit.collider.name == null));
                //Debug.Log(hit.collider.name);

                //return (hit.collider.name != WALL_NAME && hit.collider.name != PACMAN_NAME);
                return hit.collider.name == PELLET_NAME || hit.collider == GetComponent<Collider>();
            }
        }

        return false;
    }

    void MoveBasedOnPosition() {
        Vector3 p = Vector3.MoveTowards(transform.position, _dest, speed);

        //Rotation
        //Debug.Log(_lastDir + ", " + _nextDir);
        if (_lastDir != _nextDir) {
            Vector3 t = transform.localRotation.eulerAngles;

            if(_nextDir == DIRECTION_LEFT) {
                t = new Vector3(0, 450, 0);
            } else if(_nextDir == DIRECTION_RIGHT) {
                t = new Vector3(0, 270, 0);
            } else if(_nextDir == DIRECTION_UP) {
                t = new Vector3(0, 180, 0);
            } else if(_nextDir == DIRECTION_DOWN) {
                t = new Vector3(0, 360, 0);
            }

            transform.localRotation = Quaternion.Euler(t.x, t.y, t.z);
        }
        //Debug.Log("Current direction: " + _currentDir);
        GetComponent<Rigidbody>().MovePosition(p);

        _lastDir = _nextDir;

        if(tag == "Ghost") {
            if (Input.GetAxis("Horizontal2") > 0) _nextDir = DIRECTION_RIGHT;
            if (Input.GetAxis("Horizontal2") < 0) _nextDir = DIRECTION_LEFT;
            if (Input.GetAxis("Vertical2") > 0) _nextDir = DIRECTION_UP;
            if (Input.GetAxis("Vertical2") < 0) _nextDir = DIRECTION_DOWN;
        } else {
            if (Input.GetAxis("Horizontal") > 0) _nextDir = DIRECTION_RIGHT;
            if (Input.GetAxis("Horizontal") < 0) _nextDir = DIRECTION_LEFT;
            if (Input.GetAxis("Vertical") > 0) _nextDir = DIRECTION_UP;
            if (Input.GetAxis("Vertical") < 0) _nextDir = DIRECTION_DOWN;
        }
        //_dest = transform.position + _nextDir;
        // if pacman is in the center of a tile


        //Debug.DrawLine(_dest, transform.position, Color.red, 2, true);

        if (Vector3.Distance(_dest, transform.position) < 0.00001f) {
            if (Valid(_nextDir)) {
                _dest = transform.position + _nextDir;
                _dir = _nextDir;
            }
            else   // if next direction is not valid
            {
                if (Valid(_dir))  // and the prev. direction is valid
                    _dest = (Vector3)transform.position + _dir;   // continue on that direction

                // otherwise, do nothing
            }
        }

        /*Vector3 movementDirection = transform.position;

        if (Input.GetAxis("Horizontal") > 0) {
            movementDirection.z -= 0.18f;
            clearMoveFlags(false,false,false,true);
        } else if (Input.GetAxis("Horizontal") < 0) {
            movementDirection.z += 0.18f;
            clearMoveFlags(false, false, true, false);
        }
        else if (Input.GetAxis("Vertical") > 0) {
            movementDirection.x += 0.18f;
            clearMoveFlags(true, false, false, false);
        }
        else if (Input.GetAxis("Vertical") < 0) {
            movementDirection.x -= 0.18f;
            clearMoveFlags(false, true, false, false);
        } else {
            if(lastMoveRight) {
                movementDirection.z -= 0.18f;
            } else if (lastMoveLeft) {
                movementDirection.z += 0.18f;
            } else if(lastMoveUp) {
                movementDirection.x += 0.18f;
            } else if(lastMoveDown) {
                movementDirection.x -= 0.18f;
            }
        }

        transform.position = movementDirection;*/
    }

    // void Update() {
    //Vector3 spotTemp = spotLight.transform.position;
    //spotLight.transform.position = new Vector3(transform.position.x, spotTemp.y, transform.position.z);
    //}

    void clearMoveFlags(bool up, bool down, bool left, bool right) {
        lastMoveUp = up;
        lastMoveDown = down;
        lastMoveLeft = left;
        lastMoveRight = right;
    }

    public Vector2 getDir() {
        return _dir;
    }
}
