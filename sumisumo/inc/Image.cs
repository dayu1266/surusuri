using DxLibDLL;

namespace sumisumo
{
    public static class Image
    {
        public static int player;                        // プレイヤー
        public static int guardman;                      // 警備員
        public static int[] police = new int[4];         // 警察官
        public static int people;                        // 一般人
        public static int test_shiitake;                 // しいたけ
        public static int test_playerShot;               // プレイヤーの弾
        public static int[] test_zentaman = new int[22]; // ゼンタマン
        public static int[] test_mapchip = new int[128]; // マップチップ(地形・背景)画像
        public static int play_bg;                       // プレイ画面の背景
        public static int gotitle;                       // タイトルに戻る
        public static int retry;                         // リトライ
        public static int stageselect_bg;                //ステージ選択画面の背景
        public static int nextStage;                     // 次のステージへ
        public static int cursor;                        // カーソル
        public static int[] number = new int[12];        // 数字のフォント
        public static int heart;                         // ハート
        public static int downStairs;                    // 下り階段
        public static int upStairs;                      // 上り階段
        public static int stage1_back;                   // ステージ1の背景
        public static int stage2_back;                   // ステージ2の背景
        public static int stage3_back;                   // ステージ3の背景
        public static int stage1_stageSelct;             // ステージセレクト
        public static int stage2_stageSelct;             // ステージセレクト
        public static int stage3_stageSelct;             // ステージセレクト
        public static int fireHydrant;                   // 消火栓
        public static int dressingRoom_open;             // 試着室(開)
        public static int dressingRoom_close;            // 試着室(閉)
        public static int gimmicksign;                   // ギミック発見マーク
        public static int[] cooltimeGauge = new int[31]; // クールタイムゲージ
        public static int gameclear;                     // ゲームクリア画面の背景
        public static int laststageclear;                // 全クリ画面
        public static int titlelogo;                     // タイトルのロゴ
        public static int gamestart;                     // ゲームスタートボタン   
        public static int selectArrowR;
        public static int selectArrowL;
        public static void Load()
        {
            player             = DX.LoadGraph("res/Image/player.png");
            guardman           = DX.LoadGraph("res/Image/guardman.png");
            DX.LoadDivGraph("res/Image/policeanim.png", police.Length, 4, 1, 100, 140, police);
            people             = DX.LoadGraph("res/Image/people.png");

            DX.LoadDivGraph("res/Image/test_zentaman.png", test_zentaman.Length, 4, 6, 60, 70, test_zentaman);
            test_playerShot    = DX.LoadGraph("res/Image/test_player_shot.png");
            test_shiitake      = DX.LoadGraph("res/Image/test_shiitake.png");
            DX.LoadDivGraph("res/Image/test_mapchip.png", test_mapchip.Length, 16, 8, 32, 32, test_mapchip);

            play_bg            = DX.LoadGraph("res/Image/background.png");
            gotitle            = DX.LoadGraph("res/Image/gotitle.png");
            retry              = DX.LoadGraph("res/Image/retry.png");
            cursor             = DX.LoadGraph("res/Image/cursor.png");
            stageselect_bg     = DX.LoadGraph("res/Image/background1.png");
            DX.LoadDivGraph("res/Image/suuji16x32_02.png", number.Length, 12, 1, 16, 32, number);

            heart              = DX.LoadGraph("res/Image/heart.png");
            DX.LoadDivGraph("res/Image/surinukeCooltime.png", cooltimeGauge.Length, 5, 7, 48, 48, cooltimeGauge);

            fireHydrant        = DX.LoadGraph("res/Image/gimmick_hydrant.png");
            dressingRoom_open  = DX.LoadGraph("res/Image/dressingRoom_open.png");
            dressingRoom_close = DX.LoadGraph("res/Image/dressingRoom_close.png");
            gimmicksign        = DX.LoadGraph("res/Image/gimmicksign.png");

            downStairs         = DX.LoadGraph("res/Image/downstairs.png");
            upStairs           = DX.LoadGraph("res/Image/upstairs.png");

            stage1_back = DX.LoadGraph("res/Image/stage1_back.png");
            stage2_back = DX.LoadGraph("res/Image/stage2_back.png");
            stage3_back = DX.LoadGraph("res/Image/stage3_back.png");

            gameclear = DX.LoadGraph("res/Image/gameclear.png");
            laststageclear = DX.LoadGraph("res/Image/laststageclear.png");
            titlelogo = DX.LoadGraph("res/Image/titlelogo.png");
            gamestart = DX.LoadGraph("res/Image/gamestart.png");

            stage1_stageSelct = DX.LoadGraph("res/Image/stage1.png");
            stage2_stageSelct = DX.LoadGraph("res/Image/stage2.png");
            stage3_stageSelct = DX.LoadGraph("res/Image/stage3.png");

            selectArrowR = DX.LoadGraph("res/Image/selectArrowR.png");
            selectArrowL = DX.LoadGraph("res/Image/selectArrowL.png");
        }
    }
}
