using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using QimOLib;
using DxLibDLL;

namespace sumisumo
{
    public class Sight : GameObject
    {
        // 各オブジェクトの参照
        GameObject obj;
        Direction direction;

        public Sight(PlayScene playScene, GameObject gameObject, Vector2 pos) : base(playScene)
        {
            this.pos = pos;
            obj = gameObject;
            this.playScene = playScene;

            imageHeight = 140;
            
            // 親により視野の広さを変える（）
            if (typeof(Player) == obj.GetType())  imageWidth = 100;
            else imageWidth = 60;

            hitboxOffsetLeft = 0;
            hitboxOffsetRight = 0;
            hitboxOffsetTop = 9;
            hitboxOffsetBottom = 10;
        }

        public override void Update()
        {
            // 親の向いている方向を取得
            direction = obj.direction;

            // 向いている方向により視界のポジションを変える
            if (direction == Direction.Right)
            {
                pos = obj.pos;
                pos.X += 45;
            }
            if (direction == Direction.Left)
            {
                pos = obj.pos;
                pos.X -= 45;
            }
        }

        public override void Draw()
        {
        }

        public override void OnCollision(GameObject other)
        {
            // 親がプレイヤーで相手が一般ピーポーなら
            if ((typeof(Player) == obj.GetType()) && other is People)
            {
                obj.suri = true;
            }


            // 親が警備員で相手がプレイヤーなら
            if ((typeof(Guardman) == obj.GetType()) && other is Player)
            {
                obj.alert = true;
                if (playScene.state == PlayScene.State.OnAlert)
                {
                    obj.find = true;
                }
            }
        }
    }
}
