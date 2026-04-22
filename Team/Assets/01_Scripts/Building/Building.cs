using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private int width = 1;
    [SerializeField] private int height = 1;
    [SerializeField] private bool movable = true;

    public int Width => width;
    public int Height => height;
    public bool bMovable => bMovable;
}
