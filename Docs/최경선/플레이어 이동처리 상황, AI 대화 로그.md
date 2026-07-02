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

## Q.
그 뭐냐... 내가 Start 쪽에 작성한 destination = transform.position; 라고 쓴게 플레이어 캐릭터를 다른 곳으로 이동시켰을때ㅔ 원래 있던자리로 돌아오게 만드는 원인일까? 캐릭터가 필드에서 다른필드로 넘어가길 바랬는데 캐릭터를 이동시키니까 그냥 냅다 원래있던자리로 그냥 돌아오네... Update쪽에 적힌게 transform.position = Vector2.MoveTowards(transform.position, destination, Time.deltaTime * moveSpeed); 잖아. 트렌스폼의 위치를 목적지로 이동인데 위에 적은게 목적지 = 현재위치 라고 되어있으니까 되게 헷갈려.

## A.
오, 이건 **정확히 디버깅해야 할 포인트**다. 그리고 네 설명을 보니까 **원인이 두 가지 중 하나**일 가능성이 높아.

먼저 `destination = transform.position;`부터 헷갈리는 부분을 정리해보자.

---

## `destination = transform.position;`의 의미

Start에서 이 코드가 실행되는 순간은 **게임이 시작되는 딱 한 번**이야.

예를 들어 플레이어가 처음에 (0, 0)에 있었다면

```csharp
Start()
{
    destination = transform.position;
}
```

이 실행되고 나면

```text
transform.position = (0,0)
destination        = (0,0)
```

이 된다.

그 이후에는 `destination`은 **자동으로 바뀌지 않아.**

---

## Update는 무슨 일을 하냐?

```csharp
transform.position = Vector2.MoveTowards(
    transform.position,
    destination,
    Time.deltaTime * moveSpeed);
```

이건 말 그대로

> **현재 위치를 destination 쪽으로 조금씩 이동시켜라.**

라는 뜻이야.

예를 들어

```text
현재 위치 : (5,3)
목적지    : (0,0)
```

라면

매 프레임마다

```text
(5,3)

↓

(4.9,2.9)

↓

(4.8,2.8)

↓

...
```

이렇게 원래 저장된 목적지로 계속 돌아간다.

---

# 그런데 네 현상을 보면...

> **캐릭터를 다른 필드로 이동시키니까 원래 자리로 돌아온다.**

이건 거의 확실하게

**다른 코드에서 `transform.position`만 바꾸고 `destination`은 안 바꿨기 때문**일 가능성이 높아.

예를 들어 이런 코드가 있다고 해보자.

```csharp
player.transform.position = new Vector2(20, 5);
```

그러면 순간적으로는

```text
transform.position = (20,5)
destination        = (0,0)
```

이 상태가 된다.

그런데 다음 프레임 Update가 실행되면서

```text
(20,5)

↓

(19.9,4.9)

↓

(19.8,4.8)

↓

...
```

다시 (0,0)으로 걸어가기 시작하는 거야.

---

# 해결 방법

### 방법 1 (가장 추천)

캐릭터를 순간이동시킬 때는

```csharp
transform.position = newPosition;
destination = newPosition;
```

둘 다 바꿔줘야 해.

예를 들어

```csharp
public void Teleport(Vector2 newPosition)
{
    transform.position = newPosition;
    destination = newPosition;
}
```

그러면

```text
현재 위치 = (20,5)
목적지    = (20,5)
```

라서 더 이상 돌아가지 않아.

---

### 방법 2

이동은 무조건

```csharp
MoveTo(...)
```

만 사용한다.

즉,

```csharp
transform.position = ...
```

를 다른 곳에서 직접 안 건드리는 거지.

---

# 내가 궁금한 게 하나 있어.

카펠라가 말한

> **"필드에서 다른 필드로 넘어갔다."**

이게

### ① 같은 씬 안에서 위치만 바뀌는 거야?

예를 들어

```text
마을
↓

던전 입구
```

