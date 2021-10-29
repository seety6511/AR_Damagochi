using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using System;
using UnityEngine.UI;
public class FireAuth : MonoBehaviour
{
    FirebaseAuth auth;
    //UI
    public InputField inputEmail;
    public InputField inputPassword;
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        auth.StateChanged += OnChangedAUthState;

    }
    private void OnDestroy()
    {
        auth.StateChanged -= OnChangedAUthState;
    }
    void OnChangedAUthState(object sender, EventArgs s)

    {
        //만약에 유저정보가 있으면
        if (auth.CurrentUser != null)
        {
            print(auth.CurrentUser.Email);
            print("로그인 상태");
        }
        //그렇지 않으면
        else
        {
            print("로그아웃 상태");
        }
    }
    public void OnClickSignIn()
    {
        if (inputEmail.text.Length == 0 || inputPassword.text.Length == 0)
        {
            print("정보를 다 입력해주세요!");
            return;
        }
        StartCoroutine(SignIn(inputEmail.text, inputPassword.text));
    }
    IEnumerator SignIn(string email, string password)
    {
        //로그인 시도
        var task = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        //통신이 완료될때까지 기다린다
        yield return new WaitUntil(() => task.IsCompleted);

        //만약에 에러가 없다면
        if (task.Exception == null)
        {
            print("회원가입 성공");
        }
        else
        {
            print("회원가입 실패 : " + task.Exception);
        }
    }


    public void OnClickLogIn()
    {
        if (inputEmail.text.Length == 0 || inputPassword.text.Length == 0)
        {
            print("정보를 다 입력해주세요!");
            return;
        }
        StartCoroutine(Login(inputEmail.text, inputPassword.text));
    }
    IEnumerator Login(string email, string password)
    {
        //로그인 시도
        var task = auth.SignInWithEmailAndPasswordAsync(email, password);
        //통신이 완료될때까지 기다린다
        yield return new WaitUntil(() => task.IsCompleted);

        //만약에 에러가 없다면
        if (task.Exception == null)
        {
            print("로그인 성공");
        }
        else
        {
            print("로그인 실패 : " + task.Exception);
        }

    }
    public void OnClickLogOut()
    {
        //로그아웃
        auth.SignOut();
    }
}
