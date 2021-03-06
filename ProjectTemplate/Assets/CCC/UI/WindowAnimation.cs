﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

namespace CCC.UI
{
    public class WindowAnimation : MonoBehaviour
    {

        [Header("Components")]
        public RectTransform windowBg;
        public CanvasGroup content;
        public Image backBg;

        [Header("Window Bg Settings")]
        [Range(0, 1)]
        public float verticalStart = 1;
        [Range(0, 1)]
        public float horizontalStart = 0;
        [Range(0, 1)]
        public float fadeStart = 1;
        public bool openOnAwake = true;

        [Header("Open")]
        public float openTime = 0.35f;
        public Ease openEase = Ease.OutSine;

        [Header("Exit")]
        public float exitTime = 0.35f;
        public Ease exitEase = Ease.InSine;

        [Header("Size")]
        public bool autoDetectSize = true;
        public Vector2 size;


        Vector2 smallV;
        Vector2 bigV;

        private RectTransform bgTr;
        private Image bgImage;
        private float bgImageAlpha;
        private bool isOpen = false;
        private float backBgAlpha;

        void Awake()
        {
            bgTr = windowBg.GetComponent<RectTransform>();
            bgImage = windowBg.GetComponent<Image>();
            if (bgImage)
                bgImageAlpha = bgImage.color.a;
            if (backBg)
                backBgAlpha = backBg.color.a;

            bigV = autoDetectSize ? bgTr.sizeDelta : size;
            smallV = new Vector2(bigV.x * horizontalStart, bigV.y * verticalStart);

            InstantClose();

            if (openOnAwake)
                Open();
        }

        public void Open(TweenCallback onComplete = null)
        {
            isOpen = true;
            bgTr.gameObject.SetActive(true);

            if (bgImage != null)
            {
                bgImage.DOKill();
                bgImage.DOFade(bgImageAlpha, openTime * 0.75f);
            }

            if (backBg != null)
            {
                backBg.gameObject.SetActive(true);
                backBg.DOKill();
                backBg.DOFade(backBgAlpha, openTime);
            }

            if (content != null)
            {
                content.gameObject.SetActive(true);


                bgTr.DOKill();
                bgTr.DOSizeDelta(bigV, openTime).SetEase(openEase);

                content.DOKill();
                content.DOFade(1, openTime).SetDelay(openTime * 0.75f).SetEase(openEase).OnComplete(delegate ()
                {
                    content.blocksRaycasts = true;
                    if (onComplete != null)
                        onComplete.Invoke();
                });
            }
            else
            {
                bgTr.DOKill();
                bgTr.DOSizeDelta(bigV, openTime).SetEase(openEase).OnComplete(onComplete);
            }
        }

        public void Close(TweenCallback onComplete = null)
        {
            isOpen = false;

            float delay = content == null ? 0 : exitTime * 0.75f;


            //Le content se fade-out en premier
            if (content != null)
            {
                content.DOKill();
                content.DOFade(0, exitTime * 0.75f).SetEase(exitEase);
                content.blocksRaycasts = false;
            }

            //Le reste a du délai (potentiellement)
            if (bgImage != null)
            {
                bgImage.DOKill();
                bgImage.DOFade(fadeStart, exitTime).SetDelay(delay);
            }

            if (backBg != null)
            {
                backBg.DOKill();
                backBg.DOFade(0, exitTime + delay);//.SetDelay(delay);
            }

            bgTr.DOKill();
            bgTr.DOSizeDelta(smallV, exitTime).SetDelay(delay).SetEase(exitEase).OnComplete(delegate ()
            {
                bgTr.gameObject.SetActive(false);
                if (onComplete != null)
                    onComplete.Invoke();
            });
        }

        public void InstantOpen()
        {
            bgTr.DOKill();
            bgTr.sizeDelta = bigV;
            bgTr.gameObject.SetActive(true);

            if (bgImage != null)
            {
                bgImage.DOKill();
                bgImage.color = new Color(bgImage.color.r, bgImage.color.g, bgImage.color.b, bgImageAlpha);
            }

            if (backBg != null)
            {
                backBg.DOKill();
                backBg.color = new Color(backBg.color.r, backBg.color.g, backBg.color.b, backBgAlpha);
                backBg.gameObject.SetActive(true);
            }

            if (content != null)
            {
                content.DOKill();
                content.blocksRaycasts = true;
                content.alpha = 1;
                content.gameObject.SetActive(true);
            }
            isOpen = true;
        }

        public void InstantClose()
        {
            isOpen = false;
            bgTr.DOKill();
            bgTr.sizeDelta = smallV;
            bgTr.gameObject.SetActive(false);

            if (bgImage != null)
            {
                bgImage.DOKill();
                bgImage.color = new Color(bgImage.color.r, bgImage.color.g, bgImage.color.b, fadeStart);
            }

            if(backBg != null)
            {
                backBg.DOKill();
                backBg.color = new Color(backBg.color.r, backBg.color.g, backBg.color.b, 0);
                backBg.gameObject.SetActive(false);
            }

            if (content != null)
            {
                content.DOKill();
                content.blocksRaycasts = false;
                content.alpha = 0;
                content.gameObject.SetActive(false);
            }
        }

        public bool IsOpen() { return isOpen; }
    }
}
