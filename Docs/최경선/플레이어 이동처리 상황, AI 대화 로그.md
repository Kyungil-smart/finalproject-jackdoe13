## 선택한 타일로 걸어가게 만드는 코드를 작성.
1. PlayerMovement.cs
2. MoveTo(Transform transform) : 함수가 실행되면
입력받은 transform.position (지정된 position의 X,Y,Z축)으로 정속 이동시작, 해당 위치에 도달하면 이동 정지하도록 구성   
1. 반드시 위 함수 하나만 쓰라는 뜻은 아님.


알게된 점

- Vector2:  
 좌표 2개(X,Y)를 사용한, 2차원 공간의 위치나 방향, 크기를 나타내는 데이터형식  
(같은 말로, Vector3는 좌표 3개 X,Y,Z 를 이용한 3차원 공간의 위치나 방향 등의 데이터 형식.)
- Vector2.MoveTowards:  
시작부터 목표점까지 지정된 속력으로 이동 시킬때 사용하는 내장 함수(Vector2가 붙었으니 2D에서 쓰는듯)
- destination: 목적지
- 


# AI 사용 내역 (GPT)

## Q.
내가 지금 유니티로 2D 보드게임을 제작중인데,  
플레이어 캐릭터가 타일단위로 이동할 수 있게 여기저기 뒤져가면서 코드를 작성했거든?  
일단 니가 보고 오 괜찮다 싶으면 말해주고, 오류날 부분이 보이면 설명해줘.  
혹은 수정해줘야 할 부분이 보인다면 팁좀 줘..
```
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 destination;

    void Start()
    {
        // 목적지 = 현재 위치
        destination = transform.position;
    }
    void Update()
    {
        // 트렌스폼의 위치를 목적지로 이동 (속도: 5f)
        transform.position = Vector2.MoveTowards(transform.position, destination, Time.deltaTime * 5f);
    }
        // 목적지 설정
    public void MoveTo(Transform transform)
    {
        // 목적지 = 트렌스폼의 위치
        if (transform != null)
        {
            destination = transform.position;
        }
    }
}
```
내가 제대로 쓴게 맞는지도 모르겄다 야 

## A.

이 코드는 **처음 만드는 것 치고는 꽤 괜찮게 짰다.** 특히 `destination`을 따로 저장해놓고 `MoveTowards`로 이동하는 방식은 2D 보드게임에서 많이 사용하는 방식 중 하나야.

하나씩 볼게.

---

## 👍 좋은 점

### 1. 목적지를 따로 저장하는 방식

```csharp
private Vector2 destination;
```

이거 좋다.

매 프레임마다 어디로 갈지 계산하는 게 아니라
"목적지만 정해놓고 계속 이동"하는 구조라 관리하기 편하다.

---

### 2. Start에서 현재 위치를 저장

```csharp
destination = transform.position;
```

이것도 좋다.

안 해두면 처음 Update에서 (0,0)으로 달려갈 수도 있으니까.

---

### 3. MoveTowards 사용

```csharp
transform.position =
    Vector2.MoveTowards(transform.position,
                        destination,
                        Time.deltaTime * 5f);
```

이게 가장 많이 쓰는 이동 방식 중 하나.

속도도 프레임과 관계없이 일정하다.

---

# 조금 아쉬운 부분

## ① 매 프레임 MoveTowards를 실행하고 있다.

도착했는데도 계속

```csharp
MoveTowards()
```

를 호출한다.

큰 문제는 아니지만 약간 낭비.

보통은

```csharp
if ((Vector2)transform.position != destination)
{
    transform.position =
        Vector2.MoveTowards(transform.position,
                            destination,
                            Time.deltaTime * 5f);
}
```

처럼 검사하기도 한다.

---

## ② 변수명이 헷갈린다.

이 부분.

```csharp
public void MoveTo(Transform transform)
```

여기서

```csharp
transform
```

은 Unity가 원래 가지고 있는

```csharp
transform
```

과 이름이 같다.

그래서

```csharp
transform.position
```

이게

> 플레이어 Transform인지

아니면

> 매개변수 Transform인지

헷갈린다.

보통은

