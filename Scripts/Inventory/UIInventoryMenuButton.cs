using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryMenuButton : MonoBehaviour
{
    [SerializeField] Sprite[] meun;
    [SerializeField] Image image;

    private e_MenuType menuType = e_MenuType.Face;
    private Action<e_MenuType> onClickCallback = null;

    public void InitButton(e_MenuType _menuType, Action<e_MenuType> _clickCallback = null)
    {
        menuType = _menuType;
        onClickCallback = _clickCallback;
        SetMenuButton();
    }

    private void SetMenuButton()
    {
        //번역
        switch (menuType)
        {
            case e_MenuType.Face:
                image.sprite = meun[0];
                break;
            case e_MenuType.Top:
                image.sprite = meun[1];
                break;
        }
    }

    public void OnClick()
    {
        //?. : Null연산자 (변수사용x)
        onClickCallback?.Invoke(menuType);
    }
}
