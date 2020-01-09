using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace sumisumo
{
    public class Sight : GameObject
    {
        GameObject oya;
        Direction direction;

        public Sight(PlayScene playScene, GameObject gameObject, Vector2 pos) : base(playScene)
        {
            this.pos = pos;
            oya = gameObject;

            imageWidth = 60;
            imageHeight = 140;
            hitboxOffsetLeft = 0;
            hitboxOffsetRight = 0;
            hitboxOffsetTop = 9;
            hitboxOffsetBottom = 10;
        }

        public override void Update()
        {
            direction = oya.direction;
            if (direction == Direction.Right)
            {
                pos = oya.pos;
                pos.X += 45;
            }
            if(direction == Direction.Left)
            {
                pos = oya.pos;
                pos.X -= 45;
            }
        }

        public override void Draw()
        {
            //Camera.DrawGraph(pos.X, pos.Y, Image.test_shiitake);
        }

        public override void OnCollision(GameObject other)
        {
        }
    }
}