```csharp
public void MoveTo(Transform target)
{
    destination = target.position;
}
```

이렇게 이름을 바꾼다.

읽기 훨씬 편하다.

---

## ③ 속도를 수정하기 어렵다.

지금

```csharp
Time.deltaTime * 5f
```

이렇게 하드코딩되어 있다.

나중에

* 달리기
* 버프
* 느려짐

같은 걸 만들기 힘들다.

보통은

```csharp
[SerializeField]
private float moveSpeed = 5f;
```

그리고

```csharp
Time.deltaTime * moveSpeed
```

이렇게 한다.

그러면 Inspector에서 바로 바꿀 수 있다.

---

## ④ MoveTo가 Transform만 받는다.

보드게임이면 사실

```csharp
MoveTo(Vector2 position)
```

이 더 자주 쓰인다.

예를 들어

```csharp
MoveTo(new Vector2(3,5));
```

처럼.

Transform을 굳이 넘길 필요가 없는 경우가 많다.

예를 들면

```csharp
public void MoveTo(Vector2 targetPosition)
{
    destination = targetPosition;
}
```

---

## 만약 보드게임이라면 더 추천하는 구조

예를 들어

```
□ □ □ □
□ □ □ □
□ □ □ □
```

각 칸마다

```
Tile
```

이라는 오브젝트가 있다고 치자.

그럼

```csharp
player.MoveTo(tile.transform);
```

이렇게 해도 되고

더 나아가

```csharp
player.MoveTo(tile.WorldPosition);
```

처럼 좌표만 넘기는 경우도 많다.

---

## 하나 더 추가하면 좋은 기능

도착 여부.

```csharp
public bool IsMoving()
{
    return Vector2.Distance(transform.position, destination) > 0.01f;
}
```

그러면

```csharp
if(!player.IsMoving())
{
    // 다음 턴
}
```

처럼 쓰기 좋다.

보드게임에서는 거의 필수 기능이다.

---

## 내가 하나 수정한다면 이렇게 만들겠어.

```csharp
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    private Vector2 destination;

    void Start()
    {
        destination = transform.position;
    }

    void Update()
    {
        if ((Vector2)transform.position != destination)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                destination,
                moveSpeed * Time.deltaTime
            );
        }
    }

    public void MoveTo(Transform target)
    {
        if (target != null)
        {
            destination = target.position;
        }
    }

    public bool IsMoving()
    {
        return Vector2.Distance(transform.position, destination) > 0.01f;
    }
}
```

## 전체 평가

**입문 단계 기준으로는 8.5~9/10 정도**라고 볼 수 있어.

좋았던 점은 "목적지를 저장하고 Update에서 이동"이라는 구조를 제대로 잡았다는 거야. 이 구조는 이후에 **주사위를 굴려 여러 칸 이동**, **턴 종료**, **이벤트 타일 처리** 같은 기능을 붙이기에도 확장하기 쉽다.

다만 보드게임을 만들 계획이라면 다음 단계에서는 **한 칸씩 순서대로 이동하는 시스템**(예: `A → B → C → D`)을 만드는 것을 추천해. 실제 보드게임은 한 번에 최종 목적지로 이동하기보다는 타일을 하나씩 밟으며 이동하는 경우가 대부분이라, 그 구조를 익히면 프로젝트를 훨씬 수월하게 확장할 수 있을 거야.

## Q.
우선 네가 알려준 2번, 3번 사항을 받아서 수정해봤어.
```
using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 destination;

    [SerializeField] private float moveSpeed = 5f;

    void Start()
    {
        // 목적지 = 현재 위치
        destination = transform.position;
    }
    void Update()
    {
        // 트렌스폼의 위치를 목적지로 이동 (속도: 5f)
        transform.position = Vector2.MoveTowards(transform.position, destination, Time.deltaTime * moveSpeed);
    }
        // 목적지 설정
    public void MoveTo(Transform targetTransform)
    {
        destination = targetTransform.position;
        Debug.Log("이동성공!");
    }
}
```
디버그 로그도 추가해봤어. 잘 되는지 확인해봐야할테니까. 그래서 좀 나아진건가? 

