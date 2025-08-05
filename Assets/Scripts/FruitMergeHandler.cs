using UnityEngine;

public class FruitMergeHandler : MonoBehaviour
{
    public Enums.FruitLevel fruitLevel;

    public bool isMerging;

    public FruitControlManager fruitControlManager;
    private int currentLvl;

    private void Start()
    {
        currentLvl = (int)fruitLevel;
        fruitControlManager = FindFirstObjectByType<FruitControlManager>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isMerging)
            return;

        var mergeFruit = other.gameObject.GetComponent<FruitMergeHandler>();
        if (mergeFruit != null)
        {
            if (mergeFruit.isMerging)
                return;

            if (mergeFruit.fruitLevel == fruitLevel)
            {
                if (mergeFruit.GetInstanceID() < GetInstanceID())
                    return;

                isMerging = true;
                mergeFruit.isMerging = true;
                
                // 점수 획득
                ScoreManager.Instance.AddScore(currentLvl);

                Vector2 evolveFruitSpawnPos = (transform.position + mergeFruit.transform.position) / 2;

                var evolveIdx = currentLvl + 1;

                if (evolveIdx > 10)
                    return;

                fruitControlManager.SpawnEvolveFruit(evolveIdx, evolveFruitSpawnPos);

                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
    }

    /*
     * 유니티에서 두 객체 충돌 시 각각의 객체에서 로직이 실행되어 중복 결과물이 생성되는 현상은 매우 흔한 문제
     * 이러한 문제를 해결하기 위한 가장 일반적인 방법은 두 객체 중 하나에서만 로직을 실행할 주도권을 주는 것
     *
     * 해결 방법 : Instance ID를 이용해 객체를 구분하고 주도권을 정한다
     *
     * 객체간 ID값이 모두 다르므로 값을 비교구분하는 논리를 사용하면 된다
     *
     * A와 B충돌 시
     *
     * if(로직 실행 중)
     *      return
     *
     * if(A.InstanceID < B.InstanceID)
     * {
     *      A.로직 실행
     * }
     *
     */

    /*
 * 유니티에서 두 객체가 충돌할 때 각각의 객체에서 로직이 실행되어 결과물이 중복 생성되는 것은 매우 흔하게 겪는 문제입니다.

이 문제를 해결하는 가장 일반적이고 안정적인 방법은 두 객체 중 하나에게만 합체 로직을 실행할 '주도권'을 주는 것입니다.


해결 방법: 인스턴스 ID(Instance ID)를 이용한 주도권 정하기


모든 게임 오브젝트는 고유한 인스턴스 ID를 가지고 있습니다. 이 ID를 비교해서 더 작은 ID를 가진 객체만 합체 로직을 실행하도록 만들면, 충돌 이벤트가 양쪽에서 발생하더라도 실제 합체 코드는 단 한 번만 실행됩니다.

아래는 A라는 공에 적용할 수 있는 C# 스크립트 예제입니다.

MergeOnCollision.cs


1 using UnityEngine;
2
3 public class MergeOnCollision : MonoBehaviour
4 {
5     // 합체 후 생성될 B 프리팹을 유니티 에디터에서 할당합니다.
6     public GameObject prefabB;
7
8     // 합체 로직이 이미 실행되었는지 확인하는 플래그
9     private bool isMerging = false;
10
11     void OnCollisionEnter(Collision collision)
12     {
13         // 1. 이미 합체 과정에 있다면 중복 실행 방지
14         if (isMerging) return;
15
16         // 2. 충돌한 상대방도 같은 스크립트를 가진 'A' 공인지 확인합니다.
17         MergeOnCollision otherBall = collision.gameObject.GetComponent<MergeOnCollision>();
18         if (otherBall != null)
19         {
20             // 3. 상대방도 합체 과정에 있다면 중복 실행 방지
21             if (otherBall.isMerging) return;
22
23             // 4. 두 공의 고유 ID를 비교하여 ID가 더 낮은 쪽이 합체를 주도합니다.
24             //    이것이 핵심입니다. 이 조건 덕분에 코드는 단 한 번만 실행됩니다.
25             if (this.gameObject.GetInstanceID() < collision.gameObject.GetInstanceID())
26             {
27                 // 합체 시작을 양쪽에 알립니다.
28                 this.isMerging = true;
29                 otherBall.isMerging = true;
30
31                 // 5. 두 공의 중간 지점에 B를 생성합니다.
32                 Vector3 spawnPosition = (this.transform.position + collision.transform.position) / 2;
33                 Instantiate(prefabB, spawnPosition, Quaternion.identity);
34
35                 // 6. 두 개의 A 공을 모두 파괴합니다.
36                 Destroy(collision.gameObject);
37                 Destroy(this.gameObject);
38             }
39         }
40     }
41 }


적용 방법


1. 스크립트 생성: 위 MergeOnCollision.cs 스크립트를 프로젝트에 추가합니다.
2. A 공 프리팹 설정:
   * A 공으로 사용할 프리팹(또는 게임 오브젝트)에 위 MergeOnCollision 스크립트를 추가합니다.
   * Rigidbody와 Collider 컴포넌트가 반드시 있어야 합니다.
   * 스크립트의 Prefab B 필드에 합체 후 생성될 B 공의 프리팹을 드래그 앤 드롭으로 할당합니다.
3. B 공 프리팹 준비:
   * 합체된 결과물인 B 공의 프리팹을 만들어 둡니다.

왜 이 방법이 좋은가요?


* 안정성: GetInstanceID()는 모든 객체마다 고유하므로, 어떤 두 A 공이 충돌하더라도 항상 둘 중 하나의 ID가 더 작습니다. 따라서 조건문 안의 합체 로직은 반드시 한 번만 실행되는 것이 보장됩니다.
* 간결함: 별도의 매니저 객체 없이, 충돌하는 객체들끼리 문제를 해결하므로 구조가 간단해집니다.
* 레이스 컨디션 방지: isMerging 플래그를 추가하여, 아주 짧은 물리 프레임 사이에 발생할 수 있는 미세한 오류까지 방지하여 안정성을 더욱 높였습니다.


이제 A 공 두 개를 서로 충돌시켜보면, 정확히 하나의 B 공만 생성되는 것을 확인하실 수 있을 겁니다.
 */
}