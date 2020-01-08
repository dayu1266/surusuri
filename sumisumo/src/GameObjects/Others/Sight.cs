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

        public Sight(PlayScene playScene, GameObject gameObject, Vector2 pos) : base(playScene)
        {
            this.pos = pos;
            oya = gameObject;

            imageWidth = 60;
            imageHeight = 140;
            hitboxOffsetLeft = 17;
            hitboxOffsetRight = 17;
            hitboxOffsetTop = 9;
            hitboxOffsetBottom = 10;
        }

        public override void Update()
        {
            pos = oya.pos;
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
