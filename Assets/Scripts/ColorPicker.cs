using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    GameObject content;
    Transform[] contentChildren;

    [Header("Slide")]
    public GameObject panel;
    RectTransform rt;

    [Header("ColorPicker")]
    public GameObject colors;
    public Button setColorBtn;
    public ColorStore colorStore;
    Texture2D ColorTexture;
    bool colorChanged;

    [Header("Other")]
    public Camera cam;
    public Text DebugText;
    public ContentController contentController;

    void Start()
    {
        content = GameObject.Find("Content");

        rt = GetComponent<RectTransform>();

        ColorTexture = GetComponent<Image>().mainTexture as Texture2D;

        colorChanged = false;
    }

    void Update()
    {
        Vector2 curMousePos = Input.mousePosition;

        if (setColorBtn.GetComponent<RectTransform>().rect.Contains(curMousePos) && Input.GetMouseButtonDown(1))
        {
            if (colorChanged == false)
            {
                // Saving information about all colors
                SaveColorInfo();
            }
        }
    }

    void SaveColorInfo()
    {
        List<Color> currentColors = new List<Color>();

        contentChildren = content.GetComponentsInChildren<Transform>();

        foreach (Transform t in contentChildren)
        {
            if (t.tag == "Panel")
            {
                currentColors.Add(t.gameObject.GetComponent<Image>().color);
            }
        }

        colorStore.savedColors = currentColors;
    }

    void LateUpdate()
    {
        // Finding new Color to apply, debugging during development 
        Vector2 delta;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, cam, out delta);

        string debug = "mousePosition = " + Input.mousePosition;
        debug += "\ndelta = " + delta;

        float width = rt.rect.width;
        float height = rt.rect.height;

        delta += new Vector2(width * 0.5f, height * 0.5f);
        debug += "\noffset delta = " + delta;

        float x = Mathf.Clamp(delta.x / width, 0f, 1f);
        float y = Mathf.Clamp(delta.y / height, 0f, 1f);
        debug += "\nx = " + x;
        debug += "\ny = " + y;

        int textureX = Mathf.RoundToInt(x * ColorTexture.width);
        int textureY = Mathf.RoundToInt(y * ColorTexture.height);
        debug += "\ntextureX = " + textureX;
        debug += "\ntextureY = " + textureY;

        Color color = ColorTexture.GetPixel(textureX, textureY);

        // DebugText.text = debug;
        // DebugText.color = color;

        Vector2 curMousePos = Input.mousePosition;

        if (setColorBtn.GetComponent<RectTransform>().rect.Contains(curMousePos) && Input.GetMouseButtonDown(1))
        {
            // Applying new color
            SetNewColor(color);
        }
    }
    
    void SetNewColor(Color iColor)
    {        
        List<GameObject> childrenGO = new List<GameObject>();

        contentChildren = content.GetComponentsInChildren<Transform>();

        foreach (Transform t in contentChildren)
        {
            if (t.tag == "Panel")
            {
                childrenGO.Add(t.gameObject);
            }
        }

        colorChanged = true;

        panel.GetComponent<Image>().color = iColor;

        for (int j = 0; j < childrenGO.Count; j++)
        {
            if (childrenGO[j] != contentController.hitGameObject)
            {
                childrenGO[j].GetComponent<Image>().color = colorStore.savedColors[j];
            }
        }

        colorChanged = false;
    }
}
