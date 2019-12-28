using UnityEngine;
using System.Runtime.InteropServices;

public class AdMax : MonoBehaviour {
    AndroidJavaObject adViewController;


    public enum Position
	{
		TOP,
		BOTTOM,
		TOP_LEFT,
		TOP_RIGHT,
		BOTTOM_LEFT,
		BOTTOM_RIGHT
	}

	[SerializeField]
	private Position position;
	[SerializeField]
	private string iPhoneAdCode;
	[SerializeField]
	private string androidAdCode;
	[SerializeField]
	private bool tracking = true;
	[SerializeField]
	public bool testMode = true;

#if UNITY_IPHONE
	[DllImport("__Internal")]
	private static extern void installAdView_ (string adCode, bool tracking, bool testMode, int position);
#endif

	// Use this for initialization
	void Start () {
		if (Application.isEditor) {
			return;
		}
		if (Application.platform == RuntimePlatform.Android) {
			this.installAdView(this.androidAdCode, this.tracking, this.testMode, this.position);
		} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
			this.installAdView(this.iPhoneAdCode, this.tracking, this.testMode, this.position);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void installAdView(string adCode, bool tracking, bool testMode, Position position) {
#if UNITY_ANDROID
		adViewController = new AndroidJavaObject("jp.shinobi.admax.unity.android.AdViewController");
		adViewController.Call("installAdView", adCode, tracking, testMode, (int)position);
        
#elif UNITY_IPHONE
		installAdView_(adCode, tracking, testMode, (int)position);
#endif
	}

}
