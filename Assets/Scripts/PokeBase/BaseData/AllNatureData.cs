using UnityEngine;
public static class AllNatureData
{
    private static string[] natureNames = new string[]
    {
        "Hardy", "Lonely", "Adamant", "Naughty", "Brave",
        "Bold", "Docile"  , "Impish", "Lax", "Relaxed",
        "Modest", "Mild", "Bashful", "Rash", "Quiet",
        "Calm", "Gentle"  , "Careful", "Quirky", "Sassy",
        "Timid", "Hasty"  , "Jolly", "Naive", "Serious"
    };

    public static int[] VaryUp = new int[]
    {
        6,0,0,0,0,
        1,6,1,1,1,
        2,2,6,2,2,
        3,3,3,6,3,
        4,4,4,4,6
    };

    public static int[] VaryDown = new int[]
    {
        6,1,2,3,4,
        0,6,2,3,4,
        0,1,6,3,4,
        0,1,2,6,4,
        0,1,2,3,6
    };

    public static Nature GetNature()
    {
        int i = Random.Range(0, natureNames.Length);
        return new Nature(natureNames[i], VaryUp[i], VaryDown[i]);
    }

    public static Nature Instead(string name, int v)
    {
        return new Nature(name, VaryUp[v], VaryDown[v]);
    }

    //private static float[][] natureVaries = new float[][]
    //{
    //    new float[]{1f  , 1f  , 1f  , 1f  , 1f  },
    //    new float[]{1.1f, 0.9f, 1f  , 1f  , 1f  },
    //    new float[]{1.1f, 1f  , 0.9f, 1f  , 1f  },
    //    new float[]{1.1f, 1f  , 1f  , 0.9f, 1f  },
    //    new float[]{1.1f, 1f  , 1f  , 1f  , 0.9f},
//
    //    new float[]{0.9f, 1.1f, 1f  , 1f  , 1f  },
    //    new float[]{1f  , 1f  , 1f  , 1f  , 1f  },
    //    new float[]{1f  , 1.1f, 0.9f, 1f  , 1f  },
    //    new float[]{1f  , 1.1f, 1f  , 0.9f, 1f  },
    //    new float[]{1f  , 1.1f, 1f  , 1f  , 0.9f},
//
    //    new float[]{0.9f, 1f  , 1.1f, 1f  , 1f  },
    //    new float[]{1f  , 0.9f, 1.1f, 1   , 1f  },
    //    new float[]{1f  , 1f  , 1f  , 1f  , 1f  },
    //    new float[]{1f  , 1f  , 1.1f, 0.9f, 1f  },
    //    new float[]{1f  , 1f  , 1.1f, 1f  , 0.9f},
//
    //    new float[]{0.9f, 1f  , 1f  , 1.1f, 1f  },
    //    new float[]{1f  , 0.9f, 1f  , 1.1f, 1f  },
    //    new float[]{1f  , 1   , 0.9f, 1.1f, 1f  },
    //    new float[]{1f  , 1f  , 1f  , 1f  , 1f  },
    //    new float[]{1f  , 1f  , 1f  , 1.1f, 0.9f},
//
    //    new float[]{0.9f, 1f  , 1f  , 1f  , 1.1f},
    //    new float[]{1f  , 0.9f, 1f  , 1f  , 1.1f},
    //    new float[]{1f  , 1f  , 0.9f, 1f  , 1.1f},
    //    new float[]{1f  , 1f  , 1f  , 0.9f, 1.1f},
    //    new float[]{1f  , 1f  , 1f  , 1f  , 1f  }
    //};
}