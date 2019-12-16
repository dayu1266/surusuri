using System.Diagnostics; // Debug.Assertを使うのに必要
using System.Numerics;
using System.IO; // ファイル読み込みのために必要
using DxLibDLL;

namespace sumisumo
{
    // マップクラス。
    // 地形データの読み込みや描画、敵データの読み込み、敵生成などを行う。
    public class Map
    {
        public const int None = -1;   // 何も無いマス
        public const int Wall = 0;    // 壁
        public const int Needle = 1;    // 針、トゲ
        public const int Brick = 2;    // レンガ
        public const int Floor = 3;    // 下から抜けられる床

        public const int Width = 80;  // マップデータの横のマス数
        public const int Height = 40;   // マップデータの縦のマス数
        public const int CellSize = 32; // マップの1マスのピクセル数

        PlayScene playScene; // PlaySceneクラスの参照
        int[,] terrain; // 地形データ

        // コンストラクタ
        public Map(PlayScene playScene, string stageName)
        {
            this.playScene = playScene;
            LoadTerrain("res/Map/" + stageName + "_terrain.csv");
            LoadObjects("res/Map/" + stageName + "_object.csv");
        }

        // マップの地形データcsvファイルを読み込んで、二次元配列に格納する
        void LoadTerrain(string filePath)
        {
            terrain = new int[Width, Height]; // 2次元配列を生成

            string[] lines = File.ReadAllLines(filePath); // ファイルを行ごとに読み込む

            // 行数の検証
            Debug.Assert(lines.Length == Height, filePath + "の高さが不正です：" + lines.Length);

            for (int y = 0; y < Height; y++)
            {
                // 行をカンマで分割する
                string[] splitted = lines[y].Split(new char[] { ',' });

                // 列数の検証
                Debug.Assert(splitted.Length == Width, filePath + "の" + y + "行目の列数が不正です:" + splitted.Length);

                for (int x = 0; x < Width; x++)
                {
                    // 文字から整数に変換して、2次元配列に格納する
                    terrain[x, y] = int.Parse(splitted[x]);
                }
            }
        }

        // オブジェクト（敵キャラクターなど）のCSVを読み込む
        void LoadObjects(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath); // ファイルを行ごとに読み込む

            // 行数の検証
            Debug.Assert(lines.Length == Height, filePath + "の高さが不正です：" + lines.Length);

            for (int y = 0; y < Height; y++)
            {
                // 行をカンマで分割する
                string[] splitted = lines[y].Split(new char[] { ',' });

                // 列数の検証
                Debug.Assert(splitted.Length == Width, filePath + "の" + y + "行目の列数が不正です:" + splitted.Length);

                for (int x = 0; x < Width; x++)
                {
                    // 文字から整数に変換して、番号に応じた敵を生成する
                    int id = int.Parse(splitted[x]);

                    // -1（何も配置されていない場所）は何もしない
                    if (id == -1) continue;

                    // オブジェクトを生成・配置する
                    SpawnObject(x, y, id);
                }
            }
        }

        // オブジェクトを生成・配置する
        void SpawnObject(int mapX, int mapY, int objectID)
        {
            // 生成位置
            float spawnX = mapX * CellSize;
            float spawnY = mapY * CellSize;

            if (objectID == 1) // しいたけ
            {
                playScene.gameObjects.Add(new Shiitake(playScene, new Vector2(spawnX, spawnY)));
            }
            else if (objectID == 3)
            {
                playScene.gameObjects.Add(new UpStairs(playScene, new Vector2(spawnX, spawnY)));
            }
            else if (objectID == 4)
            {
                playScene.gameObjects.Add(new DownStairs(playScene, new Vector2(spawnX, spawnY)));
            }
            else if (objectID == 0) // プレイヤー
            {
                Player player = new Player(playScene, new Vector2(spawnX, spawnY));
                playScene.gameObjects.Add(player);
                playScene.player = player;
            }
            else if (objectID == 1) // しいたけ
            {
                playScene.gameObjects.Add(new Shiitake(playScene, new Vector2(spawnX, spawnY)));
            }
            else if (objectID == 3)
            {
                playScene.gameObjects.Add(new UpStairs(playScene, new Vector2(spawnX, spawnY)));
            }
            else if (objectID == 4)
            {
                playScene.gameObjects.Add(new DownStairs(playScene, new Vector2(spawnX, spawnY)));
            }
            else if (objectID == 16)
            {
                playScene.gameObjects.Add(new People(playScene, new Vector2(spawnX, spawnY)));
            }
            else if (objectID == 17)
            {
                playScene.gameObjects.Add(new Guardman(playScene, new Vector2(spawnX, spawnY)));
            }
            // 新しい種類のオブジェクトを作ったら、ここに生成処理を追加してください
            else
            {
                Debug.Assert(false, "オブジェクトID" + objectID + "番の生成処理は未実装です。");
            }
        }

        // 地形を描画する
        public void DrawTerrain()
        {
            // ステージ背景の描画
            Camera.DrawGraph(96, 320, Image.stage1_buck);

            // 画面内のマップのみ描画するようにする
            int left = (int)(Camera.cameraPos.X / CellSize);
            int top = (int)(Camera.cameraPos.Y / CellSize);
            int right = (int)((Camera.cameraPos.X + Screen.Size.X - 1) / CellSize);
            int bottom = (int)((Camera.cameraPos.Y + Screen.Size.Y - 1) / CellSize);

            if (left < 0) left = 0;
            if (top < 0) top = 0;
            if (right >= Width) right = Width - 1;
            if (bottom >= Height) bottom = Height - 1;

            for (int y = top; y <= bottom; y++)
            {
                for (int x = left; x <= right; x++)
                {
                    int id = terrain[x, y];

                    if (id == None) continue; // 描画しない

                    Camera.DrawGraph(x * CellSize, y * CellSize, Image.test_mapchip[id]);
                }
            }
        }

        // 指定された座標（ワールド座標）の地形データを取得する。
        public int GetTerrain(float worldX, float worldY)
        {
            // マップ座標系（二次元配列の行と列）に変換する
            int mapX = (int)(worldX / CellSize);
            int mapY = (int)(worldY / CellSize);

            // 二次元配列の範囲外は、何もないものとして扱う
            if (mapX < 0 || mapX >= Width || mapY < 0 || mapY >= Height)
                return None;

            return terrain[mapX, mapY]; // 二次元配列から地形IDを取り出して返却する
        }

        // 指定された座標（ワールド座標）の地形が壁か調べる
        public bool IsWall(float worldX, float worldY)
        {
            int terrainID = GetTerrain(worldX, worldY); // 指定された座標の地形のIDを取得

            return terrainID == Wall; // 地形が壁ならtrue、違うならfalseを返却する
        }
    }
}
