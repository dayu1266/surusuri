using DxLibDLL;
using System.Numerics;

namespace sumisumo
{
    // 画面のスクロール量を管理するクラス。
    // 画像の描画機能を持つ。
    // スクロールの影響を受けるオブジェクトはこのクラスを通じて描画することで
    // スクロールを意識せずに描画を行うことができる。
    public static class Camera
    {
        // カメラの位置。
        // 画面左上のワールド座標を表す。
        public static Vector2 cameraPos;

        // 指定されたワールド座標が画面の中心に来るように、カメラの位置を変更する
        public static void LookAt(float targetX, float targetY)
        {
            cameraPos.X = targetX - Screen.Size.X / 2;
            cameraPos.Y = targetY - Screen.Size.Y / 2;
        }

        /// <summary>
        /// 画像を描画する。スクロールの影響を受ける。
        /// </summary>
        /// <param name="worldX">左端のx座標</param>
        /// <param name="worldY">上端のy座標</param>
        /// <param name="handle">画像ハンドル</param>
        /// <param name="flip">左右反転するならtrue, しないならfalse（反転しない場合は省略可）
        /// </param>
        public static void DrawGraph(float worldX, float worldY, int handle, bool flip = false)
        {
            if (flip) DX.DrawTurnGraphF(worldX - cameraPos.X, worldY - cameraPos.Y, handle, DX.TRUE);
            else DX.DrawGraphF(worldX - cameraPos.X, worldY - cameraPos.Y, handle, DX.TRUE);
        }

        /// <summary>
        /// 四角形（枠線のみ）を描画する
        /// </summary>
        /// <param name="left">左端</param>
        /// <param name="top">上端</param>
        /// <param name="right">右端</param>
        /// <param name="bottom">下端</param>
        /// <param name="color">色</param>
        public static void DrawLineBox(float left, float top, float right, float bottom, uint color)
        {
            DX.DrawBox(
                (int)(left      - cameraPos.X + 0.5f),
                (int)(top       - cameraPos.Y + 0.5f),
                (int)(right     - cameraPos.X + 0.5f),
                (int)(bottom    - cameraPos.Y + 0.5f),
                color,
                DX.FALSE);
        }
    }
}
