using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UserStage : MakeRoad
{
    //Moji_dispのコードを生かすために、継承
    //setroadは最初に一回呼ぶだけにしよう。つまり既存checkpointは使用させない
    public StartCtrl strtctrl;
    public GameObject lcanvas;
    public GameObject starttext;
    public GameObject lightb;
    public GameObject android;
    public AndroidCtrl adcrl;
    public GameObject cone, section, stop, streetlamp;
    public bool pc;
    string stagestr;
    string[] data;
    public InputField inputField;
    public Text laptext; public static Text ltext;
    public Text buttontext;

    private bool err;
    private float scale;

    // Start is called before the first frame update
    void Start()
    {
        err = false;
        ltext = laptext;
        scale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //UserStageのSetRoad オーバーライドはしていない点注意
    public void SetRoad()
    {
        for (int i = 0; i < data.Length; i++)
        {
            GameObject Endobj,road;

            string roaddata = Loader(i);
            if (roaddata == null)
                continue;

            road = Interpreter(roaddata);


            if (road == null)
            {//err
                err = true;
                buttontext.text = "Reset";
                break;
            }
            if (road == this.gameObject)
                continue;

            if (VS) Instantiate(Target, transform.position, transform.rotation);


            Endobj = road.transform.Find("End").gameObject;
            transform.position = Endobj.transform.position;
            transform.rotation = Endobj.transform.rotation;

            if (roaddata[2] == '1')
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);

        }
    }

    //読み込んだデータから、置くべきオブジェクトを返す
    GameObject Interpreter(string str)
    {
        GameObject putroad=null;
        //途中でエラー時は表示してボタン復活
        
        //構文エラー ただし特殊コマンドをのぞく
        if (str.Length != 3)
        {
            if (str.Equals("#STOP"))
            {
                Debug.Log("stop");
                return Instantiate(stop, transform.position, transform.rotation);
            }
            else if (str.Equals("#SECTION"))
            {
                Debug.Log("section");
                return Instantiate(section, transform.position, transform.rotation);
            }
            else if (str.Equals("#LAMPL"))
            {
                Debug.Log("section");
                return Instantiate(streetlamp, transform.position, transform.rotation);
            }
            else if (str.Equals("#LAMPR"))
            {
                Debug.Log("section");
                GameObject p = Instantiate(streetlamp, transform.position, transform.rotation);
                Vector3 size = p.transform.localScale;
                size.y *= -1;
                p.transform.localScale = size;
                return p;
            }
            else if (str.Equals("#CONE"))
            {
                Debug.Log("cone");
                return Instantiate(cone, transform.position, transform.rotation);
            }
            else if (str.Substring(0, 6).Equals("#SCALE"))
            {
                float s;
                if(float.TryParse(str.Substring(6, str.Length-6), out s))
                {
                    scale = s/100;
                    Debug.Log(scale);
                    return this.gameObject;
                }
                else
                {
                    ErrPrint(1, str);
                    Debug.Log("syntax err");
                    return null;
                }
            }
            else if (str.Substring(0, 5).Equals("#POFS"))
            {
                float ofs;
                if (float.TryParse(str.Substring(6, str.Length - 6), out ofs))
                {
                    Debug.Log(ofs);
                    Vector3 pos = transform.position;
                    switch (str[5])
                    {
                        case 'X': pos.x += ofs; break;
                        case 'Y': pos.y += ofs; break;
                        case 'Z': pos.z += ofs; break;
                        default:
                            ErrPrint(1, str);
                            Debug.Log("syntax err");
                            return null;

                    }
                    transform.position = pos;
                    return this.gameObject;
                }
                else
                {
                    ErrPrint(1, str);
                    Debug.Log("syntax err");
                    return null;
                }
            }
            else if (str.Substring(0, 5).Equals("#ROFS"))
            {
                float ofs;
                if (float.TryParse(str.Substring(6, str.Length - 6), out ofs))
                {
                    Debug.Log(ofs);
                    Vector3 rot = transform.eulerAngles;
                    switch (str[5])
                    {
                        case 'X': rot.x += ofs; break;
                        case 'Y': rot.y += ofs; break;
                        case 'Z': rot.z += ofs; break;
                        default:
                            ErrPrint(1, str);
                            Debug.Log("syntax err");
                            return null;

                    }
                    transform.eulerAngles = rot;
                    return this.gameObject;
                }
                else
                {
                    ErrPrint(1, str);
                    Debug.Log("syntax err");
                    return null;
                }
            }
            else
            {
                if (str[0] == '$')
                    JudgeKey(str);
                ErrPrint(1, str);
                Debug.Log("syntax err");
                return null;
            }
        }

        //up down の判定ののち どのパーツを返すかroadにをきめ、

        Debug.Log(str);
        if (str[2] == '0')//down
        {
            switch (str[0])
            {
                case '0': putroad = Road[0]; break;
                case '1': putroad = Road[1]; break;
                case '2': putroad = Road[2]; break;
                case '3': putroad = Road[3]; break;
                case '9': putroad = Road[8]; break;
                default://未定義エラー
                    ErrPrint(0, str);
                    Debug.Log("data err");
                    return null;
            }
        }
        else if(str[2] == '1')//up
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y+180, transform.eulerAngles.z);
            switch (str[0])
            {
                case '0': putroad = Road[4]; break;
                case '1': putroad = Road[5]; break;
                case '2': putroad = Road[6]; break;
                case '3': putroad = Road[7]; break;
                case '9': putroad = Road[8]; break;
                default://未定義エラー
                    ErrPrint(0, str);
                    Debug.Log("data err");
                    return null;
            }
        }
        else
        {
            ErrPrint(0, str);
            Debug.Log("data err");
            return null;
        }

        GameObject put = Instantiate(putroad, transform.position, transform.rotation);

        //もし、反転の必要があるのなら返すものを反転させて
        if (str[1] == '1')
        {
            Vector3 size = put.transform.localScale;
            size.y *= -1;
            put.transform.localScale = size;
        }

        Vector3 siz = put.transform.localScale;
        siz.y *= scale;
        siz.x *= scale;
        put.transform.localScale = siz;


        return put;
    }

    public void LoadButton()
    {
        if (err)//reset button
        {
            SceneManager.LoadScene("loadstage");
            return;
        }
        //文字列を解釈し、int配列に入れる　ボタンを消しその後setroadを呼びレース開始
        stagestr = inputField.text;
        data = stagestr.Split(',');
        SetRoad();
        if (!err)
        {
            Debug.Log("ok");
            lightb.SetActive(true);
            starttext.SetActive(true);
            strtctrl.enabled = true;
            lcanvas.SetActive(false);
            if (!pc)
                android.SetActive(true);
            //adcrl.Start();
        }
    }

    void ErrPrint(int i,string s)
    {
        string e;
        switch (i)
        {
            case 0:
                e = "data err 解釈不能な命令";
                break;
            case 1:
                e = "syntax err 文法エラー";
                break;
            case 9:
                e = "get coin";
                break;
            default:
                e = "unknow err 不明エラー";
                break;
        }

        inputField.text = e + "\n >" + s + "\n\n" + "Resetを押してください。\nPush Reset button.";
        inputField.interactable = false;
    }

    string Loader(int i)
    {
        /*読込み仕様
         * カンマ区切りで読み込んだものを
         * 以下は動的に対応
         * 先頭末尾の無効文字無視 https://dobon.net/vb/dotnet/string/trim.html
         * 空文字列も無視
         * 構文は(map)(lr)(udn)
         * コメント記述は#スタートの1命令とする: #xxx,
         * 
         * */

        data[i] = data[i].Trim();
        if (data[i].Equals(""))
            return null;
        if (data[i][0] == '%')
            return null;

        Debug.Log(data[i]);
        return data[i];
    }

    public void WikiButton()
    {
        Application.OpenURL("https://otegarut.wiki.fc2.com/wiki/%E4%BD%BF%E3%81%84%E6%96%B9");
    }

    public void DataPark()
    {
        Application.OpenURL("https://otegarut.wiki.fc2.com/wiki/%E3%82%B9%E3%83%86%E3%83%BC%E3%82%B8%E3%83%87%E3%83%BC%E3%82%BF%E5%85%AC%E9%96%8B%E5%BA%83%E5%A0%B4%28%E7%B7%A8%E9%9B%86%E5%8F%AF%E8%83%BD%29");
    }

    void JudgeKey(string s)
    {
        int[] hash = { 13, 10, 9, 23, 37, 4, 22, 2, 3, 7, 30 };

        if (PlayerPrefs.HasKey("code") && PlayerPrefs.GetString("code").Equals(s))
        {
            return;
        }

        Debug.Log("judge key");
        int x = 0;
        for (int i = 1; i < s.Length-1; i++)
        {
            x += s[i];
            if (s[i] % hash[i - 1] != 0)
                return;
        }
        if ((x % 26 + 65) != s[s.Length-1])
            return ;

        ErrPrint(9, "");
        Debug.Log("get coin");
        int c = PlayerPrefs.GetInt("money");
        PlayerPrefs.SetInt("money", c + 100000);
        PlayerPrefs.SetString("code", s);
        SceneManager.LoadScene("garage");

    }

    /* java code
     *	public String makeKey(int i) {
		String s = "";
		int x=0;
		for(int p=0;p<i-1;p++) {
			while(true) {
				int r = random.nextInt(26) + 65 + random.nextInt(2)*32;
				if(judgeKey(p,(char)r)) {
					x+=r;
					s += (char)r;
					break;
				}
			}
		}
		s+=(char)(x%26 +65);
		return s;
	}*/

}
