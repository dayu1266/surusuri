using DxLibDLL;
using QimOLib;

namespace sumisumo
{
    public class Game
    {
        static Scene scene;
        static int stageLv;
        static bool init = false;

        public void Init()
        {
            Input.Init();
            Random.Init();
            scene = new LoadScene();
            scene.Init();
        }

        public void Update()
        {
            // initはUpdateの最初に１回だけ実行する
            if (!init)
            {
                scene.Init();
                init = true;
            }

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
            init = false;
        }

        public static void SetStageLevel(int Level)
        {
            stageLv = Level;
        }

        public static int GetStageLevel()
        {
            return stageLv;
        }
    }
}