처럼 좌표만 이동?

아니면

### ② 아예 새로운 씬을 불러오는 거야?

```text
Town 씬

↓

Dungeon 씬
```

처럼?

---

이 둘은 원인과 해결 방법이 완전히 달라.

**혹시 캐릭터를 다른 필드로 이동시키는 코드(또는 씬 전환 코드)를 보여줄 수 있을까?** 그걸 보면 왜 원래 자리로 되돌아가는지 거의 바로 원인을 찾아낼 수 있을 것 같아.

## Q.
1번이야.

## A.
그렇다면 원인이 거의 확실해졌어. 👍

같은 씬 안에서 **좌표만 바꾸는 방식**이라면, 지금 상황은 아마 이런 순서로 일어나고 있을 거야.

### 1. 게임 시작

```text
transform.position = (0, 0)
destination        = (0, 0)
```

Start에서

```csharp
destination = transform.position;
```

이 실행됨.

---

### 2. 다른 코드에서 플레이어를 이동

예를 들어

```csharp
player.transform.position = new Vector2(20, 10);
```

을 실행.

그러면 순간적으로

```text
transform.position = (20, 10)
destination        = (0, 0)
```

이 상태가 돼.

---

### 3. 다음 프레임 Update 실행

```csharp
transform.position = Vector2.MoveTowards(
    transform.position,
    destination,
    Time.deltaTime * moveSpeed);
```

그러면

```text
(20,10)
 ↓
(19.9,9.9)
 ↓
(19.8,9.8)
```

이렇게 원래 위치로 걸어가기 시작하는 거지.

---

# 그럼 어떻게 해야 하냐?

## 방법 1 (추천 ⭐⭐⭐⭐⭐)

순간이동을 할 때는 **목적지도 같이 바꾼다.**

예를 들어

```csharp
public void Teleport(Vector2 newPosition)
{
    transform.position = newPosition;
    destination = newPosition;
}
```

이렇게 만들어 두고,

```csharp
player.Teleport(new Vector2(20, 10));
```

처럼 사용하는 거야.

그러면

```text
현재 위치 = (20,10)
목적지    = (20,10)
```

이라서 다시 돌아가지 않아.

---

## 방법 2

만약 이동도 `MoveTo()`만 쓸 거라면

```csharp
player.MoveTo(targetTransform);
```

만 호출하고,

```csharp
player.transform.position = ...
```

처럼 직접 위치를 바꾸는 코드는 쓰지 않는 거야.

---

# 내가 하나 궁금한 게 있어.

카펠라는 **플레이어를 어떻게 다른 위치로 보내고 있어?**

예를 들면 이런 식이야?

```csharp
player.transform.position = spawnPoint.position;
```

아니면

```csharp
transform.position = new Vector2(...);
```

아니면 다른 코드?

**그 부분 코드만 보여주면** "여기서 destination도 같이 바꿔주면 된다." 하고 정확한 수정 위치까지 알려줄 수 있어.

지금까지 설명한 걸 보면 `PlayerMovement` 자체는 큰 문제는 없어 보여. **문제는 플레이어를 이동시키는 다른 코드와 `destination`의 값이 서로 달라져 버리는 것**일 가능성이 가장 높아.

## Q.
순간이동이 아니고, 걸어가. 필드가 작거든, 특정 필드에서 걸어서 다음 필드로 도착하는거야. 그럼 일단 그 필드에도 코드가 있어야하는거겠지?

## A.
아, 그럼 상황이 완전히 이해됐다. 😄

네가 말하는 "필드"가 RPG처럼

```text
┌──────────────┐
│   필드 A     │
│              │
│          ▶ 출구
└──────────────┘

        ↓ 걸어서 이동

┌──────────────┐
│ ◀ 입구       │
│   필드 B     │
│              │
└──────────────┘
```

이런 구조인 거지?

그렇다면 **맞아. 필드(출구)에도 코드가 있어야 해.**

---

