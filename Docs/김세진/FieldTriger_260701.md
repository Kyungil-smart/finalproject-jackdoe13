c:\Users\SEJIN\Downloads\Field.jpg
Field : 육각형 벌집 형태로 필드를 구성, 각 필드는 Circle Colider 2D로 충돌을 구분하였으며 사각형이나 삼각형일 시 중복되는 구간이 있어 원을 선택하고 크기를 중심에 맞춰 줄임
OntrigerEnter2D 메서드와 받는 인자값 Colider2D로 정의하였으며 Enter를 선택한 이유는 이동 후 1번 발동하면 되기 때문에 Enter를 사용하였음
```csharp
using System;
using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
    }
}
```
[결과]
각 필드에 도달 시 캐릭터가 필드 정보를 읽는 것을 확인할 수 있음
![alt text](<2026-07-01 16 48 55.jpg>)