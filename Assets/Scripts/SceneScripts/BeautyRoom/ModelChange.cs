using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelChange : MonoBehaviour
{
    // Start is called before the first frame update
    public int MaxModelId;
    public int CurrentModelId;

    public Image ModelImage;
    void Start()
    {
        gameObject.SetActive(false);
    }
    private void LoadModel(int modelId)
    {
        var user = GameManager.Instance.User;
        var path = GameManager.Instance.GetModelImagePath(user.Gender, modelId);
        ModelImage.sprite = Resources.Load<Sprite>(path);
    }
    public void NextModel()
    {
        CurrentModelId++;
        if (CurrentModelId > MaxModelId) CurrentModelId = 0;
        LoadModel(CurrentModelId);
    }
    public void PreviewModel()
    {
        CurrentModelId--;
        if (CurrentModelId < 0) CurrentModelId = MaxModelId;
        LoadModel(CurrentModelId);
    }

    public void Change()
    {

        ///gameObject.SetActive(false);
        StartCoroutine(ChangeModel());
    }
    private IEnumerator ChangeModel()
    {
        var user = GameManager.Instance.User;
        var apiServer = new ApiServer();
        var packet = new ApiRequest.ChangeModel();
        packet.UserName = user.UserName;
        packet.ModelId = CurrentModelId;
        yield return StartCoroutine(apiServer.ApiRequestChangeModel(packet));

        gameObject.SetActive(false);
    }
    public void Cancel()
    {
        gameObject.SetActive(false);
    }
    public void Open()
    {
        gameObject.SetActive(true);
    }
}
