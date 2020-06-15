# VEasyProtoTool
Very easy game prototyping tool

## 개요
스타크래프트 유즈맵 제작 툴처럼 기본적인 형태의 유닛을 제공해서 빠른 프로토타이핑을 돕는 도구

기본적인 형태의 슈팅게임, 플랫포머 게임 등을 만들어보며 다양한 게임을 빠르게 만들 수 있도록 살을 더해갈 예정


## 구성
### Operable 컴포넌트
GameObject 에 적용시킬 수 있는 다양한 행동 방식을 구현한 컴포넌트

이동 방식을 정의하고 물체를 이동시키는 Movable
키 입력 이벤트를 핸들하는 Controllable
충돌과 관련된 속성을 설정하고 충돌 이벤트를 핸들하는 Collidable
등이 있으며, 일부 Operable 컴포넌트는 moveDir, targetDir을 정의한 Actor 컴포넌트를 필요로 한다.


### Event 처리 시스템
이벤트 처리 시스템은 Trigger - Condition - Action 3단계로 구성되어 있다.

Trigger는 복수의 Condition과 Action을 멤버로 소유하며 트리거가 호출되면 Condition을 체크하고 통과되면 Action을 동작시킨다.

TrgKeyInputs, TrgCollision 등 사전에 정의된 몇개의 트리거는 Operable과 상호작용한다.

  
### ObjectPooler
오브젝트를 생성(Instantiate)/파괴(Destroy) 하는 대신 활성화/비활성화 하여 객체 생성 비용을 줄일 수 있는 기능


### ResourceManager
Resources 하위 경로의 모든 폴더를 검사해서 사전 정의된 형식의 파일이 있으면 (*.prefab, *.png 등) 해당 파일의 이름을 key 값으로 하는 Dictionary에 리소스를 적재하고 관리한다.

  
### KeyManager
키 입력에 대한 동작을 정의하고 입력된 키를 Controllable에 전달한다.


### Calculator
거리계산, 각도계산, 충돌여부 등 수학적인 기능 지원
