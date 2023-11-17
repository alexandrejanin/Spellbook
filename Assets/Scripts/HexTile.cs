using UnityEngine;

public class HexTile : MonoBehaviour {
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Material basicColor, selectedColor, clickableColor;

    public int X { get; private set; }
    public int Y { get; private set; }

    public bool selected, clickable;

    public Vector3 Position => transform.position;

    private Vector3 offset;

    public Vector3 IdealPosition => new Vector3(X + Y / 2f - Y / 2, 0, grid.innerRadius * Y) + offset;

    private HexGrid grid;

    private void Awake() {
        grid = FindAnyObjectByType<HexGrid>();

        transform.position = new Vector3(Random.Range(-20f, 20f), Random.Range(-20f, 20f), Random.Range(-20f, 20f));
        offset = Random.Range(-1, 2) / 25f * Vector3.up;
    }

    private void LateUpdate() {
        transform.position = Vector3.Lerp(transform.position, IdealPosition, 0.025f);

        foreach (var r in renderers)
            r.sharedMaterial = selected
                ? selectedColor
                : clickable
                    ? clickableColor
                    : basicColor;
    }


    public void Initialize(int x, int y) {
        X = x;
        Y = y;
    }
}
