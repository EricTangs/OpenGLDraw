using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriteWords : MonoBehaviour {

    static Material lineMaterial;
    List<Vector2> allPoint = new List<Vector2>();
    List<List<Vector2>> wordPoint = new List<List<Vector2>>();
    Material targetMat;
    //Count time
    float timer;
    //Open Count
    bool isCountTime;

    void Start()
    {
        targetMat = GetComponent<Renderer>().material;
    }

    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            isCountTime = false;
            allPoint.Add(Camera.main.ScreenToViewportPoint(Input.mousePosition));
        }
        if (Input.GetMouseButtonUp(0))
        {
            List<Vector2> temp = new List<Vector2>();
            isCountTime = true;
            for (int i=0;i<allPoint.Count;i++)
            {
                temp.Add(allPoint[i]);
            }
            allPoint.Clear();
            wordPoint.Add(temp);
            Maping();
        }
        if (isCountTime)
        {
            timer += Time.deltaTime;
        }
        if (timer>2)
        {
            timer = 0;
            wordPoint.Clear();
        }
    }

    //mapping
    public void Maping()
    {
        Texture2D tempTexture = new Texture2D(200, 320);
        for (int n=0;n<wordPoint.Count;n++)
        {
            for (int i = 1; i < wordPoint[n].Count; i++)
            {
                Vector2 front = wordPoint[n][i - 1];
                Vector2 back = wordPoint[n][i];
                for (int j = 0; j < 100; j++)
                {
                    float tempx = Mathf.Lerp(front.x, back.x, j / 100.0f);
                    float tempy = Mathf.Lerp(front.y, back.y, j / 100.0f);
                    int x = (int)(tempTexture.width * tempx);
                    int y = (int)(tempTexture.height * tempy);
                    tempTexture.SetPixel(x, y, Color.red);
                }
            }
        }
        tempTexture.Apply();
        targetMat.SetTexture("_MainTex", tempTexture);
    }

    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    // Will be called after all regular rendering is done
    public void OnRenderObject()
    {
        CreateLineMaterial();
        // Apply the line material
        lineMaterial.SetPass(0);
        GL.PushMatrix();
        // Set transformation matrix for drawing to
        //ortho
        GL.LoadOrtho();
        // Draw lines
        GL.Begin(GL.LINES);
        //Set Color
        GL.Color(Color.red);
        for (int i = 0; i < wordPoint.Count; i++)
        {
            for (int j=1;j<wordPoint[i].Count;j++)
            {
                Vector2 front = wordPoint[i][j - 1];
                Vector2 back = wordPoint[i][j];
                //start
                GL.Vertex3(front.x, front.y, 0);
                //end
                GL.Vertex3(back.x, back.y, 0);
            }
        }
        GL.End();
        GL.PopMatrix();
    }
}
