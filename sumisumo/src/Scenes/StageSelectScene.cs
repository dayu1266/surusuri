using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            stage1Pos = 120;
            moveTarget = 120;
            stage2Pos = 240 + 1200;
            stage3Pos = 300 + 2400;
        }

        public override void Update()
        {
            if (stageLevel == 1)
            {
                if (Input.GetButtonDown(DX.PAD_INPUT_RIGHT))
                {
                    stageLevel++;
                    moveTarget -= 1200;
                }
            }
            else if (stageLevel == 2)
            {
                if (Input.GetButtonDown(DX.PAD_INPUT_LEFT))
                {
                    stageLevel--;
                    moveTarget += 1200;
                }
                else if (Input.GetButtonDown(DX.PAD_INPUT_RIGHT))
                {
                    stageLevel++;
                    moveTarget -= 1200;
                }
            }
            else if (stageLevel == 3)
            {
                if (Input.GetButtonDown(DX.PAD_INPUT_LEFT))
                {
                    stageLevel--;
                    moveTarget += 1200;
                }
            }

            if (stage1Pos == moveTarget)
            {
                state = State.stop;
            }
            else if (stage1Pos >= moveTarget)
            {
                stage1Pos -= 100;
                stage2Pos -= 100;
                stage3Pos -= 100;
                state = State.move;
            }
            else if (stage1Pos <= moveTarget)
            {
                stage1Pos += 100;
                stage2Pos += 100;
                stage3Pos += 100;
                state = State.move;
            }

            if (Input.GetButtonDown(DX.PAD_INPUT_1))
            {
                Game.SetStageLevel(stageLevel);
                Game.ChangeScene(new PlayScene());
            }
        }

        public override void Draw()
        {
            DX.DrawGraph(0, 0, Image.play_bg, 1); // 背景の描画

            DX.DrawGraph(stage1Pos, 50, Image.stage1_stageSelct, 1);
            DX.DrawGraph(stage2Pos, 0, Image.stage2_stageSelct, 1);
            DX.DrawGraph(stage3Pos, 60, Image.stage3_stageSelct, 1);

            if (stageLevel == 1 && state == State.stop)
            {
                DX.DrawGraph((int)Screen.Size.X - 220, 180, Image.selectArrowR, 1);
            }
            else if (stageLevel == 2 && state == State.stop)
            {
                DX.DrawGraph(50, 180, Image.selectArrowL, 1);
                DX.DrawGraph((int)Screen.Size.X - 220, 180, Image.selectArrowR, 1);
            }
            else if (stageLevel == 3 && state == State.stop)
            {
                DX.DrawGraph(50, 180, Image.selectArrowL, 1);
            }
        }
    }
}
