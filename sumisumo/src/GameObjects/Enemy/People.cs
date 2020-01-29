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
    enum PeopleState
    {
        Normal,
        Escape,
    }
    public class People : GameObject
    {
        const float WalkSpeed = 2f;                 // 歩きの速度
        const float MaxFallSpeed = 12f;             // 最大落下速度
        const int initialHp = 1;                    // 一般人のHP
        const int initialAmount = 200;              // 移動量のベース
        const int initialdontMoveFream = 3 * 60;    // 停止フレーム
        const int View = 130;                       // 視野
        const int turnInterval = 90;                // 回転するまでのインターバル（単位：）

        float Speed;            // スピード
        float Amount;           // 移動量
        float dontMoveFream;    // 動いてはいけない時間（単位：フレーム）
        float turnFream;        // ターンするまでの時間（単位：フレーム）
        public int hp;          // HP
        int randMove;           // 動く方向
        int changecount;        // 動いている時間のカウント（歩くか止まるかをチェンジするためのカウント）
        int turnCounter;        // 
        int turn = 0;           // 
        int floor;              // 今何階にいるか
        bool beforeSearch;      // 逃走経路検索前か
        GameObject nearStair;   // 一番近い階段

        public bool see_player = false; // 視界内にプレイヤーがいるかどうか
        int count = 0;

        PeopleState state;      // 一般人のステート
        Vector2 velocity;   // 移動速度
        Direction Direction;  // 移動方向

        Player player;

        public People(PlayScene playScene, Vector2 pos) : base(playScene)
        {
            this.pos.X = pos.X;
            this.pos.Y = pos.Y;

            imageWidth = 60;
            imageHeight = 140;

            hitboxOffsetLeft = 17;
            hitboxOffsetRight = 17;
            hitboxOffsetTop = 9;
            hitboxOffsetBottom = 3;

            Speed = WalkSpeed;
            hp = initialHp;
            Amount = initialAmount;
            dontMoveFream = 0;
            turnFream = 0;
            state = PeopleState.Normal;
            beforeSearch = true;

            playScene.gameObjects.Add(new Sight(playScene, this, pos));
            nearStair = null;
        }

        public override void Update()
        {
            if (see_player) count = 0;
            else if (!see_player) count++;

            // 警戒モードへの移行
            if (see_player || count <= 3)
            {
                player = playScene.player;
                if (player.surinuke)
                {
                    playScene.StateChange(PlayScene.State.OnAlert);
                    Suri_toSee();
                    see_player = false;
                }
                else
                {
                    see_player = false;
                }
            }

            // まず横に動かす
            MoveX();
            // 次に縦に動かす
            MoveY();

            // OnAlertだと
            if (playScene.state == PlayScene.State.OnAlert) turnFream++;
        }

        void MoveX()
        {
            if (state == PeopleState.Escape) // 逃走モードなら
            {
                if (beforeSearch) // 逃走経路検索前だったら
                {
                    RouteSeach(); // 一番近い階段を探す
                }
                if (pos.X > nearStair.pos.X) // 一番近い階段が自分より左にあったら
                {
                    velocity.X = -Speed; // 左に進む
                    direction = Direction.Left;
                }
                else // それ以外は右に進む
                {
                    velocity.X = Speed;
                    direction = Direction.Right;
                }
            }
            else
            {
                if (turnFream > turnInterval)
                {
                    //TurnAround(); 
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
                        velocity.X = Speed;
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
                velocity.X *= -1;
                direction = Direction.Right;
            }
            // 右端が壁にめりこんでいるか？
            else if (
                playScene.map.IsWall(right, top) ||   // 右上が壁か？
                playScene.map.IsWall(right, middle) || // 右中が壁か？
                playScene.map.IsWall(right, bottom))  // 右下が壁か？
            {
                float wallLeft = right - right % Map.CellSize; // 壁の左端
                SetRight(wallLeft); // 一般人の右端を壁の左端に沿わす
                velocity.X *= -1;
                direction = Direction.Left;
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

        void TurnAround()
        {
            turnCounter++;
            if (turnCounter > 20)
            {
                turn++;
                turnCounter = 0;
            }
            if (turn >= 2)
            {
                turn = 0;
                turnFream = 0;
            }
        }

        public override void OnCollision(GameObject other)
        {
            // 逃げ状態で階段に当たったら死ぬ
            if (state == PeopleState.Escape && (other is UpStairs || other is DownStairs))
            {
                isDead = true;

            }
        }

        public override void Draw()
        {
<<<<<<< HEAD
            // 左右反転するか？（左向きなら反転する）
            bool flip = direction == Direction.Left;

            Camera.DrawGraph(pos.X, pos.Y, Image.people,flip);
            #if DEBUG
=======
            Camera.DrawGraph(pos.X, pos.Y, Image.people);
#if DEBUG
>>>>>>> 2e63bec1c60b002c8086917c08fb0475bcd3ed8a
            DX.DrawStringF(pos.X - Camera.cameraPos.X, pos.Y - Camera.cameraPos.Y - 12, pos.X.ToString() + "," + pos.Y.ToString(), DX.GetColor(255, 100, 255)); // デバッグ用座標表示
#endif
        }
        public override void Buzzer()
        {
            if (pos.Y - playScene.player.pos.Y == -7) // プレイヤーと同じ高さにいたら
            {
                state = PeopleState.Escape; // 逃げる
            }
        }

        public void Suri_toSee()
        {
            state = PeopleState.Escape;
            Speed = 6.0f;
            Sound.SePlay(Sound.se_scream_woman);
        }

        void RouteSeach()
        {
            for (int i = 0; i < playScene.gameObjects.Count(); i++)
            {
                if (playScene.gameObjects[i].GetType() == typeof(UpStairs)
                    || playScene.gameObjects[i].GetType() == typeof(DownStairs))
                {
                    if (nearStair == null) // まだ階段をひとつも見つけていなかったら
                    {
                        nearStair = playScene.gameObjects[i]; // 最初に見つけた階段をセット
                    }
                    else if (Vector2.DistanceSquared(pos, nearStair.pos) > Vector2.DistanceSquared(pos, playScene.gameObjects[i].pos)
                           && pos.Y * 2 > playScene.gameObjects[i].pos.Y && pos.Y > playScene.gameObjects[i].pos.Y)
                    { // 今までに見つけた階段との距離より新しく見つけた階段との距離のほうが短かったら
                        nearStair = playScene.gameObjects[i]; // 一番近い階段を入れ替える
                    }
                    //else if (Vector2.DistanceSquared(pos, nearStair.pos) > Vector2.DistanceSquared(pos, playScene.gameObjects[i].pos))
                    //{ // 今までに見つけた階段との距離より新しく見つけた階段との距離のほうが短かったら
                    //    nearStair = playScene.gameObjects[i]; // 一番近い階段を入れ替える
                    //}
                }
                beforeSearch = false; // ループが終了したら検索完了
            }
        }
    }
}
