using UnityEngine;

public abstract class HexEntity : MonoBehaviour {
    public int x, y;

    public Vector3 IdealPosition => grid.HexPosition(x, y);

    protected HexGrid grid;

    protected SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        grid = FindAnyObjectByType<HexGrid>();
        transform.position = new Vector3(Random.Range(-20f, 20f), Random.Range(-20f, 20f), Random.Range(-20f, 20f));
    }

    private void Update() {
        transform.position = Vector3.Lerp(transform.position, IdealPosition, 0.05f);
        UpdateSprite();
    }

    protected virtual void UpdateSprite() { }

    protected float DistanceTo(HexEntity entity) => (entity.IdealPosition - IdealPosition).magnitude;
    protected float DistanceBetween(HexTile tile, HexEntity entity) => (tile.Position - entity.IdealPosition).magnitude;
}
