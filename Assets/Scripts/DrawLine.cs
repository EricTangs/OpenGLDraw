using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    static Material lineMaterial;
    //所有的点
    List<Vector2> allPoint=new List<Vector2>();

    Material targetMat;

    void Start()
    {
        targetMat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            allPoint.Add(Camera.main.ScreenToViewportPoint(Input.mousePosition));
        }
        if (Input.GetMouseButtonUp(0))
        {
            Maping();
        }
    }

    //映射到图片
    public void Maping()
    {
        Texture2D tempTexture = new Texture2D(200,320);
        for (int i = 1; i < allPoint.Count; i++)
        {
            Vector2 front = allPoint[i - 1];
            Vector2 back = allPoint[i];
            for (int j = 0; j < 100; j++)
            {
                float tempx = Mathf.Lerp(front.x, back.x, j / 100.0f);
                float tempy = Mathf.Lerp(front.y, back.y, j / 100.0f);
                int x = (int)(tempTexture.width * tempx);
                int y = (int)(tempTexture.height * tempy);
                tempTexture.SetPixel(x, y, Color.red);
            }
        }
        tempTexture.Apply();
        targetMat.SetTexture("_MainTex", tempTexture);
        allPoint.Clear();
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
        //正交投影
        GL.LoadOrtho();
        // Draw lines
        GL.Begin(GL.LINES);
        //Set Color
        GL.Color(Color.red);
        for (int i=1;i<allPoint.Count;i++)
        {
            Vector2 front = allPoint[i - 1];
            Vector2 back = allPoint[i];
            //起点
            GL.Vertex3(front.x, front.y, 0);
            //终点
            GL.Vertex3(back.x, back.y, 0);
        }
        GL.End();
        GL.PopMatrix();
    }
}

