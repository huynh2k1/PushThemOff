using NaughtyAttributes;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RenderModelToSprite : MonoBehaviour
{
    public Camera renderCamera;
    public Transform parent;
    public string outputPath = "Assets/_GAMECONTAINER/Textures/sprite.png";
    public string nameFile;
    public int childCount;

    private void Start()
    {
        childCount = parent.childCount;
    }

    [Button("START")]
    public void StartRender()
    {
        GetOutputPath();

        RenderTexture renderTexture = renderCamera.targetTexture;
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);

        RenderTexture.active = renderTexture;
        renderCamera.Render();

        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(outputPath, bytes);

        Debug.Log("Sprite saved to: " + outputPath);
        RenderTexture.active = null;

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif

        Debug.Log("Sprite saved to: " + outputPath);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartRender();
        }
    }

    public void GetOutputPath()
    {
        for (int i = 0; i < childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.gameObject.activeSelf)
            {
                outputPath = $"Assets/_GAME/Sprites/{nameFile} {i}.png";
                return;
            }
        }
    }
}
