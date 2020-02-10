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
            Sound.BgmPlay(Sound.bgm_gameclearBGM);
        }

        public override void Update()
        {
            resultCursor.Update();
            flag = resultCursor.moveflag; //フラグの取得
            if (Game.GetStageLevel() == 3)
            {
                resultCursor.posX = 450;
                resultCursor.moveflag = true;
            }
            else if (Input.GetButtonDown(DX.PAD_INPUT_1) && !flag)
            {
                Game.SetStageLevel(Game.GetStageLevel() + 1);
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
            if (Game.GetStageLevel() == 3)
            {
                DX.DrawRotaGraph((int)Screen.Size.X / 2, (int)Screen.Size.Y / 2, 1.0f, 0, Image.laststageclear, 0);
            }
            else
            {
                DX.DrawRotaGraph((int)Screen.Size.X / 2, (int)Screen.Size.Y / 2, 1.0f, 0, Image.gameclear, 0);
            }
            resultCursor.Draw();
        }
    }
}
