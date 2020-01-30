using DxLibDLL;
using QimOLib;

namespace sumisumo
{
    public class ResultScene : Scene
    {
        public override void Init() { }

        public override void Update()
        {
            if (Input.GetButtonDown(DX.PAD_INPUT_1))
            {
                Game.ChangeScene(new TitleScene());
            }
        }

        public override void Draw()
        {
            DX.DrawString(0, 0, "ResultSceneです。ボタン押下でTitleSceneへ。", DX.GetColor(255, 255, 255));
        }
    }
}
