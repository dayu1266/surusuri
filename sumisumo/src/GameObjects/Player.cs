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

        const float WalkSpeed = 6f;    // 歩きの速度
        const float Gravity = 0.6f;  // 重力
        const float MaxFallSpeed = 12f;   // 最大落下速度
        const int initSurinukeLock = 90;
        const int mutekijikan = 120;// クールタイム(フレーム)

        const int initialHp = 3;         // 初期HP

        Vector2 velocity = Vector2.Zero; // 移動速度
        State state = State.Walk;        // 現在の状態
        public bool isHiding;

        public int curMoney;            // 所持金
        int surinukeLock;               // すり抜けができるかできないか

        Direction tmp = Direction.Right;    // なんだったか忘れた
        public int hp;                      // HP

        public int floor = 1;   // 今いる階層
        readonly int floorMax;       // 最上層
        const int floorMin = 1;   // 最下層
        int mutekiTimer = 0;
        int animcount = 20;

        float stairInterval = 0.0f;     // 

        public bool Guardman_isDead = false;

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

            curMoney = 1000;

            hp = initialHp;
            surinukeLock = initSurinukeLock;
            isHiding = false;

            int stageLv = Game.GetStageLevel();

            // プレイヤーの生成
            if (stageLv == 1)
            {
                floorMax = 3;
            }
            else if (stageLv == 2)
            {
                floorMax = 5;
            }
            else if (stageLv == 3)
            {
                floorMax = 7;
            }

            playScene.gameObjects.Add(new Sight(playScene, this, pos));
        }

        public override void Update()
        {
            // すり抜けの更新
            surinuke = false;

            if (velocity.X != 0)
            {
                animcount += 3;
                if (animcount > 80)
                    animcount = 20;
            }

            if (!isHiding)
            {
                // 入力を受けての処理
                HandleInput();

                // 重力による落下
                velocity.Y += Gravity;
                if (velocity.Y > MaxFallSpeed) velocity.Y = MaxFallSpeed;
            }

            // まず横に動かす
            MoveX();
            // 次に縦に動かす
            MoveY();

            // すり抜けができない時間
            if (surinukeLock > 0)
            {
                surinukeLock--;
            }

            if (direction != tmp)
            {
                tmp = direction;
            }

            stairInterval--;

            suri = false;
            Guardman_isDead = false;

            mutekiTimer--;
        }

        // 入力を受けての処理
        void HandleInput()
        {
            if (Input.GetButton(DX.PAD_INPUT_LEFT) && !(Input.GetButton(DX.PAD_INPUT_RIGHT)))
            {
                velocity.X = -WalkSpeed;        // 左が押されてたら、速度を左へ
                direction = Direction.Left;     // 左向きにする
            }
            else if (Input.GetButton(DX.PAD_INPUT_RIGHT))
            {
                velocity.X = WalkSpeed;         // 右が押されてたら、速度を右へ
                direction = Direction.Right;    // 右向きにする
            }
            else if (!(Input.GetButton(DX.PAD_INPUT_LEFT) && Input.GetButton(DX.PAD_INPUT_RIGHT)))
            {
                velocity.X = 0; // 左右両方入力、両方無入力のとき速度を0にする
            }

            // 上下すり抜け
            if (Input.GetButtonDown(DX.PAD_INPUT_UP) && floor < floorMax && this.pos.X >= 1039 && surinukeLock <= 0)
            {
                FloorUp();
                surinukeLock = initSurinukeLock;
                Sound.SePlay(Sound.se_surinuke);
            }

            if (Input.GetButtonDown(DX.PAD_INPUT_DOWN) && floor > floorMin && this.pos.X >= 1039 && surinukeLock <= 0)
            {
                FloorDown();
                surinukeLock = initSurinukeLock;
                Sound.SePlay(Sound.se_surinuke);
            }

            // 正面すり抜け
            if (Input.GetButtonDown(DX.PAD_INPUT_1) && surinukeLock <= 0)
            {
                FrontSurinuke();
                surinukeLock = initSurinukeLock;
                Sound.SePlay(Sound.se_surinuke);
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

        public override void Draw()
        {
            if (!isHiding)
            {
                // 左右反転するか？（左向きなら反転する）
                bool flip = direction == Direction.Left;

                if (state == State.Walk && mutekiTimer <= 0 || mutekiTimer % 2 == 0) // 歩き・立ち状態の場合
                {
                    if (velocity.X == 0) // 移動していない場合
                    {
                        // Camera.DrawGraph(pos.X, pos.Y, Image.test_zentaman[0], flip);
                        Camera.DrawGraph(pos.X, pos.Y, Image.player[0], flip); // 仮リソース
                    }
                    else // 移動している場合
                    {
                        //Camera.DrawGraph(pos.X, pos.Y, Image.test_zentaman[5], flip);
                        Camera.DrawGraph(pos.X, pos.Y, Image.player[animcount / 20], flip); // 仮リソース
                    }
                }
                else if (state == State.Jump) // ジャンプ中の場合
                {
                    // Camera.DrawGraph(pos.X, pos.Y, Image.test_zentaman[14], flip);
                    Camera.DrawGraph(pos.X, pos.Y, Image.player[0], flip); // 仮リソース
                }
            }

            // クールタイムゲージ描画
            GaugeDrawer();
        }

        public override void OnCollision(GameObject other)
        {
            if (playScene.state == PlayScene.State.OnAlert && (other is Guardman || other is Police) && !isHiding)
            {
                //無敵じゃなければダメージを受ける
                if (mutekiTimer <= 0 && other.find)
                {
                    Damage();
                    if (direction == Direction.Right)
                    {
                        pos.X += 300.0f;
                    }
                    else if (direction == Direction.Left)
                    {
                        pos.X -= 300.0f;
                    }
                }
            }
            if (Input.GetButtonDown(DX.PAD_INPUT_2))
            {
                if (other is UpStairs && stairInterval < 0.0f)
                {
                    StairUp();
                }
                else if (other is DownStairs && stairInterval < 0.0f)
                {
                    StairDown();
                }
            }

            if (other is Goal) //ゴールにぶつかったときの処理
            {
                IsGoal(); //ゴール処理
            }
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
            // 所持金が目標金額を超えていたら
            if (curMoney > playScene.targetAmout) playScene.isGoal = true;
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

        void FrontSurinuke()
        {
            // すり抜けが行われた
            surinuke = true;

            // スリができる状態なら
            if (suri == true)
            {
                SurinukeBoolChange(Sight.GetTarget());
                int getMoney = Random.Range(1, 5) * 100;
                playScene.gameObjects.Add(new GetMoneyUi(playScene, pos, getMoney));
                curMoney += getMoney;
                Sound.SePlay(Sound.se_get_coin);
            }

            // 警備員を倒す処理
            if (Guardman_isDead)
            {
                GameObject go = Sight.GetTarget();
                go.isDead = true;
            }

            // プレイヤーの向きに応じてワープ座標を決める
            if (direction == Direction.Right)
            {
                if (pos.X >= 3413 - 180)
                {
                    pos.X = 3413;
                }
                else
                {
                    pos.X += 180;
                }
            }
            if (direction == Direction.Left)
            {
                if (pos.X <= 1039 + 180)
                {
                    pos.X = 1039;
                }
                else
                {
                    pos.X -= 180;
                }
            }
        }

        public void BeHidden() // 隠れる
        {
            velocity.X = 0;
            isHiding = true;
        }
        public void Apeear() // 出てくる
        {
            isHiding = false;
            playScene.gameObjects.Add(new Sight(playScene, this, pos));
        }

        private void GaugeDrawer()
        {
            int counter = 90 - surinukeLock;

            counter /= 3;

            Camera.DrawGraph(Camera.cameraPos.X + 198, Camera.cameraPos.Y + 11, Image.cooltimeGauge[counter]);
        }

        void StairUp()
        {
            pos.X += 160;
            pos.Y -= 224;
            floor++;
            stairInterval = 10.0f;
        }
        void StairDown()
        {
            pos.X -= 160;
            pos.Y += 224;
            floor--;
            stairInterval = 10.0f;
        }
        void Damage()
        {
            hp--;
            if (hp <= 0)
            {
                Die();
            }
            //無敵時間発動
            mutekiTimer = mutekijikan;
        }

        public void SurinukeBoolChange(GameObject go)
        {
            go.Surizumi = true;
        }
    }
}
