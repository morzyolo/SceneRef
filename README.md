# SceneRef

**SceneRef** is used to keep a reference to a scene asset so that it can be used during game runtime.

# How to install

You can add `https://github.com/morzyolo/SceneRef.git` to Package Manager

![add package](https://github.com/morzyolo/SceneRef/assets/108426671/6e4d4911-3755-48c5-9a4c-d3d6aa93f121)

![insert link](https://github.com/morzyolo/SceneRef/assets/108426671/723ac307-d7b9-4dd4-b3ca-305e19fcc934)

or add `"com.morzyolo.sceneref": "https://github.com/morzyolo/SceneRef.git"` to `Packages/manifest.json`

# How to use

1. Declare field in MonoBehaviour class
    ```csharp
    [SerializeField] private SceneRef _sceneRef;
    ```

2. Drag a scene from the project folder into the component field in the Inspector

3. Invoke a scene transition to a different scene by utilizing the declared field.
    ```csharp
    SceneManager.LoadScene(_sceneRef);
    ```

# License
[MIT](https://github.com/morzyolo/SceneRef/blob/master/LICENSE)
