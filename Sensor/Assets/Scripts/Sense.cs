using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sense : MonoBehaviour
{
    // 디버그 모드 인지 확인용 변수
    public bool bDebug;

    // Aspect 우리가 만든 클래스(플레이어 인지 적인지 확인용)
    public Aspect.AspectName aspectName;

    // 몇 초 주기로 확인 용 변수
    public float detectionRate;


    // protected : 자식 클래스에만 공개
    // 시간 더하기 전용 변수
    protected float elapsedTime;

    // 자식 클래스에서 Start()메소드 내용을 상속
    // 메소드를 자식클래스에서 하나 하나 만들어서 스타트 메소드를 구현해도 되는데
    // 그러면 상속클래스 만드는게 의미 없음(공통점을 부모 클래스에 넣어놓기 때문)
    
    // virtual(가상) 으로 메소드를 만들어서 자식에게 상속
    // 가상으로 만든 이유는 자식 오브젝트마다 메소드 내용이 다르기 때문
    protected virtual void Initialise() { }

    protected virtual void UpdateSense() { }

    private void Start()
    {
        elapsedTime = 0f;
        Initialise();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > detectionRate)
        {
            UpdateSense();
            elapsedTime = 0f;
        }
    }
}
