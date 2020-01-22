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
        // ステージレベル
        private int stageLevel = 1;

        public override void Init()
        {
        }

        public override void Update()
        {
            if (stageLevel == 1)
            {
                if (Input.GetButtonDown(DX.PAD_INPUT_RIGHT))
                {
                    stageLevel++;
                }
            }
            else if (stageLevel == 2)
            {
                if (Input.GetButtonDown(DX.PAD_INPUT_LEFT))
                {
                    stageLevel--;
                }
                else if (Input.GetButtonDown(DX.PAD_INPUT_RIGHT))
                {
                    stageLevel++;
                }
            }
            else if (stageLevel == 3)
            {
                if (Input.GetButtonDown(DX.PAD_INPUT_LEFT))
                {
                    stageLevel--;
                }
            }

            if (Input.GetButtonDown(DX.PAD_INPUT_1))
            {
                Game.SetStageLevel(stageLevel);
                Game.ChangeScene(new PlayScene());
            }
        }

        public override void Draw()
        {
            DX.DrawString(250, 250, stageLevel.ToString(), DX.GetColor(255, 0, 0));
        }
    }
}
