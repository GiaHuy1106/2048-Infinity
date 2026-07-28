using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;


public class Tile : MonoBehaviour
{
    public TileColorData state { get; private set; }
    public TileCell cell { get; private set; }
    public int number { get; private set; }
    public bool locked { get; set; }

    private Image background;
    private TextMeshProUGUI text;

    private void Awake()
    {
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetState(TileColorData state, int number)
    {
        this.state = state;
        this.number = number;

        background.color = state.BackGroundColor;
        text.color = state.TextColor;
        text.text = number.ToString(); //convert int to string
    }

    public void Spawn (TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        AudioManager.Instance.PlayTileSpawn();

        this.cell = cell;
        this.cell.tile = this;

        transform.position = cell.transform.position; //

        StartCoroutine(AnimatePopup());
    }

    public void MoveTo(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        AudioManager.Instance.PlayTileMove();

        this.cell = cell;
        this.cell.tile = this;

        StartCoroutine(AnimateMove(cell.transform.position, false));
    }

    public void Merged(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        AudioManager.Instance.PlayTileMerge();

        this.cell = null;
        cell.tile.locked = true;

        StartCoroutine(AnimateMove(cell.transform.position, true));
    }

    private IEnumerator AnimateMove(Vector3 to, bool merging)
    {
        float elapsed = 0f;
        float duration = 0.1f;

        Vector3 from = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsed/duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = to;

        if (merging)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator AnimatePopup()
    {
        float duration = 0.15f;

        float elapsed = 0f;

        transform.localScale = Vector3.zero;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / duration;

            if (t < 0.6f)
            {
                transform.localScale = Vector3.Lerp(
                    Vector3.zero,
                    Vector3.one * 1.15f,
                    t / 0.6f
                );
            }
            else
            {
                transform.localScale = Vector3.Lerp(
                    Vector3.one * 1.15f,
                    Vector3.one,
                    (t - 0.6f) / 0.4f
                );
            }

            yield return null;
        }

        transform.localScale = Vector3.one;
    }
}
