using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using QimOLib;

namespace sumisumo
{
    public class GameClearScene : Scene
    {
        ResultCursor resultCursor = new ResultCursor();
        bool flag; // カーソルの位置のフラグ
        public override void Init()
        {
        }

        public override void Update()
        {
            resultCursor.Update();
            flag = resultCursor.moveflag; //フラグの取得

            if (Input.GetButtonDown(DX.PAD_INPUT_1) && !flag)
            {
                Game.ChangeScene(new PlayScene());
            }
            else if (Input.GetButtonDown(DX.PAD_INPUT_1) && flag)
            {
                Game.ChangeScene(new TitleScene());
            }
        }

        public override void Draw()
        {          
            DX.DrawGraph(0, 0, Image.gameclear, 0);
            resultCursor.Draw();
        }
    }
}
