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
        enum State
        {
            Scroll,
            Ready,
            Flash,
        }
        int logoPosY;
        State state;
        int flashTimer;
        int flashInterval;
        public override void Init()
        {
            state = State.Scroll;
            logoPosY = -64;
            flashTimer = 0;
            flashInterval = 0;
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
            DX.DrawGraph(0, 0, Image.play_bg, 1);
            DX.DrawRotaGraph((int)Screen.Size.X / 2, logoPosY, 6, 0, Image.titlelogo, 1);
            if (state == State.Ready && flashTimer / 16 % 2 == 0)
            {
                DX.DrawRotaGraph((int)Screen.Size.X / 2, 600, 4, 0, Image.gamestart, 1);
            }
            if(state == State.Flash && flashTimer / 2 % 6 == 0)
            {
                DX.DrawRotaGraph((int)Screen.Size.X / 2, 600, 4, 0, Image.gamestart, 1);
            }
        }
    }
}
