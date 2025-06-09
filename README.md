### 🗂️ 프로젝트 소개  
>어린 시절 즐겼던 쥬니어네이버의 ‘동물 농장’, ‘파니팡’에서 영감을 받아 간단한 미니게임을 통해 재화를 획득하고 이를 활용해 공간을 꾸밀 수 있는 캐주얼 게임 플랫폼을 개발하고 있습니다.

<br>

### 🛠️ 기술 스택
![Unity](https://img.shields.io/badge/unity-%23000000.svg?style=for-the-badge&logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)

<br>

### 🎮 [첫 번째 미니게임 '플래피 버드'](https://github.com/JoHyeonJi0408/MiniGameParty/tree/main/Assets/Games/FlappyBird/Scripts)  
장애물을 피하며 점수를 얻는 타이밍 중심의 캐주얼 게임  
![FlappyBird](https://github.com/user-attachments/assets/760ad799-f8e3-4aac-819d-d17eed024179)

#### 주요 알고리즘 & 디자인 패턴
- 오브젝트 풀링
    - `Pipe` 프리팹을 미리 생성해 비활성화된 오브젝트를 `Queue`(pool)에 저장해두고 필요할 때 꺼내서 재사용
    - 파이프가 카메라 밖으로 나가면 `OnBecameInvisible` 이벤트를 통해 자동으로 회수하여 비활성화 및 풀에 반환
- 옵저버 패턴
    - `OnPointScored`, `OnBecameInvisible`와 같은 UnityEvent를 통해 이벤트 리스너 방식으로 점수 갱신, 오브젝트 반환 등의 처리를 수행
 
<br>

### 🎮 [두 번째 미니게임 '블록 깨기'](https://github.com/JoHyeonJi0408/MiniGameParty/tree/main/Assets/Games/Breakout/Scripts)  
충돌 기반 브릭 브레이커 게임  
![Breakout_Thumbnail](https://github.com/user-attachments/assets/f8277c06-5e1f-4e2f-9842-8a8fce9b6f58)

#### 주요 알고리즘 & 디자인 패턴
- 공이 벽 및 블록 충돌 시 반사 방향 계산
    - `Vector2.Reflect` 함수를 활용해 공의 이전 속도와 충돌 `normal`을 기반으로 반사 방향 계산
    - 충돌 지점의 `normal` 벡터가 수직인지 수인지 구분하기 위해 `Vector2.Dot` 함수를 사용
- 패들 충돌시 반사 각도 조절
    - 공이 패들 중앙에 가까울수록 수직에 가까운 반사각을 갖고, 가장자리일수록 수평에 가까운 반사각을 갖도록 하여 플레이어가 방향을 조절할 수 있게 설계
    - 충돌 지점의 위치를 정규화하고, `Mathf.Lerp`로 반사각 조정값을 선형 보간하여 반영
- 벽돌
    - 상속과 템플릿 메서드 패턴을 활용한 구조로 설계
    - 기본 벽돌 클래스(`Brick`)을 기반으로 아이템 드롭 기능이 있는 `ItemBrick`과 2단계 파괴 로직을 가진 `SpecialBrick` 등 하위 클래스를 통해 각 벽돌의 개별 동작 정의
![Brick_Class](https://github.com/user-attachments/assets/84fde3b8-787b-4823-9039-cfe1e5c13680)
