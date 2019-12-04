using UnityEngine;
using System.Collections.Generic;

public class Selectable : Operable
{
    /*
        유닛의 transform 좌표를 screen 좌표로 변환하여 계산

        드래그. 유닛의 중앙 점을 포함하는지 판정

        클릭. 유닛의 visualSize 내에 있는지 판정
        
        시프트 + 클릭. 여러 유닛을 선택
     * 
     * 알트 + 클릭. 같은 종류의 유닛을 모두 선택
    */

    /*
        이 컴포넌트는 주로, 스타크래프트와 같은 여러 유닛을 컨트롤하는 게임에서 사용할 수 있다.

    */

    /*
        유닛을 선택하면 유닛에게 Controlable 컴포넌트가 붙도록.
        
        그 컴포넌트가 붙은 상태에서의 동작은 유닛에게 명령된다.

        이를 활용하여, 하나의 유닛을 조종하는 RPG 게임이라면,

        처음에 주어지는 플레이어 유닛에게 Controlable 컴포넌트를 붙여놓고,

        그 스크립트에 있는 continual flag 를 true 로 하여 계속 명령할 수 있게 하면 된다.

    */


}
