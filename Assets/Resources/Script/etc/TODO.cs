﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TODO : MonoBehaviour
{
    //    movable 에디터

    //보스 패턴 구현
    // 랜드슬레이어: 돌격모드, 난사모드 전환
    // 난사모드: player 방향 +-45도로 총알난사. 난사된 총알은 일정 체공 후 0도 각도로 각도변환
    // 난사모드: 45도 간격으로 전방향 총알난사. 난사된 총알은 일정 체공 후 45도 각도 틂
    // 난사모드: 양팔에서 60도로 길막. 가운데에서 레이저

    // 돌격모드: OXXO 순서로 자신처럼 이동하는 잔상을 남기고 멈추거나, 멈춘 잔상을 남김. 잔상은 일정 시간 후 폭발
    // 돌격모드: 플레이어의 주변을 크게 회전하며 총알남김. 남긴총알은 일정 시간 후 플레이어방향으로 이동
    // 돌격모드: 플레이어 주변을 크고 빠르게 회전하며 플레이어 방향으로 7개의 총알 방사

    // 난사모드: 양팔에서 45도 간격으로 전방향 총알난사, 플레이어 방향으로 폭탄 3개씩 발사. 폭탄에 휘말린 총알은 방향 변경


    //매트릭스 배경 구현


    // 폭탄 구현
    // 폭발 쉐이더

    //유도미사일


    // 레이저 구현


    // 판넬 구현

    //실드구현

    // 적1: 직선이동 벽팅기면 플레이어 쪽으로 방향
    // 적2: 제자리서 빙글빙글. 일정시간마다 총발사
    // 적3: 미사일처럼 플레이어쪽으로 이동. 벽닿으면 총알사방으로(일정거리 까지만감), 체력감소
    // 적4: wing 총알 일정거리내에 있으면 방향전환
}
