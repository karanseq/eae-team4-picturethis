using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PictureSelect : MonoBehaviour {

    [SerializeField]
    GameObject scrollContent = null;
    [SerializeField]
    RawImage galleryImagePrefab = null;

	// Use this for initialization
	void Start () {
        /*List<string> imagePaths = GetAllGalleryImagePaths();
        CreateGalleryImages(imagePaths);*/
        PuzzleInfoInstance.Instance.pictureName = "";
	}

    public void OnPictureSelected(string pictureName)
    {
        Debug.Log("Picture selected:" + pictureName);
        PuzzleInfoInstance.Instance.pictureName = pictureName;
        SceneManager.LoadScene("PuzzleInfo");
    }

    private List<string> GetAllGalleryImagePaths()
    {
        List<string> results = new List<string>();
        HashSet<string> allowedExtensions = new HashSet<string>() { ".png", ".jpg", ".jpeg" };

        try
        {
            AndroidJavaClass mediaClass = new AndroidJavaClass("android.provider.MediaStore$Images$Media");
            const string dataTag = "_data";

            string[] projection = new string[] { dataTag };
            AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = player.GetStatic<AndroidJavaObject>("currentActivity");

            string[] urisToSearch = new string[] { "EXTERNAL_CONTENT_URI", "INTERNAL_CONTENT_URI" };
            foreach (string uriToSearch in urisToSearch)
            {
                AndroidJavaObject externalUri = mediaClass.GetStatic<AndroidJavaObject>(uriToSearch);
                AndroidJavaObject finder = currentActivity.Call<AndroidJavaObject>("managedQuery", externalUri, projection, null, null, null);
                bool foundOne = finder.Call<bool>("moveToFirst");
                while (foundOne)
                {
                    int dataIndex = finder.Call<int>("getColumnIndex", dataTag);
                    string data = finder.Call<string>("getString", dataIndex);
                    if (allowedExtensions.Contains(Path.GetExtension(data).ToLower()))
                    {
                        string path = @"file:///" + data;
                        results.Add(path);
                    }

                    foundOne = finder.Call<bool>("moveToNext");
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("Exception:" + e.StackTrace);
        }

        return results;
    }

    private void CreateGalleryImages(List<string> imagePaths)
    {
        /*foreach (string path in imagePaths)
        {
            RawImage galleryImage = Instantiate(galleryImagePrefab) as RawImage;
            galleryImage.transform.position = new Vector3(100, 100);
        }*/
    }
}