## 보통은 이렇게 만든다.

### 필드 A의 출구

출구에 `BoxCollider2D`를 달아놓고

```csharp
OnTriggerEnter2D()
```

를 사용해서 플레이어가 들어오면

```csharp
player.transform.position = 필드B의 입구;
```

로 이동시킨다.

---

### 그런데 지금 문제가 생기는 이유

플레이어는 이동됐는데

```text
transform.position = 필드 B 입구
destination        = 필드 A 출구
```

이 상태인 거야.

그래서 PlayerMovement가

> "아직 목적지에 안 갔네?"

라고 생각해서 다시 필드 A 출구 쪽으로 걸어가 버리는 거지.

즉, **PlayerMovement는 아무 잘못도 없어.**

---

# 해결 방법은 두 가지가 있어.

### 방법 1 (내가 추천하는 방법)

PlayerMovement에 이런 함수를 하나 만든다.

```csharp
public void SetPosition(Vector2 position)
{
    transform.position = position;
    destination = position;
}
```

그리고 필드를 이동할 때는

```csharp
player.SetPosition(spawnPoint.position);
```

만 호출한다.

이게 가장 깔끔하다.

---

### 방법 2

필드 이동 스크립트에서

```csharp
player.transform.position = spawn.position;
```

을 한 뒤

```csharp
player.MoveTo(spawn);
```

를 바로 호출해도 된다.

그러면 destination도 spawn 위치가 된다.

---

## 그리고 하나 확인하고 싶은 게 있어.

네 필드 이동은

### ① Collider에 닿으면 이동하는 방식이야?

```text
□□□□□□
□     □
□     ▶
□□□□□□
```

---

아니면

### ② 문 앞에서 E키를 누르면 이동하는 방식이야?

둘 다 가능하지만 코드가 조금 달라져.

---

### 마지막으로 하나 칭찬할 점.

지금 카펠라가 질문한

> "그럼 그 필드에도 코드가 있어야 하는 거겠지?"

이 질문은 굉장히 좋은 방향이야.

초보 때는 보통 **"PlayerMovement가 이상한가?"**만 의심하는데, 실제 게임에서는 **플레이어 스크립트와 필드(출구) 스크립트가 서로 역할을 나눠서 동작**하는 경우가 대부분이야.

* **PlayerMovement** → "어떻게 이동할 것인가?"
* **필드(출구)** → "언제, 어디로 이동시킬 것인가?"

이렇게 역할을 분리하는 게 유지보수도 쉽고 확장하기도 편해져.

## Q.
말이 필드지, 그냥 오브젝트 위에 서있다가, 옆의 오브젝트로 걸어가는 거야.

## A.
아ㅋㅋ 이제 무슨 말인지 알겠다.

그럼 **맵이 여러 개 있는 게 아니라**, 그냥 하나의 작은 맵 안에서

```text
[오브젝트 A] ---- [오브젝트 B]
      ↑               ↑
   플레이어가      플레이어가
     여기 서있음    걸어서 이동
```

이런 거구나.

그렇다면 **필드 이동이나 순간이동 문제는 아닐 수도 있어.**

---

그런데 내가 궁금한 게 하나 생겼어.

카펠라가 말한

> "원래 있던 자리로 돌아온다."

이게 **언제** 일어나?

### 경우 1

```text
A에서 출발

↓

B까지 거의 다 감

↓

갑자기 A로 돌아감
```

이거야?

---

### 경우 2

```text
MoveTo() 호출

↓

한 발짝도 안 가고

↓

바로 원래 자리로 감
```

이거야?

---

### 경우 3

```text
B에 도착

↓

몇 초 후

↓

A로 걸어감
```

이거야?

---

## 사실 지금 PlayerMovement 코드만 보면...

```csharp
public void MoveTo(Transform targetTransform)
{
    destination = targetTransform.position;
}
```

이 함수가 호출되면 **destination은 무조건 새 위치로 바뀐다.**