## A.
오, **확실히 나아졌어.** 👍
이제는 "튜토리얼에서 막 따라 친 코드" 느낌보다 **조금씩 자기 코드가 되어가는 느낌**이 난다.

하나씩 볼게.

---

## 👍 좋아진 점 1. moveSpeed를 SerializeField로 뺀 것

```csharp
[SerializeField] private float moveSpeed = 5f;
```

이거 아주 좋다.

이제 Inspector에서

```
5
7
10
2.5
```

이런 식으로 테스트할 수 있다.

게임 만들면서 이런 변수는 거의 다 SerializeField로 빼는 습관을 들이면 정말 편하다.

---

## 👍 좋아진 점 2. 변수명이 명확해짐

```csharp
public void MoveTo(Transform targetTransform)
```

아까보다 훨씬 읽기 쉽다.

이제

```csharp
destination = targetTransform.position;
```

보면

"아 목적지 Transform이구나."

바로 이해된다.

---

## 👍 좋아진 점 3. Debug.Log 추가

```csharp
Debug.Log("이동성공!");
```

이거 초보들이 많이 안 하는데 의외로 좋은 습관이다.

왜냐면

```
MoveTo가 호출이 안 됐는지

↓

호출은 됐는데 destination이 이상한지

↓

MoveTowards가 문제인지
```

이걸 하나씩 추적할 수 있기 때문.

---

## 그런데 하나 살짝 아쉬운 점

지금은

```csharp
public void MoveTo(Transform targetTransform)
{
    destination = targetTransform.position;
    Debug.Log("이동성공!");
}
```

여기서

만약

```csharp
MoveTo(null);
```

이 호출되면

```
NullReferenceException
```

이 발생한다.

예전 코드에는

```csharp
if (transform != null)
```

검사가 있었잖아.

지금은 없어졌어.

그래서 이렇게 하는 게 안전하다.

```csharp
public void MoveTo(Transform targetTransform)
{
    if (targetTransform == null)
    {
        Debug.LogWarning("목적지가 없습니다.");
        return;
    }

    destination = targetTransform.position;
    Debug.Log("이동 시작!");
}
```

이러면 실수해도 안 죽는다.

---

## 하나 더 추천

로그를 조금 더 자세히 찍는 습관.

예를 들어

```csharp
Debug.Log("이동 시작");
```

보다

```csharp
Debug.Log($"이동 시작 : {targetTransform.name}");
```

또는

```csharp
Debug.Log($"목적지 : {destination}");
```

이게 훨씬 좋다.

예를 들어

```
목적지 : (3.0, 4.0)
```

이렇게 나오면

좌표가 제대로 들어왔는지도 확인 가능하다.

---

## 그리고 코드 하나가 눈에 띄는데

맨 위에

```csharp
using System;
```

이거.

지금 코드에서는 **안 쓰고 있어.**

삭제해도 된다.

Visual Studio에서도 회색으로 표시될 거야.

최종적으로는

```csharp
using UnityEngine;
```

하나만 있으면 된다.

---

## 다음에 배우면 좋은 것

지금 코드에서 내가 다음 목표로 추천하는 건 **"도착했는지 확인하는 기능"**이야.

예를 들면,

```csharp
if (Vector2.Distance(transform.position, destination) < 0.01f)
{
    Debug.Log("도착!");
}
```

이걸 이용하면 나중에

* 주사위 1칸 이동
* 다음 칸으로 이동
* 이벤트 타일 실행
* 턴 종료

같은 기능을 아주 자연스럽게 연결할 수 있어.

---

### 총평

지금 코드를 보면 단순히 복붙만 한 느낌이 아니라, **"왜 이렇게 바꾸는지"를 이해하면서 수정한 흔적**이 보여.

특히 `moveSpeed`를 인스펙터로 뺀 것과 `targetTransform`으로 이름을 바꾼 건 앞으로도 계속 도움이 되는 습관이야.

한 가지 팁을 더 주자면, `Debug.Log("이동성공!")`이라는 메시지는 실제로는 **이동이 성공한 시점이 아니라 이동을 시작한 시점**에 출력되고 있어. 그래서 `"이동 시작!"` 또는 `"목적지 설정 완료!"`처럼 바꾸면 나중에 디버깅할 때도 더 헷갈리지 않을 거야.

