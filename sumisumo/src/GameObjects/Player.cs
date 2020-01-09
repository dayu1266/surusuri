using DxLibDLL;
using QimOLib;
using System.Numerics;

namespace sumisumo
{
    // プレイヤークラス
    public class Player : GameObject
    {
        // 状態種別
        public enum State
        {
            Walk, // 歩き, 立ち
            Jump, // ジャンプ中
        }

        const float WalkSpeed = 6f;   // 歩きの速度
        const float Gravity = 0.6f; // 重力
        const float MaxFallSpeed = 12f;  // 最大落下速度
        const int initSurinukeLock = 90;  // 最大落下速度

        const int initialHp = 3;

        Vector2 velocity = Vector2.Zero; // 移動速度
        State state = State.Walk;        // 現在の状態
        
        public int curMoney;                // 所持金
        int surinukeLock;                   // すり抜けができるかできないか
        Direction tmp = Direction.Right;

        int floor = 1;      // 今いる階層
        int floorMax = 3;   // 最上層
        int fllorMin = 1;   // 最下層

        public int hp = 3;
        
        public Player(PlayScene playScene, Vector2 pos) : base(playScene)
        {
            this.pos.X = pos.X;
            this.pos.Y = pos.Y;

            direction = Direction.Right; // 向いている方向

            imageWidth = 60;
            imageHeight = 140;
            hitboxOffsetLeft = 17;
            hitboxOffsetRight = 17;
            hitboxOffsetTop = 9;
            hitboxOffsetBottom = 10;

            curMoney = 0;

            hp = initialHp;
            surinukeLock = initSurinukeLock;

            playScene.gameObjects.Add(new Sight(playScene, this, pos));
        }

        public override void Update()
        {
            // すり抜け中ではない
            surinuke_now = false;

            // 入力を受けての処理
            HandleInput();

            // 重力による落下
            velocity.Y += Gravity;
            if (velocity.Y > MaxFallSpeed) velocity.Y = MaxFallSpeed;

            // まず横に動かす
            MoveX();
            // 次に縦に動かす
            MoveY();

            // すり抜けができない時間
            surinukeLock--;

            if(direction != tmp)
            {
                tmp = direction;
            }
        }

        // 入力を受けての処理
        void HandleInput()
        {
            if (Input.GetButton(DX.PAD_INPUT_LEFT) && Input.GetButton(DX.PAD_INPUT_RIGHT))
            { // 左が押されてたら、速度を左へ
                velocity.X = 0;
            }
            else if (Input.GetButton(DX.PAD_INPUT_LEFT))
            { // 左が押されてたら、速度を左へ
                velocity.X = -WalkSpeed;
                direction = Direction.Left; // 左向きにする
            }
            else if (Input.GetButton(DX.PAD_INPUT_RIGHT))
            { // 右が押されてたら、速度を右へ
                velocity.X = WalkSpeed;
                direction = Direction.Right; // 右向きにする
            }
            else
            { // 左も右も押されてなければ、速度は0にする
                velocity.X = 0;
            }

            if (Input.GetButtonDown(DX.PAD_INPUT_UP) && floor < 3 && surinukeLock <= 0)
            {
                FloorUp();
                surinukeLock = initSurinukeLock;
            }

            if (Input.GetButtonDown(DX.PAD_INPUT_DOWN) && floor > 1 && surinukeLock <= 0)
            {
                FloorDown();
                surinukeLock = initSurinukeLock;
            }

            if (Input.GetButtonDown(DX.PAD_INPUT_1) && surinukeLock <= 0)
            {
                frontSurinuke();
                surinukeLock = initSurinukeLock;
            }
        }

        // 横の移動処理
        void MoveX()
        {
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
                SetLeft(wallRight); // プレイヤーの左端を壁の右端に沿わす
            }
            // 右端が壁にめりこんでいるか？
            else if (
                playScene.map.IsWall(right, top) ||   // 右上が壁か？
                playScene.map.IsWall(right, middle) || // 右中が壁か？
                playScene.map.IsWall(right, bottom))  // 右下が壁か？
            {
                float wallLeft = right - right % Map.CellSize; // 壁の左端
                SetRight(wallLeft); // プレイヤーの右端を壁の左端に沿わす
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
                SetTop(wallBottom); // プレイヤーの頭を天井に沿わす
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
                SetBottom(wallTop); // プレイヤーの足元を床の高さに沿わす
                velocity.Y = 0; // 縦の移動速度を0に
                state = State.Walk; // 状態を歩きに
            }
            else // 着地してなかったら
            {
                state = State.Jump; // 状態をジャンプ中に
            }
        }

        void frontSurinuke()
        {
            // スリができる状態なら
            if (suri == true)
            {
                surinuke_now = true;
                curMoney += Random.Range(1, 5) * 100;
            }

            // プレイヤーの向きに応じてワープ座標を決める
            if(direction == Direction.Right)
            {
                pos.X += 180;
            }
            if (direction == Direction.Left)
            {
                pos.X -= 180;
            }
        }

        public override void Draw()
        {
            // 左右反転するか？（左向きなら反転する）
            bool flip = direction == Direction.Left;

            if (state == State.Walk) // 歩き・立ち状態の場合
            {
                if (velocity.X == 0) // 移動していない場合
                {
                    // Camera.DrawGraph(pos.X, pos.Y, Image.test_zentaman[0], flip);
                    Camera.DrawGraph(pos.X, pos.Y, Image.player, flip); // 仮リソース
                }
                else // 移動している場合
                {
                    //Camera.DrawGraph(pos.X, pos.Y, Image.test_zentaman[5], flip);
                    Camera.DrawGraph(pos.X, pos.Y, Image.player, flip); // 仮リソース
                }
            }
            else if (state == State.Jump) // ジャンプ中の場合
            {
                // Camera.DrawGraph(pos.X, pos.Y, Image.test_zentaman[14], flip);
                Camera.DrawGraph(pos.X, pos.Y, Image.player, flip); // 仮リソース
            }
        }

        public override void OnCollision(GameObject other)
        {
            if(other is Guardman && playScene.state == PlayScene.State.OnAlert )
            {
                hp--;
            }



            //if (other is Goal) //ゴールにぶつかったときの処理
            //{
            //    IsGoal(); //ゴール処理
            //}
        }

        // 死亡処理
        public void Die()
        {
            isDead = true;
            playScene.state = PlayScene.State.PlayerDied;
        }

        // ゴール処理
        public void IsGoal()
        {
            if (curMoney > 1000)// 所持金が目標金額を超えていたら
            {
                playScene.isGoal = true;
            }
            else // 超えていなかったら
            {
                // 何もしない
            }
        }

        public void FloorUp()
        {
            pos.Y -= 280;
            floor++;
        }
        public void FloorDown()
        {
            pos.Y += 180;
            floor--;
        }
    }
}
