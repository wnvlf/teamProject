using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class MapStageFlowController
{
    private readonly MapProgressStore progressStore;
    private readonly SceneController sceneController;
    private readonly MapScrollAnimation eventPopup;

    public MapStageFlowController(MapProgressStore progressStore, SceneController sceneController, MapScrollAnimation eventPopup)
    {
        this.progressStore = progressStore;
        this.sceneController = sceneController;
        this.eventPopup = eventPopup;
    }

    public bool HasSavedProgress => progressStore.HasProgress;
    public MapTemplate SavedTemplate => progressStore.CurrentTemplate;
    public int SavedCurrentNodeId => progressStore.CurrentNodeId;
    public IReadOnlyDictionary<int, NodeState> SavedNodeStates => progressStore.NodeStates;

    public void SaveProgress(MapTemplate template, int currentNodeId, IEnumerable<NodeView> nodeViews)
    {
        progressStore.Save(template, currentNodeId, nodeViews);
    }

    public void ClearProgress()
    {
        progressStore.Clear();
    }

    public UniTask ShowEventAsync() => eventPopup.ShowEventAsync();

    public bool TryTransitionToNodeScene(MNodeType type)
    {
        switch (type)
        {
            case MNodeType.Shop: sceneController.LoadShopScene(); return true;
            case MNodeType.Battle: sceneController.LoadGameScene(); return true;
            case MNodeType.Boss: sceneController.LoadBossScene(); return true;
            default: return false;
        }
    }
}
