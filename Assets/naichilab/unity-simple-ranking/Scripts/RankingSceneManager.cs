﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using NCMB;
using NCMB.Extensions;

namespace naichilab
{
	public class RankingSceneManager : MonoBehaviour
	{
		[SerializeField] Text scoreLabel;
		[SerializeField] Text highScoreLabel;
		[SerializeField] InputField nameInputField;
		[SerializeField] Button sendScoreButton;
		[SerializeField] Button closeButton;
		[SerializeField] RectTransform scrollViewContent;
		[SerializeField] GameObject rankingNodePrefab;
		[SerializeField] GameObject readingNodePrefab;
		[SerializeField] GameObject notFoundNodePrefab;
        [SerializeField] string unit;

        [SerializeField]
		private string OBJECT_ID = "objectId";
		private string _objectid = null;

        [SerializeField]
        Text ranktext;
        
        public static bool eventRankingData;

		private string ObjectID {
			get {
                return _objectid ?? (_objectid = PlayerPrefs.GetString (OBJECT_ID, null));
			}
			set {
                if (_objectid == value)
					return;
				PlayerPrefs.SetString (OBJECT_ID, _objectid = value);
            }
        }

        [SerializeField]
		private string RankingDataClassName = "RankingData";
		private NCMBObject highScoreSpreadSheetObject;

		/// <summary>
		/// 入力した名前
		/// </summary>
		/// <value>The name of the inputted.</value>
		private string InputtedNameForSave {
			get {
				if (string.IsNullOrEmpty (this.nameInputField.text)) {
					return "名無し";
				}
				return this.nameInputField.text;
			}
		}

		void Start ()
		{
			this.sendScoreButton.interactable = false;

            if (eventRankingData)
            {
                RankingDataClassName = EventScene.EventName();
            }
            else
            {
                Debug.Log("normal ranking");
            }

            StartCoroutine (GetHighScoreAndRankingBoard ());
		}

		IEnumerator GetHighScoreAndRankingBoard ()
		{
			this.scoreLabel.text = RankingLoader.Instance.Score.TextForDisplay;
				
			//ハイスコア取得
			{
				this.highScoreLabel.text = "取得中...";

				var hiScoreCheck = new YieldableNcmbQuery<NCMBObject> (RankingDataClassName);
				hiScoreCheck.WhereEqualTo (OBJECT_ID, ObjectID);
				yield return hiScoreCheck.FindAsync ();

				if (hiScoreCheck.Count > 0) {
					//既にハイスコアは登録されている
					highScoreSpreadSheetObject = hiScoreCheck.Result.First ();

					var s = RankingLoader.Instance.BuildScore (highScoreSpreadSheetObject ["hiscore"].ToString ());
					this.highScoreLabel.text = s != null ? s.TextForDisplay : "エラー";

					this.nameInputField.text = highScoreSpreadSheetObject ["name"].ToString ();
				} else { 
					//登録されていない
					this.highScoreLabel.text = "-----";
				}
			}

			//ランキング取得
			yield return StartCoroutine (LoadRankingBoard ());

			//スコア更新している場合、ボタン有効化
			if (this.highScoreSpreadSheetObject == null) {
				this.sendScoreButton.interactable = true;
			} else {
				var highScore = RankingLoader.Instance.BuildScore (this.highScoreSpreadSheetObject ["hiscore"].ToString ());
				var score = RankingLoader.Instance.Score;

				if (RankingLoader.Instance.setting.Order == ScoreOrder.OrderByAscending) {
					//数値が低い方が高スコア
					this.sendScoreButton.interactable = score.Value < highScore.Value;
				} else {
					//数値が高い方が高スコア
					this.sendScoreButton.interactable = highScore.Value < score.Value;
				}
			}

            //event時は、別に対応
            if (eventRankingData)
            {
                var score = RankingLoader.Instance.Score;
                Debug.Log("scoreN " + score.Value);
                Debug.Log("scoreLocal " + PlayerPrefs.GetInt("local" + EventScene.EventName()));
                //ローカル記録あり
                if (PlayerPrefs.HasKey("local"+EventScene.EventName()))
                {
                    this.highScoreLabel.text = PlayerPrefs.GetInt(EventScene.EventName()).ToString();
                    this.scoreLabel.text = ((int)score.Value).ToString();
                    if (score.Value> PlayerPrefs.GetInt(EventScene.EventName()))
                    {
                        //記録更新
                        if (EventScene.IsEventDay() == 1)//開催期間中なら更新可能
                            this.sendScoreButton.interactable = true;
                        else
                            this.sendScoreButton.interactable = false;
                    }
                    else
                    {
                        //記録更新せず
                        this.sendScoreButton.interactable = false;
                    }
                }
                else//初回
                {
                    if (EventScene.IsEventDay() == 1)//開催期間中なら更新可能
                        this.sendScoreButton.interactable = true;
                    else
                        this.sendScoreButton.interactable = false;
                    this.scoreLabel.text = ((int)score.Value).ToString();
                    PlayerPrefs.SetInt("local" + EventScene.EventName(), (int)score.Value);
                    this.highScoreLabel.text = "-----";
                }

                //ローカルは強制保存する
                PlayerPrefs.SetInt("local"+EventScene.EventName(), (int)score.Value);
            }
		}


