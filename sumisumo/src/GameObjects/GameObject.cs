using DxLibDLL;
using QimOLib;
using System.Numerics;

namespace sumisumo
{
    // ゲーム上に表示される物体の基底クラス。
    // プレイヤーや敵、アイテムなどはこのクラスを継承して作る。
    public abstract class GameObject
    {   
        public EnemyId EnemyId = EnemyId.Node;

        public Vector2 pos = new Vector2();         // プレイヤーのポジション
        public bool isDead = false;                 // 死んだ（削除対象）フラグ
        public bool Surizumi = false;               // スリをされたかどうか
        public bool suri = false;                   // スリができるかどうか（一般人が視界にいるかどうか）
        public bool surinuke_now = false;           // すり抜けの途中かどうか
        public bool surinuke_old = false;           // 1フレーム前のすり抜け情報
        public Direction direction;

        protected PlayScene playScene;  // PlaySceneの参照
        protected int imageWidth;       // 画像の横ピクセル数
        protected int imageHeight;      // 画像の縦ピクセル数
        protected int hitboxOffsetLeft   = 0; // 当たり判定の左端のオフセット
        protected int hitboxOffsetRight  = 0; // 当たり判定の右端のオフセット
        protected int hitboxOffsetTop    = 0; // 当たり判定の上端のオフセット
        protected int hitboxOffsetBottom = 0; // 当たり判定の下端のオフセット

        // コンストラクタ
        public GameObject(PlayScene playScene)
        {
            this.playScene = playScene;
        }

        // 当たり判定の左端を取得
        public virtual float GetLeft()
        {
            return pos.X + hitboxOffsetLeft;
        }

        // 左端を指定することにより位置を設定する
        public virtual void SetLeft(float left)
        {
            pos.X = left - hitboxOffsetLeft;
        }

        // 右端を取得
        public virtual float GetRight()
        {
            return pos.X + imageWidth - hitboxOffsetRight;
        }

        // 右端を指定することにより位置を設定する
        public virtual void SetRight(float right)
        {
            pos.X = right + hitboxOffsetRight - imageWidth;
        }

        // 上端を取得
        public virtual float GetTop()
        {
            return pos.Y + hitboxOffsetTop;
        }

        // 上端を指定することにより位置を設定する
        public virtual void SetTop(float top)
        {
            pos.Y = top - hitboxOffsetTop;
        }

        // 下端を取得する
        public virtual float GetBottom()
        {
            return pos.Y + imageHeight - hitboxOffsetBottom;
        }

        // 下端を指定することにより位置を設定する
        public virtual void SetBottom(float bottom)
        {
            pos.Y = bottom + hitboxOffsetBottom - imageHeight;
        }

        // 更新処理
        public abstract void Update();

        // 描画処理
        public abstract void Draw();

        // 当たり判定を描画（デバッグ用）
        public void DrawHitBox()
        {
            // 四角形を描画
            Camera.DrawLineBox(GetLeft(), GetTop(), GetRight(), GetBottom(), DX.GetColor(255, 0, 0));
        }

        // 他のオブジェクトと衝突したときに呼ばれる
        public abstract void OnCollision(GameObject other);

        // 画面内に映っているか？
        public virtual bool IsVisible()
        {
            return Math2D.RectRectIntersect(
                pos.X, pos.Y, pos.X + imageWidth, pos.Y + imageHeight,
                Camera.cameraPos.X, Camera.cameraPos.Y, Camera.cameraPos.X + Screen.Size.X, Camera.cameraPos.Y + Screen.Size.Y);
        }

        public void ChangeOnAlert()
        {
            playScene.state = PlayScene.State.OnAlert;
        }
    }
}
