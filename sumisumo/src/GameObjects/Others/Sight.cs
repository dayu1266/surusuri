﻿using System;
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
        static GameObject target;
        Direction direction;

        public Sight(PlayScene playScene, GameObject gameObject, Vector2 pos) : base(playScene)
        {
            this.pos = pos;
            obj = gameObject;
            this.playScene = playScene;

            imageHeight = 140;

            // 親により視野の広さを変える（）
            if (typeof(Player) == obj.GetType()) imageWidth = 130;
            else imageWidth = 70;

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
                pos.X = obj.GetRight();
                pos.Y = obj.GetTop() - 11.0f;
            }
            if (direction == Direction.Left)
            {
                pos.X = obj.GetLeft() - imageWidth;
                pos.Y = obj.GetTop() - 11.0f;
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
                target = other;
                if (other.Surizumi == false)
                {
                    obj.suri = true;
                }
                else
                {
                    obj.suri = false;
                }
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

            // 親が一般ピーポーで相手がプレイヤーなら
            if ((typeof(People) == obj.GetType()) && other is Player)
            {
                target = other;
                People people = obj as People;
                people.see_player = true;
            }
        }

        // 親のSight中に入ったGameObjectを取得する
        static public GameObject GetTarget()
        {
            return target;
        }
    }
}
