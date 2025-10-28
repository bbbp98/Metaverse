# 프로젝트 소개
2가지의 캐주얼 게임을 즐길 수 있는 공간입니다.

# 개발 기간
2025.10.23 ~ 2025.10.28

# 실행 화면

NPC와 상호작용을 통해 미니게임을 할 수 있습니다.

<img width="742" height="415" alt="Image" src="https://github.com/user-attachments/assets/b8a0627f-31d6-40c1-8a80-7dff41ad472e" />|<img width="740" height="417" alt="Image" src="https://github.com/user-attachments/assets/dae6ed30-1a89-470e-90b5-975e4cbe2cb1" />|<img width="741" height="418" alt="Image" src="https://github.com/user-attachments/assets/e7895bc1-262c-41b1-978f-4c2d357ffe55" />
|---|---|---



# 구현 목록
### GameManager
`UIManager`, `AudioManager` 등을 관리하는 매니저입니다.

이번 프로젝트에서는 씬 전환 및 UI업데이트를 관리하는 것이 주요 역할입니다.

>[GameManager.cs](https://github.com/bbbp98/Metaverse/blob/main/Assets/01.%20Scripts/Manager/GameManager.cs)


### FadeManager
`Fade-In` / `Fade-Out` 기능을 수행하는 매니저입니다.

이미지 파일의 a 값을 보간을 통해 조정해서 화면 전환 시 부드럽게 전환되는 모습을 보여줍니다.
```cs
private IEnumerator Fade(float targetAlpha)
{
    if (fadeImage == null)
    {
        Debug.Log("fade image is null");
        yield break;
    }

    Color color = fadeImage.color;
    float startAlpha = color.a;
    float time = 0f;

    while (time < fadeDuration)
    {
        time += Time.deltaTime;
        color.a = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
        fadeImage.color = color;
        yield return null;
    }

    color.a = targetAlpha;
    fadeImage.color = color;
}
```

>[FadeManager.cs](https://github.com/bbbp98/Metaverse/blob/main/Assets/01.%20Scripts/Manager/FadeManager.cs)


### UIManager
플레이어의 코인, NPC와의 대화창 등 전체적인 UI를 관리합니다.

NPC와 대화할 때 타자가 입력되듯이 한 글자씩 화면에 출력하는 연출을 만들고 싶어 `Coroutine`을 사용해 기능을 구현했습니다.
```cs
private IEnumerator TypeTextEffect(string text)
{
    text = text.Replace("\\n", "\n");
    isTyping = true;
    currentText = text;
    dialogueText.text = "";

    foreach (char c in text)
    {
        dialogueText.text += c;
        yield return new WaitForSeconds(typingSpeed);
    }

    isTyping = false;
}
```

>[UIManager.cs](https://github.com/bbbp98/Metaverse/blob/main/Assets/01.%20Scripts/Manager/UIManager.cs)


### NPC
NPC와 상호작용을 해 짧은 대화를 할 수 있고, 플레이어의 선택에 따라 미니 게임 씬으로 화면을 전환합니다.

NPC와의 상호작용은 `Collider`를 사용해 UI로 상호작용을 위한 키를 표시합니다.

`IInteractable`이라는 인터페이스를 사용해 NPC마다 다른 상호작용을 할 수 있도록 구현했습니다.

>[IInteractable.cs](https://github.com/bbbp98/Metaverse/blob/main/Assets/01.%20Scripts/Interface/IInteractable.cs)
<br>
[NPCBase.cs](https://github.com/bbbp98/Metaverse/blob/main/Assets/01.%20Scripts/Entity/NPCBase.cs)
<br>
[NPCPortal.cs](https://github.com/bbbp98/Metaverse/blob/main/Assets/01.%20Scripts/Entity/NPCPortal.cs)

### Camera
카메라와 플레이어의 `offset`을 지정하고, 플레이어의 위치를 따라 다니도록 했습니다.

카메라가 움직이는 경우 타일맵으로 맵을 만든 환경이어서 타일맵의 `Bounds`를 사용해 범위를 제한했습니다.

>[CameraFollow.cs](https://github.com/bbbp98/Metaverse/blob/main/Assets/01.%20Scripts/Camera/CameraFollow.cs)
<br>
[CameraBoundsSetter.cs](https://github.com/bbbp98/Metaverse/blob/main/Assets/01.%20Scripts/Camera/CameraBoundsSetter.cs)



## 미니 게임
