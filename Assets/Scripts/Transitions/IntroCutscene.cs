using System.Collections;
using Loopie;

public class IntroCutscene : Component
{
    [Header("Cutscene Settings")]
    public float initialWaitTime = 1.0f;
    public float walkDuration = 3.0f;
    public Vector3 walkDirection = new Vector3(1, 0, 1);

    [Header("Cinema Bars")]
    public Entity topCinemaBar;
    public Entity bottomCinemaBar;
    public float barAnimDuration = 1.0f;
    public float barHideOffset = 200f;
    public bool invertYAxis = false;

    void OnCreate()
    {
        StartCoroutine(PlayIntroWalk());
    }

    private IEnumerator PlayIntroWalk()
    {
        yield return null;

        if (Player.Instance == null)
        {
            Debug.Log("IntroCutscene: Player.Instance is missing!");
            yield break;
        }

        Player.Instance.IsInCutscene = true;

        float waitTimer = 0f;
        while (waitTimer < initialWaitTime)
        {
            waitTimer += Time.deltaTime;
            yield return null;
        }

        float walkTimer = 0f;
        while (walkTimer < walkDuration)
        {
            Player.Instance.Input.moveDirection = walkDirection;
            walkTimer += Time.deltaTime;
            yield return null;
        }

        Player.Instance.Input.moveDirection = Vector3.Zero;

        if (topCinemaBar != null && bottomCinemaBar != null)
        {
            Vector3 topStartPos = topCinemaBar.transform.position;
            Vector3 bottomStartPos = bottomCinemaBar.transform.position;

            float directionMod = invertYAxis ? -1f : 1f;
            Vector3 topTargetPos = topStartPos + new Vector3(0, barHideOffset * directionMod, 0);
            Vector3 bottomTargetPos = bottomStartPos + new Vector3(0, -barHideOffset * directionMod, 0);

            Image topImage = topCinemaBar.GetComponent<Image>();
            Image bottomImage = bottomCinemaBar.GetComponent<Image>();

            Vector4 topStartTint = topImage != null ? topImage.GetTint() : Vector4.Zero;
            Vector4 botStartTint = bottomImage != null ? bottomImage.GetTint() : Vector4.Zero;

            float lerpTimer = 0f;
            while (lerpTimer < barAnimDuration)
            {
                lerpTimer += Time.deltaTime;
                float t = lerpTimer / barAnimDuration;

                topCinemaBar.transform.position = Vector3.Lerp(topStartPos, topTargetPos, t);
                bottomCinemaBar.transform.position = Vector3.Lerp(bottomStartPos, bottomTargetPos, t);

                if (topImage != null)
                {
                    topImage.SetTint(new Vector4(topStartTint.x, topStartTint.y, topStartTint.z, Mathf.Lerp(topStartTint.w, 0f, t)));
                }
                if (bottomImage != null)
                {
                    bottomImage.SetTint(new Vector4(botStartTint.x, botStartTint.y, botStartTint.z, Mathf.Lerp(botStartTint.w, 0f, t)));
                }

                yield return null;
            }

            topCinemaBar.SetActive(false);
            bottomCinemaBar.SetActive(false);
        }

        Player.Instance.IsInCutscene = false;
    }
}