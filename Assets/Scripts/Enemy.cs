using UnityEngine;

public class Enemy : HexEntity {
    [SerializeField] private Sprite idleSprite, attackSprite;

    private float attackTime;

    public void TakeTurn() {
        if (DistanceTo(grid.Player) < 1.1f) {
            // Attack player
            attackTime = 0.5f;
            return;
        }

        var minDist = DistanceTo(grid.Player);
        HexTile bestTile = null;

        foreach (var neighbor in grid.GetNeighbors(x, y)) {
            var dist = DistanceBetween(neighbor, grid.Player);
            if (dist < minDist) {
                minDist = dist;
                bestTile = neighbor;
            }
        }

        if (bestTile != null) {
            x = bestTile.X;
            y = bestTile.Y;
            attackTime = 0f;
        }
    }

    protected override void UpdateSprite() {
        attackTime -= Time.deltaTime;

        spriteRenderer.sprite = attackTime > 0 ? attackSprite : idleSprite;
    }
}
