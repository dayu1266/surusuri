using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using QimOLib;

namespace sumisumo
{
    public class GameOverScene: Scene
    {
        ResultCursor resultCursor = new ResultCursor();
        bool flag;
        public override void Init()
        {
            Sound.BgmPlay(Sound.bgm_gameclearBGM);
        }

        public override void Update()
        {
            resultCursor.Update();
            flag = resultCursor.moveflag;
            
            if (Input.GetButtonDown(DX.PAD_INPUT_1) && !flag)
            {
                Sound.SePlay(Sound.se_decision);
                Game.ChangeScene(new TitleScene());
            }
            else if (Input.GetButtonDown(DX.PAD_INPUT_1) && flag)
            {
                Sound.SePlay(Sound.se_decision);
                Game.ChangeScene(new PlayScene());
            }
        }

        public override void Draw()
        {
            
            DX.DrawRotaGraph((int)Screen.Size.X / 2, (int)Screen.Size.Y / 2, 0.8f, 0, Image.gameover, 0);
            resultCursor.Draw();
            DX.DrawGraph(260, 570, Image.gotitle, 0);
            DX.DrawGraph(760, 570, Image.retry, 0);
        }
    }
}
