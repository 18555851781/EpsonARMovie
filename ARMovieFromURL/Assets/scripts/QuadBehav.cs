using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;//File
using DLLTestVS;

public class QuadBehav : MonoBehaviour 
{
    
    GameObject imgSrcQuad;
 
 	GCHandle pixelHandle;
     
	ObjOnPic lemoCar;
	ObjOnPic lemoCar2;
	ObjOnPic lemoCar3;
    ObjOnPic lemoCar4;
    ObjOnPic lemoCar5;

	PostCamView camview;

	CommonTools commontools;

    private string bundlename = "com.Test.ARMovieFormURL";

	int markersNUM = 5;

#if (UNITY_ANDROID)&& !UNITY_EDITOR
	[DllImport("uvc_camera_native_layer")]
	private static extern void StartNative(string bundlename);
	[DllImport("uvc_camera_native_layer")]
	private static extern bool 		UpdateNative( IntPtr  dataArray,
	int width, int height, float focus, 
	bool showvideo,//if need close IMU option open here
	ref float pcx, ref float pcy, ref float pcz,

	float[] pa,  float[] pb,  float[] pc,
	float[] pra, float[] prb, float[] prc,
	int[]  pIfSuc);
#endif


    // Use this for initialization
	void Start () 
    {
		commontools = new CommonTools ();

		StartCoroutine (commontools.CopyFileAsyncOnAndroid());
		 
		imgSrcQuad = GameObject.Find("Quad");
         
		commontools.QuadFit(imgSrcQuad);

		camview = new PostCamView ();
		camview.Start ();
		StartCoroutine (camview.iniView(imgSrcQuad));


#if (UNITY_ANDROID)&& !UNITY_EDITOR
		StartNative(bundlename);
#endif

        lemoCar = new ObjOnPic();
        lemoCar._obj = GameObject.Find("world");
        lemoCar.Start();

		lemoCar2 = new ObjOnPic ();
		lemoCar2._obj = GameObject.Find ("world2");
		lemoCar2.Start ();

		lemoCar3 = new ObjOnPic ();
		lemoCar3._obj = GameObject.Find ("world3");
		lemoCar3.Start ();

        lemoCar4 = new ObjOnPic();
        lemoCar4._obj = GameObject.Find("world4");
        lemoCar4.Start();

        lemoCar5 = new ObjOnPic();
        lemoCar5._obj = GameObject.Find("world5");
        lemoCar5.Start();
    }
	
	// Update is called once per frame
	void Update () 
	{
		if (camview._webCamTex.width > 16 && camview._webCamTex.height>16)
        {
			camview.Update ();
			pixelHandle = GCHandle.Alloc( camview._colors, GCHandleType.Pinned );
            
            IntPtr pixelPointer = pixelHandle.AddrOfPinnedObject();
            bool bSaving = false;
            float cx = 0.0f;
            float cy = 0.0f;
            float cz = 0.0f;
			int w = camview._webCamTex.width;
			int h = camview._webCamTex.height;


			float[]  a = new float[markersNUM];
			float[]  b = new float[markersNUM];
			float[]  c = new float[markersNUM];
			float[] ra = new float[markersNUM];
			float[] rb = new float[markersNUM];
			float[] rc = new float[markersNUM];
			int[] bIfDeTrSuc = new int[markersNUM];


		//	Debug.Log ("w= " + w + " h=" + h);

#if (UNITY_ANDROID)&& !UNITY_EDITOR
			UpdateNative( pixelPointer,

			w, h, 640.0f,
			false,//if need close IMU option open here
			ref cx, ref cy, ref cz,

			a,  b,  c,
			ra, rb, rc, 
			bIfDeTrSuc
			);
#endif


            pixelHandle.Free();


			lemoCar.a = a[0];
			lemoCar.b = b[0];
			lemoCar.c = c[0];
			lemoCar.ra = ra[0];
			lemoCar.rb = rb[0];
			lemoCar.rc = rc[0];
			if(bIfDeTrSuc[0]==1)
				lemoCar.bIfDeTrSuc = true;
			else
				lemoCar.bIfDeTrSuc = false;

			lemoCar2.a = a[1];
			lemoCar2.b = b[1];
			lemoCar2.c = c[1];
			lemoCar2.ra = ra[1];
			lemoCar2.rb = rb[1];
			lemoCar2.rc = rc[1];
			if(bIfDeTrSuc[1]==1)
				lemoCar2.bIfDeTrSuc = true;
			else
				lemoCar2.bIfDeTrSuc = false;

			lemoCar3.a = a[2];
			lemoCar3.b = b[2];
			lemoCar3.c = c[2];
			lemoCar3.ra = ra[2];
			lemoCar3.rb = rb[2];
			lemoCar3.rc = rc[2];
			if(bIfDeTrSuc[2]==1)
				lemoCar3.bIfDeTrSuc = true;
			else
				lemoCar3.bIfDeTrSuc = false;
            
            lemoCar4.a = a[3];
            lemoCar4.b = b[3];
            lemoCar4.c = c[3];
            lemoCar4.ra = ra[3];
            lemoCar4.rb = rb[3];
            lemoCar4.rc = rc[3];
            if (bIfDeTrSuc[3] == 1)
                lemoCar4.bIfDeTrSuc = true;
            else
                lemoCar4.bIfDeTrSuc = false;
            
            lemoCar5.a = a[4];
            lemoCar5.b = b[4];
            lemoCar5.c = c[4];
            lemoCar5.ra = ra[4];
            lemoCar5.rb = rb[4];
            lemoCar5.rc = rc[4];
            if (bIfDeTrSuc[4] == 1)
                lemoCar5.bIfDeTrSuc = true;
            else
                lemoCar5.bIfDeTrSuc = false;



            lemoCar.Update();
			lemoCar2.Update ();
			lemoCar3.Update ();
            lemoCar4.Update();
            lemoCar5.Update();

			lemoCar._obj.SetActive (lemoCar.bIfDeTrSuc);	
			lemoCar2._obj.SetActive (lemoCar2.bIfDeTrSuc);
			lemoCar3._obj.SetActive (lemoCar3.bIfDeTrSuc);
            lemoCar4._obj.SetActive(lemoCar4.bIfDeTrSuc);
            lemoCar5._obj.SetActive(lemoCar5.bIfDeTrSuc);


            lemoCar._obj.transform.Rotate(90, 0, 0);
			lemoCar2._obj.transform.Rotate(90,0,0);//if still world close here 1
			lemoCar3._obj.transform.Rotate(90,0,0);//if still world close here 1
            lemoCar4._obj.transform.Rotate(90, 0, 0);
            lemoCar5._obj.transform.Rotate(90, 0, 0);
            lemoCar._obj.transform.Translate(0, 0, 0);
        }
	}
}
