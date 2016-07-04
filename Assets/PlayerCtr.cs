using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCtr : NetworkBehaviour
{
    #region ===字段、属性===

    private PlayerState _state;

    private Rigidbody _body;

    private bool _isRegistered;

    private NetworkBehaviour _pickedPlayer;

    private Vector3 _direction;
    private float _speed;

    private Vector2 _fingerStartPos;
    private Vector2 _fingerEndndPos;
    private float _fingerStartTime;
    private float _fingerEndTime;

    #endregion

    #region ===Unity事件=== 快捷键： Ctrl + Shift + M /Ctrl + Shift + Q  实现

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _state = PlayerState.IDEL;
        if(!_isRegistered)
            RegisterActions(true);
    }

    private void FixedUpdate()
    {
        if (_state == PlayerState.MOVING && _body.IsSleeping())
        {
            Debug.Log(_body.velocity + " ang " + _body.angularVelocity);
            _state = PlayerState.IDEL;
            Debug.Log("stoped!!!!!");
        }
    }

    private void OnEnable()
    {
        RegisterActions(true);
    }

    private void OnDisable()
    {
        RegisterActions(false);
    }

    private void OnDestroy()
    {
        RegisterActions(false);
    }

    #endregion

    #region ===方法===

    [ClientCallback]
    private void RegisterActions(bool reg)
    {
        if (!isLocalPlayer)
            return;

        if (reg)
        {
            EasyTouch.On_DragStart += On_DragStart;
            EasyTouch.On_Drag += On_Drag;
            EasyTouch.On_DragEnd += On_DragEnd;
        }
        else
        {
            EasyTouch.On_DragStart -= On_DragStart;
            EasyTouch.On_Drag -= On_Drag;
            EasyTouch.On_DragEnd -= On_DragEnd;
        }
        _isRegistered = reg;
    }

    private void On_DragStart(Gesture gesture)
    {
        if (_state != PlayerState.IDEL) return;
        if (gesture.pickedObject == null) return;
        _pickedPlayer = gesture.pickedObject.GetComponent<PlayerCtr>();
        if (_pickedPlayer == null) return;
        if (!_pickedPlayer.isLocalPlayer) return;

        _fingerStartPos = gesture.startPosition;
        _fingerStartTime = Time.time;
        _state = PlayerState.SELECTED;
    }

    private void On_Drag(Gesture gesture)
    {
    }

    private void On_DragEnd(Gesture gesture)
    {
        if (_state != PlayerState.SELECTED) return;

        _fingerEndndPos = gesture.position;
        _fingerEndTime = Time.time;
        CalcSpeedAndDirection();
        Move();
    }

    private void Move()
    {
        _state = PlayerState.MOVING;
        _body.AddForce(_direction * _speed);
    }

    private void CalcSpeedAndDirection()
    {
        var starPos = new Vector3(_fingerStartPos.x, 0, _fingerStartPos.y);
        var endPos = new Vector3(_fingerEndndPos.x, 0, _fingerEndndPos.y);
        var dis = Vector3.Distance(starPos, endPos);
        var deltaTime = _fingerEndTime - _fingerStartTime;
        _direction = Vector3.Normalize(endPos - starPos);
        _speed = dis/deltaTime;
    }

    #endregion

}
