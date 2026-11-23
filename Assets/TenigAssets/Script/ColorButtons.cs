using UnityEngine;

public class ColorButtons : MonoBehaviour
{
    [SerializeField] private ColorPuzzle puzzle;
    [SerializeField] private CodeColor color;

    public void Press() => puzzle.PressColor(color);
}
