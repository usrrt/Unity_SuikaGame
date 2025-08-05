using UnityEngine;

public class NextFruitActivator : MonoBehaviour
{
    public FruitControlManager fruitControlManager;

    private void OnCollisionEnter2D(Collision2D other)
    {
        gameObject.tag = "DroppedFruit";

        // 과일이 다른 과일이나 바닥과 충돌하면 새로운 과일을 생성하는 함수 호출
        // 합성된 과일은 새로운 과일을 생성할 필요가 없음
        fruitControlManager.SpawnAndPrepareNextFruit();
        Destroy(this);
    }
}