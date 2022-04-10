using System;
using System.IO;

//MetaTrader5の履歴ファイルをクリプタクトのカスタムファイル形式に変換する

namespace MetaTraderToCryptact
{
    class Program
    {
        static void Main(string[] args)
        {
           
            string[] str;

            //履歴ファイル名を入力
            string Filename = "ReportHistory-******.csv";
            //出力ファイル名を入力
            string OutputName = "ReportHistory-Cryptact.csv";
            //基軸通貨を入力　USDTまたはBTC
            string Base = "USDT";
            //クリプタクト上で識別するタグ
            string Source = "FXGT";
            
            //相手通貨　基本的にはJPY
            string Counter="JPY";
            //手数料通貨　Baseと同じ
            string FeeCcy = Base;
            //コメントを入れる時はここに入力
            string Comment = "";

            //カスタムファイル形式の文字列
            string Timestamp, Action,  Volume, Price, Fee;



            if (File.Exists(Filename))//ファイルが存在する場合処理を行う
            {
                var sr = new StreamReader(Filename);//読み込みファイル

                var sw = new StreamWriter(OutputName);//書き込みファイル

                //1行目に書き込み

             //クリプタクトのカスタムファイルの形式
             //Timestamp,Action,Source,Base,Volume,Price,Counter,Fee,FeeCcy,Comment
            //（時間、売買、入力ソース、通貨、量、価格、相手通貨、手数料、手数料通貨、コメント）
     
     
                sw.WriteLine("Timestamp,Action,Source,Base,Volume,Price,Counter,Fee,FeeCcy,Comment");
             
                Console.WriteLine("start");

                int count = 0;//取引数カウント
      
                //ヘッダ部分を読み飛ばす
                for (int i = 0; i < 7; i++) sr.ReadLine();


                while (sr.Peek() >= 0)
                {

                    //csvファイルを,区切りで読み込む
                    str = sr.ReadLine().Split(',');

                    Console.WriteLine("読み込み数："+count.ToString());

                    //ポジション一覧を読み終わったら終了
                    if (str[0] == "注文") break;

                    //TimeStamp読み込み　日付形式はyyyy/mm/ddに変換
                    Timestamp = str[8].Replace(".", "/");

                    //損益読み込み
                    Volume= str[12];

                    
                    //マイナスの時は0円で売却として扱う(FXはこう扱うと公式ヘルプで書いていたので)
                    //プラスのときは時価で取得したとみなす
                    if (Volume.Contains("-"))
                    {
                        Action = "SELL";
                        Price = "0";
                    }
                    else
                    {
                        Action = "BONUS";
                        Price = "";
                    }
                    //マイナス除去
                    Volume=Volume.Replace("-","");

                    //スワップ手数料をFeeとして扱う マイナスは除去
                    Fee = str[11].Replace("-","");
                    // sw.WriteLine("Timestamp,Action,Source,Base,Volume,Price,Counter,Fee,FeeCcy,Comment");

                    //ファイルに書き込む
                    sw.WriteLine(Timestamp+","+ Action +","+ Source+","
                        + Base+","+ Volume+","+ Price+","+ Counter+","+ Fee+","+ FeeCcy+","+ Comment);

                    count++;

                }

                sw.Close();
                sr.Close();

                Console.WriteLine("end");

                Console.ReadKey();
            }
            else {

                Console.WriteLine("ファイルが存在しません");
                Console.ReadKey();

            }

            return;

        }
    }
}
