using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPalette : MonoBehaviour
{
    [Header("Change color by platform Variables")]
    public GameObject tutorialControlsXBOX_GO;
    public GameObject tutorialControlsPS4_GO;

    public Material[] A_Cross_Materials;
    public Material[] X_Square_Materials;
    public Material[] B_Circle_Materials;
    public Material[] Y_Triangle_Materials;
    
    bool xbox_game = false;
    bool ps4_game = false;

    void Start()
    {
        //Colores
        xbox_game = true;
        ps4_game = false;
        ChangeMaterialColor();

        //Tutorial
        if ((tutorialControlsPS4_GO == null || tutorialControlsXBOX_GO == null))
        { GetTutorialGO(); }
        ChangeTutorialGO();
        
    }

    private void Update()
    {
        if ((tutorialControlsPS4_GO == null || tutorialControlsXBOX_GO == null))
        {
            GetTutorialGO();
            ChangeTutorialGO();
        }
    }

    public void ChangeMaterialColor()
    {
        if (A_Cross_Materials.Length == 0 || 
            X_Square_Materials.Length == 0 || 
            B_Circle_Materials.Length == 0 || 
            Y_Triangle_Materials.Length == 0)
        {
            return;
        }

        if (xbox_game)
        {
            for (int i = 0; i < A_Cross_Materials.Length; i++)
            {
                //Green
                Color aux = new Color(0.3372549f, 0.8392157f, 0.427451f);
                A_Cross_Materials[i].color = aux;
            }
            for (int i = 0; i < B_Circle_Materials.Length; i++)
            {
                //Red
                Color aux = new Color(0.8666667f, 0.145098f, 0.1607843f);
                B_Circle_Materials[i].color = aux;
            }
            for (int i = 0; i < X_Square_Materials.Length; i++)
            {
                //Blue
                Color aux = new Color(0.2313726f, 0.3215686f, 0.7921569f);
                X_Square_Materials[i].color = aux;
            }
            for (int i = 0; i < Y_Triangle_Materials.Length; i++)
            {
                //Yellow
                Color aux = new Color(0.95f, 0.9f, 0.4f);
                Y_Triangle_Materials[i].color = aux;
            }
        }
        if (ps4_game)
        {
            for (int i = 0; i < A_Cross_Materials.Length; i++)
            {
                //Blue
                Color aux = new Color(0.2313726f, 0.3215686f, 0.7921569f);
                A_Cross_Materials[i].color = aux;
            }
            for (int i = 0; i < B_Circle_Materials.Length; i++)
            {
                //Red
                Color aux = new Color(0.8666667f, 0.145098f, 0.1607843f);
                B_Circle_Materials[i].color = aux;
            }
            for (int i = 0; i < X_Square_Materials.Length; i++)
            {
                //Pink
                Color aux = new Color(0.8705882f, 0.5960785f, 0.8470588f);
                X_Square_Materials[i].color = aux;
            }
            for (int i = 0; i < Y_Triangle_Materials.Length; i++)
            {
                //Green
                Color aux = new Color(0.3372549f, 0.8392157f, 0.427451f);
                Y_Triangle_Materials[i].color = aux;
            }
        }
    }

    void ChangeTutorialGO()
    {

        if (tutorialControlsPS4_GO == null || tutorialControlsXBOX_GO == null)
        { return; }

        if (xbox_game)
        {
            tutorialControlsPS4_GO.SetActive(false);
            tutorialControlsXBOX_GO.SetActive(true);
        }
        if (ps4_game)
        {
            tutorialControlsXBOX_GO.SetActive(false);
            tutorialControlsPS4_GO.SetActive(true);
        }
    }

    void GetTutorialGO()
    {
        if (GameManager.levelNumber == 0)
        {
            tutorialControlsXBOX_GO = GameObject.Find("XBOX_CUBES");
            tutorialControlsPS4_GO = GameObject.Find("PS4_CUBES");
        }
    }

    public void ChangeColorPalette()
    {
        if (xbox_game)
        {
            xbox_game = false;
            ps4_game = true;
            ChangeMaterialColor();
            ChangeTutorialGO();
        }
        else if (ps4_game)
        {
            xbox_game = true;
            ps4_game = false;
            ChangeMaterialColor();
            ChangeTutorialGO();
        }
    }
}