그래서 **이 코드만으로는 원래 자리로 돌아갈 이유가 없어.**

즉,

**다른 스크립트에서 `MoveTo()`를 다시 호출하거나, `destination`이 다시 예전 위치로 바뀌고 있을 가능성**이 굉장히 높아.

---

## 내가 가장 보고 싶은 코드

아마 범인은 `PlayerMovement`가 아니라 이 코드야.

예를 들면

```csharp
player.MoveTo(????);
```

를 호출하는 스크립트.

또는

```csharp
OnMouseDown()

OnTriggerEnter2D()

Update()

Button 클릭
```

이런 데서 `MoveTo()`를 호출하고 있을 거야.

**그 코드를 보여주면 거의 100% 원인을 찾을 수 있어.**

---

### 하나 테스트도 해보자.

`Update()`에 이 로그를 잠깐 넣어봐.

```csharp
Debug.Log(destination);
```

그리고 이동해 봐.

만약 로그가

```text
(0,0)
(0,0)
(0,0)
```

만 계속 찍힌다면 `MoveTo()`가 아예 호출되지 않는 거고,

```text
(5,0)
(5,0)
(0,0)
```

처럼 중간에 다시 예전 좌표로 바뀐다면 **다른 코드가 destination을 덮어쓰고 있는 것**이 확실해져.

지금 단계에서는 **PlayerMovement보다 `MoveTo()`를 호출하는 코드**를 보는 게 훨씬 중요해 보여.

## Q.
내가 캐릭터를 직접 드래그해서 이동시켰을때 스르륵 돌아오던데...

## A.
ㅋㅋㅋㅋㅋ 그거였냐. 그럼 거의 **정상 동작**이야. 😂

네가 말한 상황은 아마 이거지?

1. 게임 실행.
2. Scene 뷰에서 플레이어를 마우스로 드래그해서 다른 위치로 옮김.
3. 놓는 순간 플레이어가 스르륵 원래 자리로 돌아감.

이게 맞다면 이유는 아주 간단해.

---

## 왜 돌아오냐?

게임 시작할 때

```csharp
void Start()
{
    destination = transform.position;
}
```

여기서 `destination`이 저장됐어.

예를 들어 시작 위치가

```text
destination = (2, 3)
```

이라고 하자.

그런데 실행 중에 네가 Scene 뷰에서 플레이어를

```text
(10, 8)
```

로 드래그했어.

그러면

```text
transform.position = (10,8)
destination        = (2,3)
```

이 상태가 된다.

다음 프레임에

```csharp
transform.position = Vector2.MoveTowards(
    transform.position,
    destination,
    ...
);
```

가 실행되니까

> "목적지가 (2,3)이네? 그쪽으로 걸어가자."

하고 원래 자리로 돌아가는 거야.

---

## 즉, 이건 버그가 아니다.

`MoveTowards`가 **너무 성실하게 자기 일을 하는 것**이야. 😄

---

## 실제 게임에서는?

실제 게임에서는 **플레이어를 마우스로 드래그해서 움직이지 않잖아.**

대신

```csharp
player.MoveTo(tile.transform);
```

처럼 `MoveTo()`를 호출해서 목적지를 바꿔준다.

그러면

```text
destination = 타일 위치
```

가 되니까 그쪽으로 걸어가는 거고.

---

## 테스트할 때는

드래그로 위치를 바꾸는 대신

```csharp
player.MoveTo(목표 타일);
```

이 호출되는지 확인하는 게 맞아.

---

### 하나 팁!

만약 디버깅 때문에 실행 중에 위치를 옮기고 싶다면, 이런 메서드를 하나 만들어 두는 것도 좋아.

```csharp
public void SetPosition(Vector2 position)
{
    transform.position = position;
    destination = position;
}
```

그러면 위치와 목적지가 항상 같아져서 다시 되돌아가지 않아.

---

