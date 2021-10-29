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
        //���࿡ ���������� ������
        if (auth.CurrentUser != null)
        {
            print(auth.CurrentUser.Email);
            print("�α��� ����");
        }
        //�׷��� ������
        else
        {
            print("�α׾ƿ� ����");
        }
    }
    public void OnClickSignIn()
    {
        if (inputEmail.text.Length == 0 || inputPassword.text.Length == 0)
        {
            print("������ �� �Է����ּ���!");
            return;
        }
        StartCoroutine(SignIn(inputEmail.text, inputPassword.text));
    }
    IEnumerator SignIn(string email, string password)
    {
        //�α��� �õ�
        var task = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        //����� �Ϸ�ɶ����� ��ٸ���
        yield return new WaitUntil(() => task.IsCompleted);

        //���࿡ ������ ���ٸ�
        if (task.Exception == null)
        {
            print("ȸ������ ����");
        }
        else
        {
            print("ȸ������ ���� : " + task.Exception);
        }
    }


    public void OnClickLogIn()
    {
        if (inputEmail.text.Length == 0 || inputPassword.text.Length == 0)
        {
            print("������ �� �Է����ּ���!");
            return;
        }
        StartCoroutine(Login(inputEmail.text, inputPassword.text));
    }
    IEnumerator Login(string email, string password)
    {
        //�α��� �õ�
        var task = auth.SignInWithEmailAndPasswordAsync(email, password);
        //����� �Ϸ�ɶ����� ��ٸ���
        yield return new WaitUntil(() => task.IsCompleted);

        //���࿡ ������ ���ٸ�
        if (task.Exception == null)
        {
            print("�α��� ����");
        }
        else
        {
            print("�α��� ���� : " + task.Exception);
        }

    }
    public void OnClickLogOut()
    {
        //�α׾ƿ�
        auth.SignOut();
    }
}
