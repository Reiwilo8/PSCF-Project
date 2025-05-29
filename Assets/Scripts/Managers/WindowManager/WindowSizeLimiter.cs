#if UNITY_STANDALONE_WIN
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class WindowSizeLimiter : MonoBehaviour
{
    [DllImport("user32.dll")]
    static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
        int X, int Y, int cx, int cy, uint uFlags);

    private const uint SWP_NOMOVE = 0x0002;
    private const uint SWP_NOZORDER = 0x0004;

    struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    void Start()
    {
        IntPtr handle = GetActiveWindow();

        int minWidth = 540;
        int minHeight = 960;

        RECT rect;
        GetClientRect(handle, out rect);

        int width = rect.right - rect.left;
        int height = rect.bottom - rect.top;

        if (width < minWidth || height < minHeight)
        {
            SetWindowPos(handle, IntPtr.Zero, 0, 0,
                Mathf.Max(width, minWidth),
                Mathf.Max(height, minHeight),
                SWP_NOMOVE | SWP_NOZORDER);
        }
    }
}
#endif
