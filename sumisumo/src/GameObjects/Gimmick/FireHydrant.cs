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
            if (discovery && state == UseState.Before)
            {
                Camera.DrawGraph(pos.X + 64, pos.Y - 32, Image.gimmicksign);
            }
             
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Player && state == UseState.Before)
            {
                discovery = true;
                if (Input.GetButtonDown(DX.PAD_INPUT_2))
                {
                    if (playScene.state == PlayScene.State.Active)
                    {
                        playScene.state = PlayScene.State.OnAlert;
                    }

                    float playerposy = playScene.player.pos.Y;
                    int gameObjectsCount = playScene.gameObjects.Count;
                    for (int i = 0; i < gameObjectsCount; i++)
                    {
                        playScene.gameObjects[i].Buzzer(playerposy);
                    }
                    state = UseState.After;
                }
            }
        }
    }
}
