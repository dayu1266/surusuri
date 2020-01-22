using System;
using System.Numerics;
using DxLibDLL;
using QimOLib;

namespace sumisumo
{
    enum UseState
    {
        Before,
        After,
    }
    public class FireHydrant : GameObject
    {
        UseState state;
        private bool discovery;
        public FireHydrant(PlayScene playScene, Vector2 pos) : base(playScene)
        {
            this.pos.X = pos.X;
            this.pos.Y = pos.Y;

            imageWidth = 100;
            imageHeight = 128;
            hitboxOffsetLeft = 0;
            hitboxOffsetRight = 36;
            hitboxOffsetTop = 0;
            hitboxOffsetBottom = 32;
            state = UseState.Before;
            discovery = false;
        }
        public override void Update()
        {
            discovery = false;
        }

        public override void Draw()
        {
            Camera.DrawGraph(pos.X, pos.Y, Image.fireHydrant);
            if (state == UseState.Before && discovery) // 使用前かつ発見されているなら
            {
                Camera.DrawGraph(pos.X + 64, pos.Y - 32, Image.gimmicksign); // ビックリマークを表示する
            }
             
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Player && state == UseState.Before) //使用前だったら
            {
                discovery = true; // 発見された状態にする
                if (Input.GetButtonDown(DX.PAD_INPUT_2))
                {
                    if (playScene.state == PlayScene.State.Active)
                    {
                        playScene.StateChange(PlayScene.State.OnAlert);
                    }
                    int gameObjectsCount = playScene.gameObjects.Count;
                    for (int i = 0; i < gameObjectsCount; i++)
                    {
                        playScene.gameObjects[i].Buzzer(); // ゲームオブジェクトのbuzzer処理を呼び出す
                    }
                    state = UseState.After;
                }
            }
        }
    }
}
