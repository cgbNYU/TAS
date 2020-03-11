using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapinCameraController : MonoBehaviour
{
    
    #region Internal References
    private Transform _app;
    private Transform _view;
    private Transform _cameraBaseTransform;
    private Transform _cameraTransform;
    private Transform _cameraLookTarget;
    private Transform _avatarTransform;
    private Rigidbody _avatarRigidbody;
    float _standingToWalkingSlider = 0;
    private Transform _objectOfInterest;
    #endregion

    #region Public Tuning Variables

    public Vector3 avatarObservationOffset_Base;
    public float pitchGreaterLimit;
    public float pitchLowerLimit;
    public float fovAtUp;
    public float fovAtDown;
    public float followDistance_Base;
    public float verticalOffset_Base;
    public float AvoidRadius;

    #endregion
    
    #region Persistent Outputs
    //Positions
    private Vector3 _camRelativePostion_Auto;

    //Directions
    private Vector3 _avatarLookForward;

    //Scalars
    private float _followDistance_Applied;
    private float _verticalOffset_Applied;

    //States
    private enum CameraStates { Manual, Automatic, SlowTurn, AvoidObstacle, AvoidAndTurn, LookingAtObject }
    private CameraStates _currentState;
    #endregion
    
    private void Awake()
    {
        _app = GameObject.Find("Application").transform;
        _view = _app.Find("View");
        _cameraBaseTransform = _view.Find("CameraBase");
        _cameraTransform = _cameraBaseTransform.Find("Camera");
        _cameraLookTarget = _cameraBaseTransform.Find("CameraLookTarget");

        _avatarTransform = _view.Find("AIThirdPersonController");
        _avatarRigidbody = _avatarTransform.GetComponent<Rigidbody>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _currentState = CameraStates.Automatic;
    }

    // Update is called once per frame
    void Update()
    {
        _ComputeData();
        switch (_currentState)
        {
            case CameraStates.Manual:
                _ManualControl();
                _FollowAvatar();
                ClickUpCheck();
                break;
            case CameraStates.Automatic:
                _FollowAvatar();
                _LookAtAvatar();
                ObstacleCheck();
                ObjectOfInterestCheck();
                ClickDownCheck();
                break;
            case CameraStates.AvoidObstacle:
                //_FollowAvatar();
                //_LookAtAvatar();
                AvoidObstacle();
                ObjectOfInterestCheck();
                break;
            case CameraStates.SlowTurn:
                break;
            case CameraStates.AvoidAndTurn:
                break;
            case CameraStates.LookingAtObject:
                //_FollowAvatar();
                _LookAtObject();
                OutOfObjectAreaCheck();
                break;
            default:
                Debug.Log("Camera state broken");
                break;
        }
    }

    #region Calculations
    
    private void _ComputeData()
    {
        _avatarLookForward = Vector3.Normalize(Vector3.Scale(_avatarTransform.forward, new Vector3(1, 0, 1)));

        if (_Helper_IsWalking())
        {
            _standingToWalkingSlider = Mathf.MoveTowards(_standingToWalkingSlider, 1, Time.deltaTime * 3);
        }
        else
        {
            _standingToWalkingSlider = Mathf.MoveTowards(_standingToWalkingSlider, 0, Time.deltaTime);
        }

        float _followDistance_Walking = followDistance_Base;
        float _followDistance_Standing = followDistance_Base * 2;

        float _verticalOffset_Walking = verticalOffset_Base;
        float _verticalOffset_Standing = verticalOffset_Base * 4;

        _followDistance_Applied = Mathf.Lerp(_followDistance_Standing, _followDistance_Walking, _standingToWalkingSlider);
        _verticalOffset_Applied = Mathf.Lerp(_verticalOffset_Standing, _verticalOffset_Walking, _standingToWalkingSlider);
    }

    private void CalculateVerticalOffset()
    {
        
    }
    
    private void CalculateFollowDistance()
    {
        
    }

    private void CalculateAvoidDistance()
    {
        
    }

    private void CalculateTurnSpeed()
    {
        
    }
    
    #endregion

    #region StateSwitches

    private void ObjectOfInterestCheck()
    {
        Collider[] stuffInSphere = Physics.OverlapSphere(_avatarTransform.position, 5);

        bool objectOfInterestPresent = false;

        foreach (Collider col in stuffInSphere)
        {
            if (col.CompareTag("ObjectOfInterest"))
            {
                objectOfInterestPresent = true;
            }
        }

        if (objectOfInterestPresent)
        {
            _currentState = CameraStates.LookingAtObject;
        }
    }

    private void OutOfObjectAreaCheck()
    {
        Collider[] stuffInSphere = Physics.OverlapSphere(_avatarTransform.position, 5);

        bool objectOfInterestPresent = false;

        foreach (Collider col in stuffInSphere)
        {
            if (col.CompareTag("ObjectOfInterest"))
            {
                objectOfInterestPresent = true;
            }
        }

        if (!objectOfInterestPresent)
        {
            _currentState = CameraStates.Automatic;
        }
    }

    private void TurnCheck()
    {
        
    }

    private void ClickDownCheck()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _currentState = CameraStates.Manual;
        }
    }

    private void ClickUpCheck()
    {
        if (Input.GetMouseButtonUp(1))
        {
            _currentState = CameraStates.Automatic;
        }
    }

    private void ObstacleCheck()
    {
        //Make an overlapsphere around camera
        Collider[] obstacles = Physics.OverlapSphere(_cameraTransform.position, AvoidRadius);
        
        //Check if sphere hits any colliders
        if (obstacles.Length > 0)
        {
            //Send the state to avoid
            _currentState = CameraStates.AvoidObstacle;
        }
    }

    #endregion

    #region Camera Movement Functions
    
    private void _FollowAvatar()
    {
        _camRelativePostion_Auto = _avatarTransform.position;

        _cameraLookTarget.position = _avatarTransform.position + avatarObservationOffset_Base;
        _cameraTransform.position = _avatarTransform.position - _avatarLookForward * _followDistance_Applied + Vector3.up * _verticalOffset_Applied;
    }

    private void _LookAtAvatar()
    {
        _cameraTransform.LookAt(_cameraLookTarget);
    }
    
    private void _LookAtObject()
    {
        _cameraTransform.LookAt(_objectOfInterest);
    }
    
    private void _ManualControl()
    {
        Vector3 _camEulerHold = _cameraTransform.localEulerAngles;

        if (Input.GetAxis("Mouse X") != 0)
            _camEulerHold.y += Input.GetAxis("Mouse X");

        if (Input.GetAxis("Mouse Y") != 0)
        {
            float temp = _camEulerHold.x - Input.GetAxis("Mouse Y");
            temp = (temp + 360) % 360;

            if (temp < 180)
                temp = Mathf.Clamp(temp, 0, 80);
            else
                temp = Mathf.Clamp(temp, 360 - 80, 360);

            _camEulerHold.x = temp;
        }

        Debug.Log("The V3 to be applied is " + _camEulerHold);
        _cameraTransform.localRotation = Quaternion.Euler(_camEulerHold);
    }

    private void AvoidObstacle()
    {
        _cameraTransform.position += _cameraTransform.forward * AvoidRadius * Time.deltaTime;
        
        //Make an overlapsphere around camera
        Collider[] obstacles = Physics.OverlapSphere(_cameraTransform.position, AvoidRadius);

        if (obstacles.Length == 0)
        {
            _currentState = CameraStates.Automatic;
        }
    }

    #endregion

    #region Helpers

    private Vector3 _lastPos;
    private Vector3 _currentPos;
    private bool _Helper_IsWalking()
    {
        _lastPos = _currentPos;
        _currentPos = _avatarTransform.position;
        float velInst = Vector3.Distance(_lastPos, _currentPos) / Time.deltaTime;

        if (velInst > .15f)
            return true;
        else return false;
    }

    #endregion
}
