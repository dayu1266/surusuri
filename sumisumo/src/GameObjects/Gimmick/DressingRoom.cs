﻿using DxLibDLL;
using QimOLib;
using System.Numerics;

namespace sumisumo
{
    public class DressingRoom : GameObject
    {
        private bool discovery;
        private bool inUse;

        public DressingRoom(PlayScene playScene, Vector2 pos) : base(playScene)
        {
            this.pos.X = pos.X;
            this.pos.Y = pos.Y;

            imageWidth = 100;
            imageHeight = 128;
            hitboxOffsetLeft = 1;
            hitboxOffsetRight = 6;
            hitboxOffsetTop = 0;
            hitboxOffsetBottom = 32;

            discovery = false;
            inUse = false;
        }

        public override void Update()
        {
            discovery = false;
        }

        public override void Draw()
        {
            if (!inUse)
            {
                Camera.DrawGraph(pos.X, pos.Y, Image.dressingRoom_open);

                if (discovery)
                {
                    Camera.DrawGraph(pos.X + 64, pos.Y - 32, Image.gimmicksign);
                }
            }
            else
            {
                Camera.DrawGraph(pos.X, pos.Y, Image.dressingRoom_close);
            }
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Player)
            {
                discovery = true;

                if (!inUse && Input.GetButtonDown(DX.PAD_INPUT_2))
                {
                    inUse = true;
                    playScene.player.BeHidden();
                    discovery = false;
                }
                else if (inUse && Input.GetButtonDown(DX.PAD_INPUT_2))
                {
                    inUse = false;
                    playScene.player.Apeear();
                }
            }

           
        }
    }
}
