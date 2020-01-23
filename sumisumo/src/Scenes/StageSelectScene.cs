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
        int stageLevel = 1;

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
                    Sound.SePlay(Sound.se_switch);
                }
            }
            else if (stageLevel == 2)
            {
                if (Input.GetButtonDown(DX.PAD_INPUT_LEFT))
                {
                    stageLevel--;
                    Sound.SePlay(Sound.se_switch);
                }
                else if (Input.GetButtonDown(DX.PAD_INPUT_RIGHT))
                {
                    stageLevel++;
                    Sound.SePlay(Sound.se_switch);
                }
            }
            else if (stageLevel == 3)
            {
                if (Input.GetButtonDown(DX.PAD_INPUT_LEFT))
                {
                    stageLevel--;
                    Sound.SePlay(Sound.se_switch);
                }
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
            DX.DrawRotaGraph((int)Screen.Size.X / 2, (int)Screen.Size.Y / 2, 1, 0, Image.stageselect_bg, 1);

            if (stageLevel == 1)
            {
                DX.DrawRotaGraph((int)Screen.Size.X/2, (int)Screen.Size.Y / 2, 0.5, 0, Image.stage1_buck, 1);
                DX.DrawRotaGraph((int)Screen.Size.X/2, (int)Screen.Size.Y / 2 , 1, 0, Image.stage_name_1, 1);
            }
            else if (stageLevel == 2)
            {
                DX.DrawRotaGraph((int)Screen.Size.X / 2, (int)Screen.Size.Y / 2, 0.5, 0, Image.stage2_buck, 1);
                DX.DrawRotaGraph((int)Screen.Size.X/2, (int)Screen.Size.Y / 2 , 1, 0, Image.stage_name_2, 1);
            }
            else if (stageLevel == 3)
            {
                DX.DrawRotaGraph((int)Screen.Size.X / 2, (int)Screen.Size.Y / 2, 0.5, 0, Image.stage3_buck, 1);
                DX.DrawRotaGraph((int)Screen.Size.X/2, (int)Screen.Size.Y / 2, 1, 0, Image.stage_name_3, 1);
            }
        }
    }
}
