using DxLibDLL;

namespace sumisumo
{
    public static class Image
    {
        public static int player;                        // プレイヤー
        public static int guardman;                      // 警備員
        public static int people;                        // 一般人
        public static int test_shiitake;                 // しいたけ
        public static int test_playerShot;               // プレイヤーの弾
        public static int[] test_zentaman = new int[22]; // ゼンタマン
        public static int[] test_mapchip = new int[128]; // マップチップ(地形・背景)画像
        public static int play_bg;                       // プレイ画面の背景
        public static int goMenu;                        // メインメニューに戻る
        public static int retry;                         // リトライ
        public static int nextStage;                     // 次のステージへ
        public static int cursor;                        // カーソル
        public static int[] number = new int[12];        // 数字のフォント
        public static int heart;                         // ハート
        public static int downStairs;                    // 下り階段
        public static int upStairs;                      // 上り階段
        public static int stage1_buck;                   // ステージ１の背景
        public static int fireHydrant;                   // 消火栓
        public static int dressingRoom_open;             // 試着室(開)
        public static int dressingRoom_close;            // 試着室(閉)
        public static int gimmicksign;                   // ギミック発見マーク
        public static int[] cooltimeGauge = new int[31]; // クールタイムゲージ

        public static void Load()
        {
            player      = DX.LoadGraph("res/Image/player.png");
            guardman    = DX.LoadGraph("res/Image/guardman.png");
            people      = DX.LoadGraph("res/Image/people.png");

            DX.LoadDivGraph("res/Image/test_zentaman.png", test_zentaman.Length, 4, 6, 60, 70, test_zentaman);
            test_playerShot = DX.LoadGraph("res/Image/test_player_shot.png");
            test_shiitake   = DX.LoadGraph("res/Image/test_shiitake.png");
            DX.LoadDivGraph("res/Image/test_mapchip.png", test_mapchip.Length, 16, 8, 32, 32, test_mapchip);

            play_bg     = DX.LoadGraph("res/Image/background.png");
            goMenu      = DX.LoadGraph("res/Image/goMain.png");
            nextStage   = DX.LoadGraph("res/Image/nextStage.png");
            retry       = DX.LoadGraph("res/Image/retry.png");
            cursor      = DX.LoadGraph("res/Image/cursor.png");
            DX.LoadDivGraph("res/Image/suuji16x32_02.png", number.Length, 12, 1, 16, 32, number);

            heart       = DX.LoadGraph("res/Image/heart.png");
            DX.LoadDivGraph("res/Image/surinukeCooltime.png", cooltimeGauge.Length, 5, 7, 48, 48, cooltimeGauge);

            fireHydrant         = DX.LoadGraph("res/Image/gimmick_hydrant.png");
            dressingRoom_open   = DX.LoadGraph("res/Image/dressingRoom_open.png");
            dressingRoom_close  = DX.LoadGraph("res/Image/dressingRoom_close.png");
            gimmicksign         = DX.LoadGraph("res/Image/gimmicksign.png");

            downStairs  = DX.LoadGraph("res/Image/downstairs.png");
            upStairs    = DX.LoadGraph("res/Image/upstairs.png");

            stage1_buck = DX.LoadGraph("res/Image/stage1_buck.png");
        }
    }
}
