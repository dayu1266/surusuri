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
    public class Police : GameObject
    {
        const float WalkSpeed = 3f;                 // 歩く速度
        const float RunSpeed = 7f;                  // 走るスピード
        const float MaxFallSpeed = 12f;             // 最大落下速度
        const int initialHp = 1;                    // 警備員のHP

        const int View = 180;                       // 視野

        int count;              // 猶予時間のカウント
        float Amount;           // 移動量
        float dontMoveFream;    // 動いてはいけない時間（単位：フレーム）
        public int hp;          // HP
        int randMove;           // 動く方向（ランダムで決定）
        int changecount;        // 動いている時間のカウント（歩くか止まるかをチェンジするためのカウント）

        bool floorUp;           // 上の階への移動
        bool floorDown;         // 下の階への移動
        bool beforeSearch;       // 階段を見つける前か

        Vector2 velocity;       // 移動速度
        Direction Direction;    // 移動方向

        PlayScene playScene;    // playSceneの参照
        Player player;          // playerの参照
        GameObject nearStair;  // 一番近い階段を保存する

        public Police(PlayScene playScene, Vector2 pos) : base(playScene)
        {
            this.playScene = playScene;
            player = playScene.player;

            this.pos.X = pos.X;
            this.pos.Y = pos.Y;

            imageWidth = 60;
            imageHeight = 140;

            hitboxOffsetLeft = 17;
            hitboxOffsetRight = 17;
            hitboxOffsetTop = 9;
            hitboxOffsetBottom = 9;
            beforeSearch = true;

            hp = initialHp;
            find = true;
            nearStair = null;

            playScene.gameObjects.Add(new Sight(playScene, this, pos));
        }
        public override void Update()
        {
            MoveX();
            MoveY();
            if (player.isHiding)
            {
                find = false;
            }
        }
        void MoveX()
        {
            if (floorUp) // 階段を上りたかったら
            {
                if (beforeSearch) // まだ階段を探してなかったら
                {
                    SeachUpStair(); // 1番近い上り階段を探す
                }
                if (pos.X < nearStair.pos.X) // 1番近い階段が自分より右にあったら
                {
                    velocity.X = WalkSpeed; // 右に進む
                    direction = Direction.Right;
                }
                else // それ以外は
                {
                    velocity.X = -WalkSpeed; // 左に進む
                    direction = Direction.Left;
                }
            }
            else if (floorDown) // 階段を下りたかったら
            {
                if (beforeSearch) // まだ階段を探す前だったら
                {
                    SeachDownStair(); // 1番近い下り階段を探す
                }
                if (pos.X < nearStair.pos.X)
                {
                    velocity.X = WalkSpeed;
                    direction = Direction.Right;
                }
                else
                {
                    velocity.X = -WalkSpeed;
                    direction = Direction.Left;
                }
            }
            else if (find)
            {
                if (pos.Y + 225 == player.pos.Y) // プレイヤーが1つ下の階にいたら
                {
                    floorDown = true;
                }
                else if (pos.Y - 223 == player.pos.Y) // 1つ上の階にいたら
                {
                    floorUp = true;
                }
                if (Math.Pow(playScene.player.pos.X - pos.X, 2) < 8) velocity.X = 0;
                else if (playScene.player.pos.X > pos.X)
                {
                    float prePosX = velocity.X; // 1フレーム前の速度を保存
                    velocity.X = RunSpeed;// 1フレーム前の速度より今の速度がのほうが速い、つまり振り向いた
                    direction = Direction.Right;
                }
                else
                {
                    float prePosX = velocity.X;
                    velocity.X = -RunSpeed;
                    direction = Direction.Left;
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
            if (floorUp && other is UpStairs) // 上の階に行きたくて、上り階段に当たったら
            {
                StairUp(); // 上る
            }
            if (floorDown && other is DownStairs) // 下の階に行きたくて、下り階段に当たったら
            {
                StairDown(); // 下る
            }
        }

        public void Die()
        {
            isDead = true;
        }

        public override void Draw()
        {
            Camera.DrawGraph(pos.X, pos.Y - 10.0f, Image.police[0]);
            #if DEBUG
            DX.DrawStringF(pos.X - Camera.cameraPos.X, pos.Y - Camera.cameraPos.Y - 12, pos.X.ToString() + "," + pos.Y.ToString(), DX.GetColor(255, 100, 255)); // デバッグ用座標表示
            #endif
        }

        public override void Buzzer()
        {
            if (pos.Y + 225 >= player.pos.Y) // プレイヤーが1つ下の階にいたら
            {
                floorDown = true;
            }
            else if (pos.Y - 223 <= player.pos.Y) // 1つ上の階にいたら
            {
                floorUp = true;
            }
        }
        void StairUp() // 階段を上る
        {
            pos.X += 160;
            pos.Y -= 224;
            floorUp = false;
        }
        void StairDown() // 階段を降りる
        {
            pos.X -= 160;
            pos.Y += 224;
            floorDown = false;
        }
        // 1番近い下り階段を探す
        void SeachDownStair()
        {
            for (int i = 0; i < playScene.gameObjects.Count(); i++)
            {
                if (playScene.gameObjects[i].GetType() == typeof(DownStairs))
                {
                    if (nearStair == null) // まだ階段をひとつも見つけていなかったら
                    {
                        nearStair = playScene.gameObjects[i]; // 最初に見つけた階段をセット
                    }
                    else if (Vector2.DistanceSquared(pos, nearStair.pos) > Vector2.DistanceSquared(pos, playScene.gameObjects[i].pos))
                    { // 今までに見つけた階段との距離より新しく見つけた階段との距離のほうが短かったら
                        nearStair = playScene.gameObjects[i]; // 一番近い階段を入れ替える
                    }
                }
                beforeSearch = false; // ループが終了したら検索完了
            }
        }
        // 1番近い上り階段を探す
        void SeachUpStair()
        {
            for (int i = 0; i < playScene.gameObjects.Count(); i++)
            {
                if (playScene.gameObjects[i].GetType() == typeof(UpStairs))
                {
                    if (nearStair == null) // まだ階段をひとつも見つけていなかったら
                    {
                        nearStair = playScene.gameObjects[i]; // 最初に見つけた階段をセット
                    }
                    else if (Vector2.DistanceSquared(pos, nearStair.pos) > Vector2.DistanceSquared(pos, playScene.gameObjects[i].pos))
                    { // 今までに見つけた階段との距離より新しく見つけた階段との距離のほうが短かったら
                        nearStair = playScene.gameObjects[i]; // 一番近い階段を入れ替える
                    }
                }
                beforeSearch = false; // ループが終了したら検索完了
            }
        }
    }
}