그래서 이번 현상은 오히려 **코드가 의도대로 작동하고 있다는 증거**야. 드래그로 `transform.position`만 바뀌었고 `destination`은 그대로였기 때문에, `MoveTowards`가 원래 저장된 목적지로 다시 이동시킨 거지.

## Q.
플레이어 캐릭터가, 아까 내가 말한 설명대로 움직여야하는데, 내가 지금 자료조사를 못하는건가 싶을정도로 이걸어떻게 해야할지 감을 못잡겠어. 테스트를 해보고싶은데... 아까 말했던대로 오브젝트에도 코드 넣어야하나? 아니면 다른방법이 있나?

## A.
카펠라, 지금 막힌 이유는 **이동 코드**가 아니라 **누가 `MoveTo()`를 호출하느냐**를 아직 정하지 않았기 때문이야.

지금 네 `PlayerMovement`는 이미 역할이 있어.

> "목적지를 받으면 그곳까지 걸어간다."

그런데 아직 아무도

> "여기로 가."

라고 말해주는 사람이 없는 거지.

---

## 내가 추천하는 구조

예를 들어 맵이 이렇게 있다고 하자.

```text
[타일1] ---- [타일2] ---- [타일3]
```

플레이어는 처음에 타일1 위에 있다.

플레이어는

> 난 그냥 목적지만 주면 걸어간다.

만 담당한다.

---

## 그러면 타일은?

각 타일은

> 옆 타일이 누구인지 안다.

예를 들어

```
타일1
 └── nextTile → 타일2

타일2
 └── nextTile → 타일3
```

이런 식.

그러면 타일에 이런 스크립트를 붙일 수 있다.

```csharp
using UnityEngine;

public class BoardTile : MonoBehaviour
{
    public Transform nextTile;
}
```

이것만 있어도 된다.

---

## 그럼 테스트는 어떻게?

예를 들어 스페이스바를 누르면

```csharp
player.MoveTo(currentTile.nextTile);
```

를 호출한다.

그러면

```
타일1

↓

타일2
```

로 걸어간다.

도착하면

```
currentTile = currentTile.nextTile;
```

로 현재 타일도 갱신한다.

---

## 오브젝트에 코드가 꼭 있어야 하나?

**반드시 그런 건 아니야.**

두 가지 방법이 있어.

### 방법 1 (내 추천)

타일마다 스크립트가 있다.

장점

* 길을 연결하기 쉽다.
* 분기길도 만들 수 있다.
* 나중에 이벤트도 넣기 쉽다.

---

### 방법 2

게임매니저가

```text
0번
↓

1번
↓

2번
↓

3번
```

이런 리스트를 들고 있는다.

그러면

```csharp
player.MoveTo(tileList[index]);
```

이렇게 이동한다.

이 방법도 많이 쓴다.

---

# 카펠라가 만들려는 보드게임이라면

나는 **방법 1**을 추천해.

왜냐하면 나중에

* 갈림길
* 함정 칸
* 상점
* 워프

같은 걸 넣기 편하다.

---

# 그런데 더 쉬운 테스트 방법이 있다.

지금은 오브젝트에 코드도 필요 없다.

빈 오브젝트 하나를 만들고

예를 들어

```
GameObject
└── TestMove.cs
```

를 붙인다.

그리고

```csharp
using UnityEngine;

public class TestMove : MonoBehaviour
{
    public PlayerMovement player;
    public Transform target;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.MoveTo(target);
        }
    }
}
```

이렇게 해.

그다음

* Player 칸에는 플레이어를 드래그.
* Target 칸에는 옆 타일 오브젝트를 드래그.

그리고 게임을 실행해서 **스페이스바를 누르면 플레이어가 그 타일까지 걸어가는지** 확인하는 거야.

---

## 내가 추천하는 순서

지금은 절대 한 번에 다 만들려고 하지 마.