		public void SendScore ()
		{
			StartCoroutine (SendScoreEnumerator ());			
		}

		private IEnumerator SendScoreEnumerator ()
		{
			this.sendScoreButton.interactable = false;
			this.highScoreLabel.text = "送信中...";

			//ハイスコア送信
			if (this.highScoreSpreadSheetObject == null) {
				this.highScoreSpreadSheetObject = new NCMBObject (RankingDataClassName);
				this.highScoreSpreadSheetObject.ObjectId = ObjectID;
			}

			this.highScoreSpreadSheetObject ["name"] = this.InputtedNameForSave;
			this.highScoreSpreadSheetObject ["hiscore"] = RankingLoader.Instance.Score.Value;
			NCMBException errorResult = null;
			
			yield return this.highScoreSpreadSheetObject.YieldableSaveAsync (error => errorResult = error);

			if (errorResult != null) {  //NCMBのコンソールから直接削除した場合に、該当のobjectIdが無いので発生する（らしい）
				highScoreSpreadSheetObject.ObjectId = null;
				yield return this.highScoreSpreadSheetObject.YieldableSaveAsync (error => errorResult = error);	//新規として送信
			}

			//ObjectIDを保存して次に備える
			ObjectID = this.highScoreSpreadSheetObject.ObjectId;
			
			this.highScoreLabel.text = RankingLoader.Instance.Score.TextForDisplay;

            //eventモードの対応
            if (eventRankingData)
            {
                PlayerPrefs.SetInt(EventScene.EventName(), (int)RankingLoader.Instance.Score.Value);
            }

			yield return StartCoroutine (LoadRankingBoard ());
		}


		/// <summary>
		/// ランキング取得＆表示
		/// </summary>
		/// <returns>The ranking board.</returns>
		private IEnumerator LoadRankingBoard ()
		{
			int nodeCount = scrollViewContent.childCount;
			Debug.Log (nodeCount);
			for (int i = nodeCount - 1; i >= 0; i--) {
				Destroy (scrollViewContent.GetChild (i).gameObject);
			}

			var msg = Instantiate (readingNodePrefab, scrollViewContent);

			//2017.2.0b3の描画されないバグ暫定対応
			MaskOffOn ();

			var so = new YieldableNcmbQuery<NCMBObject> (RankingDataClassName);
			so.Limit = 30;
			if (RankingLoader.Instance.setting.Order == ScoreOrder.OrderByAscending) {
				so.OrderByAscending ("hiscore");
			} else {
				so.OrderByDescending ("hiscore");
			}
			yield return so.FindAsync ();

			Debug.Log ("count : " + so.Count.ToString ());
			Destroy (msg);

			if (so.Count > 0) {

				int rank = 0;
                int myscore = PlayerPrefs.GetInt(EventScene.EventName());
                int myrank = -1;
				foreach (var r in so.Result) {

					var n = Instantiate (this.rankingNodePrefab, scrollViewContent);
					var rankNode = n.GetComponent<RankingNode> ();
					rankNode.NoText.text = (++rank).ToString ();
					rankNode.NameText.text = r ["name"].ToString ();

					var s = RankingLoader.Instance.BuildScore (r ["hiscore"].ToString ());
					rankNode.ScoreText.text = s != null ? unit + s.TextForDisplay : "エラー";

					Debug.Log (r ["hiscore"].ToString ());

                    if (eventRankingData && myrank ==-1)
                    {
                        int score;
                        int.TryParse(r["hiscore"].ToString(), out score);
                        if (score == myscore)
                        {
                            myrank = rank;
                            ranktext.text = myrank.ToString()+"位";
                            if (EventScene.IsEventDay() == 2)
                            {
                                GetComponent<EventResult>().GiveAward(myrank);
                                Debug.Log("-------award");
                            }
                            Debug.Log("Myrank:"+myrank);
                        }
                    }


                }
                if (eventRankingData && myrank == -1)
                {
                    ranktext.text = "--位";
                    if (EventScene.IsEventDay() == 2)
                    {
                        GetComponent<EventResult>().GiveAward(-1);
                    }
                }

            }
            else {
				Instantiate (this.notFoundNodePrefab, scrollViewContent);
			}
		}

		public void OnCloseButtonClick ()
		{
			this.closeButton.interactable = false;
			UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync ("Ranking");
		}

		private void MaskOffOn ()
		{
			//2017.2.0b3でなぜかScrollViewContentを追加しても描画されない場合がある。
			//親maskをOFF/ONすると直るので無理やり・・・
			var m = this.scrollViewContent.parent.GetComponent<Mask> ();
			m.enabled = false;
			m.enabled = true;
		}


	}
}
