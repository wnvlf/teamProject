using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class PlayerShopManager : MonoBehaviour
{
    public int TempGold { get; private set; }
    public int RerollCount { get; private set; }
    public int RerollCost => BaseRerollCost + RerollCount;

    public List<DiceData> TempDices = new();
    public List<BattleItemSo> TempItems = new();

    [Header("Settings")]
    [SerializeField] private int baseRerollCost = 1;
    public int BaseRerollCost => baseRerollCost;
    [SerializeField] private string ShopBGMKey;
    [SerializeField] private string PurchaseSFXKey;

    [Header("임시 테스트용 (인벤토리 시스템 확정 전까지)")]
    [SerializeField] private PlayerDeckData testDiceDatabase;

    [Header("UI References")]
    [SerializeField] private GameObject shopCanvas;
    [SerializeField] private RectTransform shopPanel;
    [SerializeField] private ShopPanelAnimator shopAnimator;

    public event System.Action<int> OnGoldChanged;

    [SerializeField] private DiceData defaultDice;

    public bool IsOpen { get; private set; }

    private AudioManager _audioManager;
    private ResourceManager _resourceManager;
    private ItemManager _itemManager;

    [Inject]
    public void Construct(AudioManager audioManager, ResourceManager resourceManager, ItemManager itemManager)
    {
        _audioManager = audioManager;
        _resourceManager = resourceManager;
        _itemManager = itemManager;
    }

    private void Start()
    {
        OpenWithAnimation();
        _audioManager.PlayBgm(ShopBGMKey);
    }

    public void Open()
    {
        TempGold = _resourceManager.gold;
        RerollCount = 0;

        TempDices = new List<DiceData>();
        TempItems = new List<BattleItemSo>(_itemManager.items);

        IsOpen = true;
        OnGoldChanged?.Invoke(TempGold);
    }

    public async void OpenWithAnimation()
    {
        Open();
        shopCanvas.SetActive(true);
        if (shopAnimator != null && shopAnimator.gameObject != null)
        {
            await shopAnimator.Show();
        }
        else
        {
            Debug.Log("shopAnimator 또는 gameObject가 null입니다!");
        }
    }

    public void Commit()
    {
        if (!IsOpen)
        {
            Debug.Log("Commit 호출됐지만 상점이 열려있지 않습니다.");
            return;
        }
        _resourceManager.gold = TempGold;
        _itemManager.items = new List<BattleItemSo>(TempItems);
        _resourceManager.Save();
        _itemManager.Save();
        IsOpen = false;
    }

    public async UniTask CommitWithAnimation()
    {
        Commit();
        await shopAnimator.Hide();
    }

    public void Discard()
    {
        IsOpen = false;
        shopAnimator.Hide().Forget();
        Debug.Log("상점 변경사항 폐기");
    }

    //--- 구매 / 리롤 ---

    public bool TryPurchaseDice(DiceData dice)
    {
        int cost = dice.gold;
        if (!HasEnoughGold(cost)) return false;
        _audioManager.PlaySfx(PurchaseSFXKey);
        SpendGold(cost);
        TempDices.Add(dice);
        return true;
    }

    public bool TryPurchaseItem(BattleItemSo item)
    {
        int cost = item.gold;
        if (!HasEnoughGold(cost)) return false;
        _audioManager.PlaySfx(PurchaseSFXKey);
        SpendGold(cost);
        TempItems.Add(item);
        return true;
    }

    public bool TryReroll()
    {
        if (!HasEnoughGold(RerollCost)) return false;

        SpendGold(RerollCost);
        RerollCount++;
        return true;
    }

    //---------- Private -------------

    private bool HasEnoughGold(int amount) => TempGold >= amount;

    private void SpendGold(int amount)
    {
        TempGold -= amount;
        OnGoldChanged?.Invoke(TempGold);
    }
}