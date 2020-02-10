using DxLibDLL;
using QimOLib;
using System.Collections.Generic;
using System.Numerics;

namespace sumisumo
{
    public class PlayScene : Scene
    {
        // プレイ画面の状態
        public enum State
        {
            Active, // 通常時
            OnAlert, //警戒中
            PlayerDied, // プレイヤーが死んだとき
        }

        // 参照
        public Map map;
        public Player player;

        // 全GameObjectを一括管理するリスト
        public List<GameObject> gameObjects = new List<GameObject>();

        private readonly string stageName;                   // ステージの名前
        public State state        = State.Active;  // PlaySceneの状態
        int timeToGameOver        = 120;           // ゲームオーバーになるまでの時間（フレーム）

        public bool isGoal        = false;         // ゴールしたかどうか
        bool clearSE              = false;         // クリア可能SEを流すとTRUEになる
        public int targetAmout    = 1000;          // 目標金額

        bool OnAlertOnce = false;

        public PlayScene()
        {
            int stageLv = Game.GetStageLevel();
            stageName = "stage" + stageLv.ToString();
            targetAmout *= stageLv;

            // インスタンス生成
            map = new Map(this, stageName);

            // プレイヤーの生成
            if (stageLv == 1)
            {
                player = new Player(this, new Vector2(1160, 640));
            }
            else if (stageLv == 2)
            {
                player = new Player(this, new Vector2(920, 1248));
            }
            else if (stageLv == 3)
            {
                player = new Player(this, new Vector2(920, 1696));
            }

            gameObjects.Insert(gameObjects.Count,player);
            Camera.LookAt(player.pos.X, player.pos.Y);
        }

        public override void Init()
        {
            Sound.BgmPlay(Sound.bgm_nomalBGM);
        }

        public override void Update()
        {
            // OnAlertになったとき１度だけ呼ばれる
            if(state == State.OnAlert && !OnAlertOnce)
            {
                GameObject goal;
                goal = gameObjects.Find(n => n is Goal);
                gameObjects.Add(new Police(this, new Vector2(goal.pos.X + 100.0f, goal.pos.Y + 80.0f)));
                Sound.BgmPlay(Sound.bgm_warningBGM);
                OnAlertOnce = true;
            }

            // 全オブジェクトの更新
            int gameObjectsCount = gameObjects.Count; // ループ前の個数を取得しておく
            for (int i = 0; i < gameObjectsCount; i++)
            {
                gameObjects[i].Update();
            }

            // オブジェクト同士の衝突を判定
            for (int i = 0; i < gameObjects.Count; i++)
            {
                GameObject a = gameObjects[i];

                for (int j = i + 1; j < gameObjects.Count; j++)
                {
                    // オブジェクトAが死んでたらこのループは終了
                    if (a.isDead) break;

                    GameObject b = gameObjects[j];

                    // オブジェクトBが死んでたらスキップ
                    if (b.isDead) continue;

                    // オブジェクトAとBが重なっているか？
                    if (Math2D.RectRectIntersect(
                        a.GetLeft(), a.GetTop(), a.GetRight(), a.GetBottom(),
                        b.GetLeft(), b.GetTop(), b.GetRight(), b.GetBottom()))
                    {
                        a.OnCollision(b);
                        b.OnCollision(a);
                    }
                }
            }

            // 不要となったオブジェクトを除去する
            gameObjects.RemoveAll(go => go.isDead);

            Camera.LookAt(player.pos.X, player.pos.Y);

            // isGoalがTrueになったらSEを流す
            if(player.curMoney >= targetAmout && clearSE == false)
            {
                Sound.SePlay(Sound.se_gameclear);
                clearSE = true;
            }

            // プレイヤーがゴールしたときの処理
            if (state != State.PlayerDied && isGoal)
            {
                Game.ChangeScene(new GameClearScene());// GameClearSceneにする
            }

            // プレイヤーが死んでゲームオーバーに移る直前の状態の処理
            if (state == State.PlayerDied)
            {
                timeToGameOver--; // カウントダウン

                if (timeToGameOver <= 0) // 0になったら
                {
                    Game.ChangeScene(new GameOverScene()); // GameOverSceneにする
                }
            }
        }

        public override void Draw()
        {
            DX.DrawGraph(0, 0, Image.play_bg, 0);

            // マップの描画
            map.DrawTerrain();

            if (state == State.OnAlert) DX.DrawGraph(0, 0, Image.alerteffect, DX.TRUE);

            // 全オブジェクトの描画
            foreach (GameObject go in gameObjects)
            {
                go.Draw();
            }
            // プレイヤーのHPのUI
            for (int i = 0; i < player.hp; i++)
            {
                DX.DrawRotaGraph(48 + (i * 54), 36, 4, 0, Image.heart, 1);
            }

            // プレイヤーの所持金表示
            string money = player.curMoney.ToString("00000");
            for (int i = 0; i < money.Length; i++)
            {
                //DX.DrawRotaGraph(1140 + (16 * i), 32, 1.0f, 0, Image.number[money[i] - '0'], 1);
                DX.DrawRotaGraph((int)Screen.Size.X - (int)Screen.Size.X / 15 + (16 * i), 32, 1.0f, 0, Image.number[money[i] - '0'], DX.TRUE);
            }
            DX.DrawRotaGraph((int)Screen.Size.X - (int)Screen.Size.X / 18 + 80, 32, 1.0f, 0, Image.yen, DX.TRUE);
            #if DEBUG
            DX.DrawStringF(Screen.Size.X/2, 330, player.pos.X.ToString() + "," + player.pos.Y.ToString(), DX.GetColor(255, 255, 255));
            #endif
            //DX.DrawString(1060, 26, "/", DX.GetColor(0, 0, 0));
            //for (int i = 0; i < targetAmout.ToString().Length; i++)
            //{
            //    DX.DrawRotaGraph(1080 + (16 * i), 32, 0.3f, 0, Image.number[targetAmout.ToString()[i] - '0'], 1);
            //}



            #if DEBUG // Debugのみ実行される
            // 当たり判定のデバッグ表示
            foreach (GameObject go in gameObjects)
            {
                go.DrawHitBox();
            }
            #endif
        }

        public void StateChange(State state)
        {
            this.state = state;
        }
    }
}