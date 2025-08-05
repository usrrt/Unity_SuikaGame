using UnityEngine;
using UnityEngine.UI;

public class FruitControlManager : MonoBehaviour
{
    [SerializeField] private Vector2 spawnPoint;
    [SerializeField] private GameObject[] fruitPrefabs;
    [SerializeField] private GameObject dropIndicator;

    [SerializeField] private Image nextFruitPreviewImg;

    public GameObject currentFruit;
    public float mouseClampRangeX;

    public bool isGameOver;

    private int nextFruitIdx;


    private void Start()
    {
        nextFruitIdx = Random.Range(0, fruitPrefabs.Length/2);
        nextFruitPreviewImg.sprite = fruitPrefabs[nextFruitIdx].GetComponent<SpriteRenderer>().sprite;

        SpawnAndPrepareNextFruit();
    }

    private void Update()
    {
        if (isGameOver)
            return;

        if (currentFruit == null)
            return;

        FollowMouse();

        if (Input.GetMouseButtonDown(0)) DropCurrentFruit();
    }

    private void DropCurrentFruit()
    {
        // 버튼클릭시 Rigid를 추가하여 중력의 영항을 받도록한다
        currentFruit.AddComponent<Rigidbody2D>();
        currentFruit.GetComponent<Collider2D>().enabled = true;

        // 직접 떨어뜨린 과일만 다음 과일생성에 관여
        currentFruit.AddComponent<NextFruitActivator>().fruitControlManager = this;

        currentFruit = null;
        dropIndicator.SetActive(false);
    }


    private void FollowMouse()
    {
        var mouseX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        var clampX = Mathf.Clamp(mouseX, -mouseClampRangeX, mouseClampRangeX);

        currentFruit.transform.position = new Vector2(clampX, spawnPoint.y);
        dropIndicator.transform.position = new Vector2(clampX, dropIndicator.transform.position.y);
    }

    private void SpawnFruits(int idx)
    {
        currentFruit = Instantiate(fruitPrefabs[idx], spawnPoint, Quaternion.identity);
        dropIndicator.SetActive(true);
    }

    public void SpawnEvolveFruit(int idx, Vector2 position)
    {
        Debug.Log("evolve");
        var evolveFruit = Instantiate(fruitPrefabs[idx], position, Quaternion.identity);

        evolveFruit.AddComponent<Rigidbody2D>();
        evolveFruit.GetComponent<Collider2D>().enabled = true;
        evolveFruit.gameObject.tag = "DroppedFruit";
    }

    public void SpawnAndPrepareNextFruit()
    {
        // 과일 생성
        SpawnFruits(nextFruitIdx);

        // 다음 과일 인덱스를 설정
        nextFruitIdx = Random.Range(0, fruitPrefabs.Length/2);

        // 다음과일의 미리보기 갱신
        UpdateNextFruitPreview();
    }

    private void UpdateNextFruitPreview()
    {
        nextFruitPreviewImg.sprite = fruitPrefabs[nextFruitIdx].GetComponent<SpriteRenderer>().sprite;
    }
}