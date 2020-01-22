using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using QimOLib;
using System.Numerics;

namespace sumisumo
{
    public class Guardman : GameObject
    {
        const float WalkSpeed = 3f;                 // 歩く速度
        const float RunSpeed = 9f;                  // 走るスピード
        const float MaxFallSpeed = 12f;             // 最大落下速度
        const int initialHp = 1;                    // 一般人のHP
        const int initialAmount = 200;              // 移動量のベース
        const int initialdontMoveFream = 3 * 60;    // 停止フレーム
        const int View = 130;                       // 視野

        int count;              // 猶予時間のカウント
        float Amount;           // 移動量
        float dontMoveFream;    // 動いてはいけない時間（単位：フレーム）
        public int hp;          // HP
        int randMove;           // 動く方向（ランダムで決定）
        int changecount;        // 動いている時間のカウント（歩くか止まるかをチェンジするためのカウント）

        Vector2 velocity;       // 移動速度
        Direction Direction;    // 移動方向

        PlayScene playScene;
        Player player;

        public Guardman(PlayScene playScene, Vector2 pos) : base(playScene)
        {
            this.playScene = playScene;

            this.pos.X = pos.X;
            this.pos.Y = pos.Y;

            imageWidth = 60;
            imageHeight = 140;

            hitboxOffsetLeft = 17;
            hitboxOffsetRight = 17;
            hitboxOffsetTop = 9;
            hitboxOffsetBottom = 9;

            hp = initialHp;
            Amount = initialAmount;
            dontMoveFream = 0;

            playScene.gameObjects.Add(new Sight(playScene, this, pos));
        }

        public override void Update()
        {
            MoveX();
            MoveY();

            // 警戒モードへの猶予時間
            if (alert)
            {
                count++;
            }
            else if (count < 5)
            {
                count = 0;
            }
            else
            {
                count = 0;
            }

            // 警戒モードへの移行
            if (count >= 1)
            {
                player = playScene.player;
                if (player.surinuke)
                {
                    if (alert) playScene.state = PlayScene.State.OnAlert;
                    count = 0;
                }
            }
        }

        void MoveX()
        {
            if (playScene.state == PlayScene.State.OnAlert)
            {
                if (Math.Pow(playScene.player.pos.X - pos.X, 2) < 8) velocity.X = 0;
                else if (playScene.player.pos.X > pos.X)

                {
                    float prePosX = velocity.X; // 1フレーム前の速度を保存
                    velocity.X = RunSpeed;
                    // 1フレーム前の速度より今の速度がのほうが速い
                    // つまり振り向いた

                }
                else
                {
                    float prePosX = velocity.X;
                    velocity.X = -RunSpeed;
                }
            }

            else
            {
                if (dontMoveFream <= 0)
                {
                    // 初期値代入
                    Amount = initialAmount;
                    dontMoveFream = initialdontMoveFream;

                    // ランダムで移動方向を決定
                    int tmp = randMove;
                    randMove = QimOLib.Random.Range(1, 3);
                    if (changecount == 0)
                    {
                        randMove = 2;
                    }
                    if (tmp != randMove && changecount != 0)
                    {

                    }

                    if (randMove == 1)
                    {
                        direction = Direction.Left;
                    }
                    else
                    {
                        direction = Direction.Right;
                    }


                    // 移動量を決定
                    int randAmount = QimOLib.Random.Range(1, 4);
                    Amount = initialAmount * randAmount;
                }

                changecount++;

                // Amount が0以上なら動く
                if (Amount > 0)
                {
                    velocity.X = WalkSpeed;
                    Amount -= velocity.X;
                    if (randMove == 1)
                    {
                        velocity.X *= -1;
                    }
                }
                else
                {
                    velocity.X = 0;
                    dontMoveFream--;
                }
            }

            // 横に移動する
            pos.X += velocity.X;

            // 当たり判定の四隅の座標を取得
            float left = GetLeft();
            float right = GetRight() - 0.01f;
            float top = GetTop();
            float middle = top + 32;
            float bottom = GetBottom() - 0.01f;

            // 左端が壁にめりこんでいるか？
            if (playScene.map.IsWall(left, top) ||    // 左上が壁か？
                playScene.map.IsWall(left, middle) || // 左中が壁か？
                playScene.map.IsWall(left, bottom))   // 左下が壁か？
            {
                float wallRight = left - left % Map.CellSize + Map.CellSize; // 壁の右端
                SetLeft(wallRight); // 一般人の左端を壁の右端に沿わす
            }
            // 右端が壁にめりこんでいるか？
            else if (
                playScene.map.IsWall(right, top) ||   // 右上が壁か？
                playScene.map.IsWall(right, middle) || // 右中が壁か？
                playScene.map.IsWall(right, bottom))  // 右下が壁か？
            {
                float wallLeft = right - right % Map.CellSize; // 壁の左端
                SetRight(wallLeft); // 一般人の右端を壁の左端に沿わす
            }
        }

        // 縦の移動処理
        void MoveY()
        {
            // 縦に移動する
            pos.Y += velocity.Y;

            // 着地したかどうか
            bool grounded = false;

            // 当たり判定の四隅の座標を取得
            float left = GetLeft();
            float right = GetRight() - 0.01f;
            float top = GetTop();
            float bottom = GetBottom() - 0.01f;

            // 上端が壁にめりこんでいるか？
            if (playScene.map.IsWall(left, top) || // 左上が壁か？
                playScene.map.IsWall(right, top))   // 右上が壁か？
            {
                float wallBottom = top - top % Map.CellSize + Map.CellSize; // 天井のy座標
                SetTop(wallBottom); // 一般人の頭を天井に沿わす
                velocity.Y = 0; // 縦の移動速度を0に
            }
            // 下端が壁にめりこんでいるか？
            else if (
                playScene.map.IsWall(left, bottom) || // 左下が壁か？
                playScene.map.IsWall(right, bottom))   // 右下が壁か？
            {
                grounded = true; // 着地した
            }

            if (grounded) // もし着地してたら
            {
                float wallTop = bottom - bottom % Map.CellSize; // 床のy座標
                SetBottom(wallTop); // 一般人の足元を床の高さに沿わす
                velocity.Y = 0; // 縦の移動速度を0に
            }
        }

        public override void OnCollision(GameObject other)
        {
        }

        public void Die()
        {
            isDead = true;
        }

        public override void Draw()
        {
            Camera.DrawGraph(pos.X, pos.Y, Image.guardman);
            #if DEBUG
            DX.DrawStringF(pos.X - Camera.cameraPos.X, pos.Y - Camera.cameraPos.Y - 12, pos.X.ToString() + "," + pos.Y.ToString(), DX.GetColor(255, 100, 255)); // デバッグ用座標表示
            #endif
        }
    }
}
