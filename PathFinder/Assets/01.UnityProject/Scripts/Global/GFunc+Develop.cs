using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static partial class GFunc
{
    #region Print log func
    [System.Diagnostics.Conditional("DEBUG_MODE")]
    public static void Log(object message)
    {
#if DEBUG_MODE
        Debug.Log(message);
#endif      // DEBUG_MODE
    }       // Log()

    [System.Diagnostics.Conditional("DEBUG_MODE")]
    public static void Log(object message, UnityEngine.Object context)
    {
#if DEBUG_MODE
        Debug.Log(message, context);
#endif      // DEBUG_MODE
    }

    [System.Diagnostics.Conditional("DEBUG_MODE")]
    public static void LogWarning(object message)
    {
#if DEBUG_MODE
        Debug.LogWarning(message);
#endif      // DEBUG_MODE
    }       // Log()
    #endregion      // Print log func

    #region Assert for debug
    [System.Diagnostics.Conditional("DEBUG_MODE")]
    public static void Assert(bool condition)
    {
#if DEBUG_MODE
        Debug.Assert(condition);
#endif      // DEBUG_MODE
    }       // Assert()

    [System.Diagnostics.Conditional("DEBUG_MODE")]
    public static void Assert(bool condition, object message)
    {
#if DEBUG_MODE
        Debug.Assert(condition, message);
#endif      // DEBUG_MODE
    }       // Assert()
    #endregion      // Assert for debug

    #region Vaild Func
    // ������Ʈ�� ��ȿ�� �˻�
    public static bool IsValid<T>(this T component_) where T : Component
    {
        Component convert_ = (Component)(component_ as Component);
        bool isInvalid = convert_ == null || convert_ == default;
        return !isInvalid;
    }

    // ������Ʈ�� ��ȿ�� �˻�
    public static bool IsValid(this GameObject obj_)
    {
        bool isInValid = (obj_ == null || obj_ == default);
        return !isInValid;
    }

    public static bool IsValid<T>(this List<T> list_)
    {
        bool isInValid = (list_ == null || list_ == default || list_.Count < 1);
        return !isInValid;
    }
    public static bool IsValid<T>(this List<T> list_, int index_)
    {
        bool isInValid = (list_.IsValid() == false) ||
            (index_ < 0 || index_ >= list_.Count);
        return !isInValid;
    }
    #endregion      // Vaild Func
}
