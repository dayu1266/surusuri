using DxLibDLL;
using QimOLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sumisumo
{
    class TitleScene : Scene
    {
        public override void Init()
        {
        }

        public override void Update()
        {
            if (Input.GetButtonDown(DX.PAD_INPUT_1))
            {
                Game.ChangeScene(new PlayScene());
            }

        }

        public override void Draw()
        {
            DX.DrawGraph(0, 0, Image.play_bg,1);
            DX.DrawRotaGraph((int)Screen.Size.X/2, 250, 6, 0, Image.titlelogo, 1);
        }
    }
}
