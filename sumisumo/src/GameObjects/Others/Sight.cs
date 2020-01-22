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
        // 各オブジェクトの参照（主にコリジョンで使用）
        GameObject obj;
        Direction direction;
        PlayScene playScene;
        Player player;
        Guardman guardman;

        public Sight(PlayScene playScene, GameObject gameObject, Vector2 pos) : base(playScene)
        {
            this.pos = pos;
            obj = gameObject;
            this.playScene = playScene;

            imageWidth = 60;
            imageHeight = 140;
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
            // 警備員のリストを生成しplayScene.gameObjectsから警備員のみを取り出す
            List<GameObject> guardman = new List<GameObject>();
            guardman = playScene.gameObjects.FindAll(n => n is Guardman);

            // 親がプレイヤーで相手が一般ピーポーなら
            if (obj == playScene.gameObjects.Find(n => n is Player) && other is People)
            {
                obj.suri = true;
            }
            else
            {
                obj.suri = false;
            }

            // もし親が警備員で相手がプレイヤーなら
            for (int i = 0; i < guardman.Count; i++)
            {
                if (obj == guardman[i] && other is Player)
                {
                    obj.alert = true;
                }
            }
        }
    }
}
