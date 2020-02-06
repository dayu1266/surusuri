using DxLibDLL;
using QimOLib;

namespace sumisumo
{
    class StageSelectScene : Scene
    {
        enum State
        {
            stop,
            move
        }

        // ステージレベル
        private int stageLevel = 1;
        private int stage1Pos;
        private int stage2Pos;
        private int stage3Pos;
        private int moveTarget;

        State state;

        public override void Init()
        {
            state = State.stop;

            stage1Pos = (int)Screen.Size.X / 2 - 530;
            moveTarget = (int)Screen.Size.X / 2 - 530;
            stage2Pos = (int)Screen.Size.X / 2 - 430 + 2400;
            stage3Pos = (int)Screen.Size.X / 2 - 340 + 4800;
        }

        public override void Update()
        {
            if (stageLevel == 1)
            {
                if (Input.GetButtonDown(DX.PAD_INPUT_RIGHT))
                {
                    stageLevel++;
                    moveTarget -= 2400;
                    Sound.SePlay(Sound.se_switch);
                }
            }
            else if (stageLevel == 2)
            {
                if (Input.GetButtonDown(DX.PAD_INPUT_LEFT))
                {
                    stageLevel--;
                    moveTarget += 2400;
                    Sound.SePlay(Sound.se_switch);
                }
                else if (Input.GetButtonDown(DX.PAD_INPUT_RIGHT))
                {
                    stageLevel++;
                    moveTarget -= 2400;
                    Sound.SePlay(Sound.se_switch);
                }
            }
            else if (stageLevel == 3)
            {
                if (Input.GetButtonDown(DX.PAD_INPUT_LEFT))
                {
                    stageLevel--;
                    moveTarget += 2400;
                    Sound.SePlay(Sound.se_switch);
                }
            }

            if (stage1Pos == moveTarget)
            {
                state = State.stop;
            }
            else if (stage1Pos >= moveTarget)
            {
                stage1Pos -= 200;
                stage2Pos -= 200;
                stage3Pos -= 200;
                state = State.move;
            }
            else if (stage1Pos <= moveTarget)
            {
                stage1Pos += 200;
                stage2Pos += 200;
                stage3Pos += 200;
                state = State.move;
            }

            if (Input.GetButtonDown(DX.PAD_INPUT_1))
            {
                Game.SetStageLevel(stageLevel);
                Game.ChangeScene(new PlayScene());
                Sound.SePlay(Sound.se_decision);
            }
        }

        public override void Draw()
        {
            DX.DrawGraph(0, 0, Image.play_bg, 1); // 背景の描画

            DX.DrawGraph(stage1Pos, (int)Screen.Size.Y / 2 - 300, Image.stage1_stageSelct, 1);
            DX.DrawGraph(stage2Pos, (int)Screen.Size.Y / 2 - 380, Image.stage2_stageSelct, 1);
            DX.DrawGraph(stage3Pos, (int)Screen.Size.Y / 2 - 350, Image.stage3_stageSelct, 1);

            if (stageLevel == 1 && state == State.stop)
            {
                DX.DrawGraph((int)Screen.Size.X - 220, (int)Screen.Size.Y / 2 - 150, Image.selectArrowR, 1);
            }
            else if (stageLevel == 2 && state == State.stop)
            {
                DX.DrawGraph(50, (int)Screen.Size.Y / 2 - 150, Image.selectArrowL, 1);
                DX.DrawGraph((int)Screen.Size.X - 220, (int)Screen.Size.Y / 2 - 150, Image.selectArrowR, 1);
            }
            else if (stageLevel == 3 && state == State.stop)
            {
                DX.DrawGraph(50, (int)Screen.Size.Y / 2 - 150, Image.selectArrowL, 1);
            }
        }
    }
}
