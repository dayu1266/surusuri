using DxLibDLL;
using QimOLib;

namespace sumisumo
{
    public class Game
    {
        static Scene scene;

        public void Init()
        {
            Input.Init();
            Random.Init();
            scene = new LoadScene();
            scene.Init();
        }

        public void Update()
        {
            Input.Update();

            if (Input.GetButtonDown(DX.PAD_INPUT_9))
            {
                DX.DxLib_End();
            }

            scene.Update();
        }

        public void Draw()
        {
            scene.Draw();
        }

        public static void ChangeScene(Scene newScene)
        {
            scene = newScene;
        }
    }
}