## Q.
그럼 이게 도착했을때 멈추는 코드는 없는건가? 아니면 지정된 목적지로 이동하는거라 딱히 멈추는 코드가 없어도 되는건가?
## A.
좋은 질문이다. 이걸 이해하면 `MoveTowards`를 거의 다 이해한 거라고 봐도 돼.

결론부터 말하면,

> **멈추는 코드를 따로 작성하지 않아도 된다.**

왜냐하면 **`Vector2.MoveTowards()`가 알아서 멈춰주기 때문**이야.

---

## 예를 들어 보자.

현재 위치가

```text
(0,0)
```

목적지가

```text
(5,0)
```

라고 해보자.

매 프레임마다

```csharp
transform.position = Vector2.MoveTowards(
    transform.position,
    destination,
    moveSpeed * Time.deltaTime);
```

를 실행하면

대충 이런 식으로 움직여.

```text
0
↓

0.2
↓

0.4
↓

0.6
↓

...

↓

4.8
↓

5.0
```

그리고 다음 프레임에도 또 실행되는데

현재 위치가

```text
(5,0)
```

목적지도

```text
(5,0)
```

이므로

`MoveTowards()`는

```text
(5,0)
```

을 그대로 반환한다.

즉,

```text
5.0
↓

5.0
↓

5.0
↓

5.0
```

계속 같은 좌표만 돌려주기 때문에 더 이상 움직이지 않는 거야.

---

## 그럼 Update는 계속 실행되잖아?

맞아.

```csharp
void Update()
{
    transform.position =
        Vector2.MoveTowards(...);
}
```

이 코드는 1초에 60번 정도 계속 실행된다.

하지만 `MoveTowards()`는 이미 목적지에 도착했으면 **같은 좌표를 반환**하므로 실제로는 이동이 일어나지 않는다.

그래서 멈춘 것처럼 보이는 거야.

---

## 그럼 왜 내가 `if`를 넣는 걸 추천했냐?

성능 때문이라기보다는 **의도를 명확하게 표현하기 위해서**야.

예를 들어

```csharp
if ((Vector2)transform.position != destination)
{
    transform.position = Vector2.MoveTowards(...);
}
```

이렇게 하면

> "움직일 때만 이동 코드를 실행한다."

라는 의도가 코드에 드러난다.

그리고 나중에

```csharp
if ((Vector2)transform.position == destination)
{
    // 도착!
}
```

같은 로직을 추가하기도 쉬워진다.

---

## 보드게임에서는 "도착"이 정말 중요하다

예를 들어 주사위가 3이 나왔다고 해보자.

```text
A → B → C → D
```

플레이어는

```text
A

↓

B

↓

C

↓

D
```

이렇게 한 칸씩 이동해야 하지.

그런데 **D에 도착했을 때만**

* 이벤트 발생
* 턴 종료
* 다음 플레이어 차례

를 실행해야 한다.

그래서 보드게임에서는 보통 이런 식으로 체크한다.

```csharp
if (Vector2.Distance(transform.position, destination) < 0.01f)
{
    Debug.Log("도착!");
}
```

---

## 하나 팁!

`==`로 위치를 비교하는 것보다

```csharp
Vector2.Distance(transform.position, destination) < 0.01f
```

처럼 **거리로 비교하는 습관**을 들이는 걸 추천해.

왜냐하면 게임에서는 부동소수점 오차 때문에 좌표가 아주 미세하게 달라지는 경우가 있어서, 거리 기준이 더 안전한 경우가 많거든.

---

그래서 지금 네 코드도 **정상적으로 목적지에서 멈춘다.** `MoveTowards()`가 그 역할을 이미 해주고 있어. 다만 **"도착했다"는 사실을 감지해서 다른 행동을 하고 싶을 때**는 `Distance` 같은 체크를 추가하는 거야. 이 차이를 이해하면 앞으로 이동 시스템을 훨씬 수월하게 만들 수 있을 거야.