1. ✅ 플레이어가 지정한 오브젝트까지 걸어간다.
2. ✅ 도착을 감지한다.
3. ✅ 한 칸 이동이 성공한다.
4. ✅ 여러 칸 연속 이동한다.
5. ✅ 주사위 숫자만큼 반복 이동한다.

이 순서대로 가면 훨씬 덜 막힌다.

그리고 지금까지 대화를 보니까, **`PlayerMovement`는 거의 완성 단계**야. 이제 필요한 건 **이동을 지시하는 시스템**이지, 이동 자체를 다시 만드는 게 아니야. 이 부분만 넘으면 보드게임의 핵심 구조가 잡히기 시작할 거야.

## Q.
클릭하니까 이동하긴한다. 근데 움직이는 애니메이션을 적용해주려면 어떻게해야하지... 애니메이터에는 일단 움직이는 모션, 서있는 모션 다 넣어놨거든.

## A.
좋다!! 🎉
**클릭해서 이동이 된다면 가장 어려운 부분은 이미 넘은 거야.**

이제 남은 건 **"움직이는 동안만 걷기 애니메이션을 재생하고, 도착하면 다시 Idle로 돌아가기"**야.

이럴 때는 Animator에 **bool 변수 하나**를 만드는 게 가장 흔한 방법이야.

---

# 1. Animator에 bool 추가

Animator 창에서 Parameters를 열고

```
isMoving
```

이라는 Bool을 하나 만들어.

---

# 2. Transition 설정

```
Idle  ----(isMoving = true)---> Walk

Walk  ----(isMoving = false)---> Idle
```

그리고 두 Transition 모두

```
Has Exit Time
```

은 꺼주는 걸 추천해.

안 그러면 걷는 모션이 끝날 때까지 기다렸다가 바뀌는 경우가 생길 수 있어.

---

# 3. PlayerMovement에서 Animator 가져오기

```csharp
private Animator animator;
```

그리고 Start에서

```csharp
animator = GetComponent<Animator>();
```

---

# 4. Update에서 움직이는지 검사

지금 코드가 아마 이런 느낌이지?

```csharp
transform.position = Vector2.MoveTowards(...);
```

그럼 이동하기 전에 거리만 확인하면 된다.

예를 들면

```csharp
bool moving = Vector2.Distance(transform.position, destination) > 0.01f;

animator.SetBool("isMoving", moving);

transform.position = Vector2.MoveTowards(
    transform.position,
    destination,
    moveSpeed * Time.deltaTime);
```

이렇게.

---

## 그러면 동작이

```
목적지 설정

↓

Distance > 0.01

↓

isMoving = true

↓

Walk 재생

↓

도착

↓

Distance < 0.01

↓

isMoving = false

↓

Idle 재생
```

이렇게 된다.

---

# 보드게임에서는 이 방식이 정말 많이 쓰인다.

이동 중

```
Walk
```

도착

```
Idle
```

이것만 해도 충분한 경우가 많다.

---

# 나중에 추가하면 좋은 것

방향도 바꾸고 싶다면

```
Idle
Walk
```

만 있는 게 아니라

```
Idle Down
Idle Up
Idle Left
Idle Right

Walk Down
Walk Up
Walk Left
Walk Right
```

처럼 만들고,

```csharp
animator.SetFloat("MoveX", ...);
animator.SetFloat("MoveY", ...);
```

로 방향까지 넘겨줄 수 있어.

하지만 **지금은 그 단계까지 갈 필요는 없어.**

---

## 내 추천

지금은 **`isMoving` 하나만 구현하는 것**을 목표로 해.

* ✅ 클릭하면 이동
* ✅ 이동하는 동안 Walk
* ✅ 도착하면 Idle

이 세 가지만 완성해도 이동 시스템이 훨씬 "게임 같다"는 느낌이 들 거야.

그리고 그 다음에는 **플레이어가 이동하는 방향에 따라 좌우 반전(SpriteRenderer.flipX)이나 방향별 애니메이션**을 붙이면 자연스럽게 다음 단계로 이어질 수 있어.
