using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheStack : MonoBehaviour
{
    private const float BoundSize = 3.5f;
    private const float MovingBoundSize = 3f;
    private const float StackMovingSpeed = 5.0f;
    private const float BlockMovingSpeed = 3.5f;
    private const float ErrorMargin = 0.1f;

    public GameObject originBlock = null;

    private Vector3 prevBlockPosition;
    private Vector3 desiredPosition;
    private Vector3 stackBounds = new Vector3(BoundSize, 1, BoundSize);

    Transform lastBlock = null;
    float blockTransition;
    float secondaryPosition = 0f;

    int stackCount = -1;
    public int Score { get { return stackCount; } }

    int comboCount = 0;
    public int Combo { get { return comboCount; } }

    int maxCombo = 0;
    public int MaxCombo { get { return maxCombo; } }

    public Color prevColor;
    public Color nextColor;

    bool isMovingX = true;

    int bestScore = 0;
    public int BestScore { get { return bestScore; } }

    int bestCombo = 0;
    public int BestCombo { get { return bestCombo; } }

    private const string BestScoreKey = "M_Stack_BestScore";
    private const string BestComboKey = "M_Stack_BestCombo";

    private bool isGameOver = true;

    private void Start()
    {
        if (originBlock == null)
        {
            Debug.Log("OriginBlock is null");
            return;
        }

        bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
        bestCombo = PlayerPrefs.GetInt(BestComboKey, 0);

        prevColor = GetRandomColor();
        nextColor = GetRandomColor();

        prevBlockPosition = Vector3.down;

        SpawnBlock();
        SpawnBlock();
    }

    private void Update()
    {
        if (isGameOver)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (PlaceBlock())
                SpawnBlock();
            else
            {
                Debug.Log("game over");
                UpdateScore();
                isGameOver = true;
                GameOverEffect();
                Stack_UIManager.Instance.SetGameOverUI();
            }
        }

        MoveBlock();
        transform.position = Vector3.Lerp(transform.position, desiredPosition, StackMovingSpeed * Time.deltaTime);
    }

    private bool SpawnBlock()
    {
        if (lastBlock != null)
            prevBlockPosition = lastBlock.localPosition;

        GameObject newBlock = null;
        Transform newTransform = null;

        newBlock = Instantiate(originBlock);

        if (newBlock == null)
        {
            Debug.Log("new block is null");
            return false;
        }

        ColorChange(newBlock);

        newTransform = newBlock.transform;
        newTransform.parent = this.transform;
        newTransform.localPosition = prevBlockPosition + Vector3.up;
        newTransform.localRotation = Quaternion.identity;
        newTransform.localScale = stackBounds;

        stackCount++;

        desiredPosition = Vector3.down * stackCount;
        blockTransition = 0f;

        lastBlock = newTransform;

        isMovingX = !isMovingX;

        Stack_UIManager.Instance.UpdateScore();

        return true;
    }

    private bool PlaceBlock()
    {
        Vector3 lastPosition = lastBlock.position;

        if (isMovingX)
        {
            float deltaX = prevBlockPosition.x - lastPosition.x;
            bool isNegativeNum = (deltaX < 0f) ? true : false;

            deltaX = Mathf.Abs(deltaX);

            if (deltaX > ErrorMargin)
            {
                stackBounds.x -= deltaX;

                if (stackBounds.x <= 0f)
                    return false;

                float middle = (prevBlockPosition.x + lastPosition.x) / 2;
                lastBlock.localScale = new Vector3(stackBounds.x, 1, stackBounds.z);

                Vector3 tempPosition = lastBlock.localPosition;
                tempPosition.x = middle;
                lastBlock.localPosition = lastPosition = tempPosition;

                float rubbleHalfScale = deltaX / 2f;
                CreateRubble(
                    new Vector3(
                        isNegativeNum
                        ? (lastPosition.x + stackBounds.x / 2) + rubbleHalfScale
                        : (lastPosition.x - stackBounds.x / 2) - rubbleHalfScale
                        , lastPosition.y
                        , lastPosition.z),
                    new Vector3(deltaX, 1, stackBounds.z));

                comboCount = 0;
            }
            else
            {
                ComboCheck();
                lastBlock.localPosition = prevBlockPosition + Vector3.up;
            }
        }
        else
        {
            float deltaZ = prevBlockPosition.z - lastPosition.z;
            bool isNegativeNum = (deltaZ < 0f) ? true : false;

            deltaZ = Mathf.Abs(deltaZ);

            if (deltaZ > ErrorMargin)
            {
                stackBounds.z -= deltaZ;

                if (stackBounds.z <= 0f)
                    return false;

                float middle = (prevBlockPosition.z + lastPosition.z) / 2;
                lastBlock.localScale = new Vector3(stackBounds.x, 1, stackBounds.z);

                Vector3 tempPosition = lastBlock.localPosition;
                tempPosition.z = middle;
                lastBlock.localPosition = lastPosition = tempPosition;

                float rubbleHalfScale = deltaZ / 2f;
                CreateRubble(
                    new Vector3(
                        lastPosition.x
                        , lastPosition.y
                        , isNegativeNum
                        ? (lastPosition.z + stackBounds.z / 2) + rubbleHalfScale
                        : (lastPosition.z - stackBounds.z / 2) - rubbleHalfScale),
                    new Vector3(stackBounds.x, 1, deltaZ));

                comboCount = 0;

            }
            else
            {
                ComboCheck();
                lastBlock.localPosition = prevBlockPosition + Vector3.up;
            }
        }

        secondaryPosition = (isMovingX) ? lastBlock.localPosition.x : lastBlock.localPosition.z;
        return true;
    }

    private void MoveBlock()
    {
        blockTransition += Time.deltaTime * BlockMovingSpeed;

        float movePosition = Mathf.PingPong(blockTransition, BoundSize) - BoundSize / 2;

        if (isMovingX)
            lastBlock.localPosition = new Vector3(movePosition * MovingBoundSize, stackCount, secondaryPosition);
        else
            lastBlock.localPosition = new Vector3(secondaryPosition, stackCount, -movePosition * MovingBoundSize);
    }

    private void ColorChange(GameObject go)
    {
        Color applyColor = Color.Lerp(prevColor, nextColor, (stackCount % 11) / 10f);

        Renderer rn = go.GetComponent<Renderer>();

        if (rn == null)
        {
            Debug.Log("renderer is null");
            return;
        }

        rn.material.color = applyColor;
        Camera.main.backgroundColor = applyColor - new Color(0.1f, 0.1f, 0.1f);

        if (applyColor.Equals(nextColor))
        {
            prevColor = nextColor;
            nextColor = GetRandomColor();
        }
    }

    private Color GetRandomColor()
    {
        float r = Random.Range(100f, 250f) / 255;
        float g = Random.Range(100f, 250f) / 255;
        float b = Random.Range(100f, 250f) / 255;

        return new Color(r, g, b);
    }

    private void CreateRubble(Vector3 position, Vector3 scale)
    {
        GameObject go = Instantiate(lastBlock.gameObject);
        go.transform.parent = this.transform;

        go.transform.localPosition = position;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = scale;

        go.AddComponent<Rigidbody>();
        go.name = "Rubble";
    }

    private void ComboCheck()
    {
        comboCount++;

        if (comboCount > maxCombo)
            maxCombo = comboCount;

        if ((comboCount % 5) == 0)
        {
            Debug.Log("5 combo success");
            stackBounds += new Vector3(0.5f, 0f, 0.5f);
            stackBounds.x = (stackBounds.x > BoundSize) ? BoundSize : stackBounds.x;
            stackBounds.z = (stackBounds.z > BoundSize) ? BoundSize : stackBounds.z;
        }
    }

    private void UpdateScore()
    {
        if (bestScore < stackCount)
        {
            Debug.Log("최고 점수 갱신");
            bestScore = stackCount;
            bestCombo = maxCombo;

            PlayerPrefs.SetInt(BestScoreKey, bestScore);
            PlayerPrefs.SetInt(BestComboKey, bestCombo);
        }
    }

    private void GameOverEffect()
    {
        int childCount = transform.childCount;

        for (int i = 1; i < 20; i++)
        {
            if (childCount < i)
                break;

            GameObject go = transform.GetChild(childCount - i).gameObject;

            if (go.name.Equals("Rubble"))
                continue;

            Rigidbody rigid = go.AddComponent<Rigidbody>();
            rigid.AddForce(
                (Vector3.up * Random.Range(0, 10f))
                + (Vector3.right * (Random.Range(0, 10f) - 5f)
                * 100f));
        }
    }

    public void Restart()
    {
        int childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
            Destroy(transform.GetChild(i).gameObject);

        isGameOver = false;

        lastBlock = null;
        desiredPosition = Vector3.zero;
        stackBounds = new Vector3(BoundSize, 1, BoundSize);

        stackCount = -1;
        isMovingX = true;
        blockTransition = 0f;
        secondaryPosition = 0f;

        comboCount = 0;
        maxCombo = 0;

        prevBlockPosition = Vector3.down;

        prevColor = GetRandomColor();
        nextColor = GetRandomColor();

        SpawnBlock();
        SpawnBlock();
    }
}
