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
        }

        public override void Update()
        {
            resultCursor.Update();
            flag = resultCursor.moveflag;
            
            if (Input.GetButtonDown(DX.PAD_INPUT_1) && !flag)
            {
                Game.ChangeScene(new TitleScene());
            }
            else if (Input.GetButtonDown(DX.PAD_INPUT_1) && flag)
            {
                Game.ChangeScene(new PlayScene());
            }
        }

        public override void Draw()
        {
            resultCursor.Draw();
            DX.DrawString(0, 0, "GameOverSceneです。ボタン押下でTitleSceneへ。", DX.GetColor(255, 255, 255));
            DX.DrawGraph(120, 300, Image.goMenu,0);
            DX.DrawGraph(120, 450, Image.retry,0);
        }
    }
}
