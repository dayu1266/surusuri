using DxLibDLL;
using QimOLib;

namespace sumisumo
{
    class TitleScene : Scene
    {
        enum State
        {
            Scroll, // タイトルロゴのスクロール状態
            Ready,  // 開始準備完了
            Flash,  // スタートボタン点滅状態
        }

        int logoPosY; // ロゴのY座標
        State state;
        int flashTimer; // 点滅のカウンター
        int flashInterval; // ボタンを押してからシーン遷移までの時間
        public override void Init()
        {
            state = State.Scroll;
            logoPosY = -64;
            flashTimer = 0;
            flashInterval = 0;

            Sound.BgmPlay(Sound.bgm_titleBGM);
        }

        public override void Update()
        {
            if (state == State.Scroll)
            {
                logoPosY += 2;
                if (logoPosY >= 250)
                {
                    state = State.Ready;
                }
                if (Input.GetButtonDown(DX.PAD_INPUT_1))
                {
                    logoPosY = 250;
                }
            }
            if (state == State.Ready)
            {
                flashTimer++;
                if (Input.GetButtonDown(DX.PAD_INPUT_1))
                {
                    Sound.SePlay(Sound.se_decision);
                    state = State.Flash;
                }
            }
            if (state == State.Flash)
            {
                flashTimer++;
                flashInterval++;
                if (flashInterval >= 80)
                {
                    Game.ChangeScene(new StageSelectScene());
                }
            }
        }

        public override void Draw()
        {
            DX.DrawGraph(0, 0, Image.play_bg, 1); // 背景の描画
            DX.DrawRotaGraph((int)Screen.Size.X / 2, logoPosY, 6, 0, Image.titlelogo, 1); // ロゴの描画
            if (state == State.Ready && flashTimer / 16 % 2 == 0)
            {
                DX.DrawRotaGraph((int)Screen.Size.X / 2, 600, 4, 0, Image.gamestart, 1); // スタートボタンの描画
            }
            if(state == State.Flash && flashTimer / 2 % 6 == 0)
            {
                DX.DrawRotaGraph((int)Screen.Size.X / 2, 600, 4, 0, Image.gamestart, 1);
            }
        }
    }
}
