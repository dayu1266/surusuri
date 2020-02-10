using DxLibDLL;
using QimOLib;

namespace sumisumo
{
    public class GameOverScene : Scene
    {
        ResultCursor resultCursor = new ResultCursor();
        bool flag;
        public override void Init()
        {
            Sound.BgmPlay(Sound.bgm_gameoverBGM);
        }

        public override void Update()
        {
            resultCursor.Update();
            flag = resultCursor.moveflag;

            if (Input.GetButtonDown(DX.PAD_INPUT_1) && !flag)
            {
                Sound.SePlay(Sound.se_decision);
                Game.ChangeScene(new PlayScene());
            }
            else if (Input.GetButtonDown(DX.PAD_INPUT_1) && flag)
            {
                Sound.SePlay(Sound.se_decision);
                Game.ChangeScene(new TitleScene());
            }
        }

        public override void Draw()
        {
            DX.DrawRotaGraph((int)Screen.Size.X / 2, (int)Screen.Size.Y / 2 - 100, 0.8f, 0, Image.gameover, 0);
            resultCursor.Draw();
            DX.DrawGraph((int)Screen.Size.X / 2 + 200, (int)Screen.Size.Y / 2 + 200, Image.gotitle, 1);
            DX.DrawGraph((int)Screen.Size.X / 2 - 550, (int)Screen.Size.Y / 2 + 200, Image.retry, 1);
        }
    }
}
