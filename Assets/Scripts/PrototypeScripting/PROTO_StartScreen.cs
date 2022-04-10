﻿using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

namespace PrototypeScripting
{
    public class PROTO_StartScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeSpeed;
        [SerializeField] private AudioSource _audio;
        private bool _fadeStarted;

        private void Awake()
        {
            _fadeStarted = false;
        }

        private void Update()
        {
            var gamePadAnyKey =
                Gamepad.current != null &&
                Gamepad.current.allControls.Any(x => x is ButtonControl button && button.isPressed && !x.synthetic);

            var keyboardAnyKey =
                Keyboard.current != null &&
                Keyboard.current.anyKey.wasPressedThisFrame;

            switch (_fadeStarted)
            {
                case false when (gamePadAnyKey || keyboardAnyKey):
                {
                    _fadeStarted = true;
                    _audio.DOFade(0.0f, 2.0f);
                    break;
                }
                case true:
                {
                    _canvasGroup.alpha += Time.deltaTime * _fadeSpeed;
                    if (_canvasGroup.alpha >= 1.0f)
                    {
                        SceneManager.LoadScene("Demo Level", LoadSceneMode.Single);
                    }
                    break;
                }
            }
        }
    }
}