using UnityEngine;

public class CubeManager : MonoBehaviour
{
    // Constant values of this class.
    private const int layerMask = 512;
	private const int layer = 9;
    
    // These public fields shouldn't be used directly, use the getters instead.
    [HideInInspector]
    public Material[] materials;
    [HideInInspector]
    public Mesh[] cubes, outlines, colliders;

    // Singleton
    private static CubeManager instance;


    // This function is only executed in the editor.
    // Used to update the type of cube on variable change.
    void OnValidate()
    {
        //Create a singleton for this script
        instance = this;
    }

    void Awake()
    {
        // Avoid a warning message regarding DontDestroyOnLoad
        transform.parent = null;

        //Create a singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    } 

    // Getters for the different cube components.
    public static Material[] Materials { get => instance.materials; }
    public static Mesh[] Cubes { get => instance.cubes; }
    public static Mesh[] Outlines { get => instance.outlines; }
    public static Mesh[] Colliders { get => instance.colliders; }
    
    // Getters for cube constants.
    public static int Layer => layer;
    public static int LayerMask => layerMask;
}

public enum CubeType
{
    A,
    B,
    C,
    D,
    E,
    Count
}

public enum CubeColor
{
    Green,
    Red,
    Blue,
    Yellow,
    White,
    Count
}
