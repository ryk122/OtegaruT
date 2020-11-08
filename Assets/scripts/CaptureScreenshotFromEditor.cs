using UnityEditor;
using UnityEngine;

namespace djkusuha.Utility
{
    /// <summary>
    /// Unityエディタ上からGameビューのスクリーンショットを撮るEditor拡張
    /// </summary>
    public class CaptureScreenshotFromEditor : Editor
    {
        /// <summary>
        /// キャプチャを撮る
        /// </summary>
        /// <remarks>
        /// Edit > CaptureScreenshot に追加。
        /// HotKeyは Ctrl + Shift + F12。
        /// </remarks>
        [MenuItem("Edit/CaptureScreenshot #%F12")]
        private static void CaptureScreenshot()
        {
            // 現在時刻からファイル名を決定
            var filename = System.DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".png";
            // キャプチャを撮る
#if UNITY_2017_1_OR_NEWER
            ScreenCapture.CaptureScreenshot(filename); // ← GameViewにフォーカスがない場合、この時点では撮られない
#else
            Application.CaptureScreenshot(filename); // ← GameViewにフォーカスがない場合、この時点では撮られない
#endif
            // GameViewを取得してくる
            var assembly = typeof(EditorWindow).Assembly;
            var type = assembly.GetType("UnityEditor.GameView");
            var gameview = EditorWindow.GetWindow(type);
            // GameViewを再描画
            gameview.Repaint();

            Debug.Log("ScreenShot: " + filename);
        }
    }
}