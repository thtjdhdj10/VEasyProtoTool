# VEasyProtoTool
This is very easy game prototyping tool.

## 개요
Unity 는 Unreal과 달리 퍼포먼스보단 생산성에 맞춰져 있기에, 게임의 프로토타입을 만들기에 적합한 편이다.  
  
  그럼에도, 게임의 재미요소를 구현하기 위한 기골들을 만드는 작업은 많은 시간을 필요로 한다.
  
  이는 반대로 말하면, 게임을 이루는 기본적인 구성 요소들이 제공된다면, 마치 레고 블럭을 맞추듯 게임을 만들 수 있다는 것이다.
  
  스타크래프트 에디터가 유닛의 이동과 공격을 제공하거나, RPG 쯔꾸르가 턴제 전투 방식을 제공하는 것처럼 말이다.
  
  하지만 이들은 화면을 구성하는 기본적인 요소 등에 의해, 명확한 한계점이 존재한다.
  
  이 프로젝트는 이에 대한 한가지 해결책으로써, 그런 기골들을 Unity 라는 상용 게임 엔진에 기반하여 다양하게 제공할 예정이다.
  
  또한 Component, Inspector 같은 빠르게 접근할 수 있는 장치를 적극적으로 활용하여, 보다 쉽게 프로토타이핑할 수 있는 환경을 구성할 것이다.


## 구성 요소
### Trigger
  복수의 Condition 과 Action 을 가지고 있는 조건부 실행 컴포넌트. 종류에 따라 n frame 마다, 혹은 후술할 Operable 을 통해 이벤트를 받으면 Condition 이 성립하는지 확인하고, 통과하면 Action 들을 수행한다.
  
  ex) [Slime 을 죽였을 때, 점수가 10 미만이면, 점수 +1] 여기서 Slime 을 죽이는 것이 Event, 점수가 10 미만인지 확인하는 것이 Condition, 점수 +1 이 Action 이다.
  
### Operable
  주로 Unit 에 붙이는 기능성 컴포넌트. Unit 의 몇가지 이동, 공격 방식을 제공하거나, Unit 간 충돌이나 키 입력으로 Trigger 를 작동시키는 역할.
  
  몇가지 이동 방식을 제공하거나 맵을 나가면 Destroy 하는 등 이동 관련 지원을 하는 Movable. 유닛간의 충돌을 검사하고 이벤트화 하는 Hittable. 키 입력과 유닛을 묶어 이벤트화 하는 Controlable 등이 있다.
  
###Map Editor
  
### VEasyPooler
  쉽게 사용할 수 있는 오브젝트 풀러이다.
  
### CustomRandGen
  각 랜덤을 구하는 주체에 대해 같은 값이 n 번 이상 연속으로 나오지 않게 하는 등, 인공적이지만 더 자연스러운 랜덤값을 제공.  
  
### CustomLog  
  주로 로그 출력 빈도를 줄여서, Update() 에서 호출해도 렉이 걸리지 않게 하는 용도로 사용.
