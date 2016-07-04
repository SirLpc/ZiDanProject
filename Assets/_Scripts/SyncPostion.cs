using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 描述：同步位置脚本
/// author： 
/// </summary>
public class SyncPostion : NetworkBehaviour
{
    #region ===字段===

    /// <summary>
    /// 平滑同步卡顿用的值
    /// </summary>
    [SerializeField]
    private float _syncRate;

    [SerializeField]
    private Transform _myTransform;

    /// <summary>
    /// 要在同步的参数（位置）
    /// </summary>
    [SyncVar]
    private Vector3 _syncPos;

    #endregion

    #region ===属性===

    #endregion

    #region ===Unity事件=== 快捷键： Ctrl + Shift + M /Ctrl + Shift + Q  实现

    private void FixedUpdate()
    {
        TransmitPos();
        LerpPositon();
    }

    #endregion

    #region ===方法===

    /// <summary>
    /// 客户端调用同步的方法
    /// </summary>
    [ClientCallback]
    private void TransmitPos()
    {
        //如果是自己的对象，就把自己的位置发送给服务器
        if (isLocalPlayer)
            CmdTransmitPosToServer(_myTransform.position);
    }

    /// <summary>
    /// 真正同步数据到服务器的Command方法
    /// </summary>
    /// <param name="pos"></param>
    [Command]
    private void CmdTransmitPosToServer(Vector3 pos)
    {
        _syncPos = pos;
    }

    /// <summary>
    /// 其它客户端上，同步本对象所属玩家控制后的位置
    /// </summary>
    private void LerpPositon()
    {
        //如果是自己的对象，就直接返回
        if (isLocalPlayer)
            return;

        //使用插值，更平滑
        var targetPos = Vector3.Lerp(_myTransform.position, _syncPos, Time.deltaTime * _syncRate);
        _myTransform.position = targetPos;
    }

    #endregion

}
