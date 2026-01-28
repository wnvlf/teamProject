using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class HomeScreenUI : MonoBehaviour
{
    public static HomeScreenUI instance;
    [Header("text")]
    public TextMeshProUGUI bestRoundtext;
    public TextMeshProUGUI bestScoretext;
    public TextMeshProUGUI synergyGoldText;
    public GameObject synergyButton;
    public GameObject synergy;

    public int synergyGold;
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        bestRoundtext.text = "최고 라운드: " + Player.instance.player.bestRound.ToString();
        bestScoretext.text = "최고 점수: " + Player.instance.player.bestScore.ToString();
    }

    public void PushSynergyImage(ItemSo item)
    {
        for (int i = 0; i < synergy.transform.childCount; i++)
        {
            if (!synergy.transform.GetChild(i).gameObject.activeSelf)
            {
                synergyGold += item.Gold;
                synergyGoldText.text = "필요 골드: " + synergyGold.ToString();
                synergy.transform.GetChild(i).GetComponent<SynergyDIce>().Dice = item;
                synergy.transform.GetChild(i).GetComponent<Image>().sprite = item.itemIcon;
                synergy.transform.GetChild(i).gameObject.SetActive(true);
                return;
            }
        }
    }

    public void PopSynergyImage()
    {
        synergyGoldText.text = "필요 골드: " + synergyGold.ToString();
    }

    public void SynergyButtonImageUpdate()
    {
        for(int i = 0; i < synergyButton.transform.childCount; i++)
        {
            Transform synergyChild = synergyButton.transform.GetChild(i);
            if (Player.instance.player.itemSo1[i] == null)
            {
                synergyChild.gameObject.SetActive(false);
            }
            else
            {
                synergyChild.GetComponent<Image>().sprite =
                    Player.instance.player.itemSo1[i].itemIcon;
                if (!synergyChild.gameObject.activeSelf)
                    synergyChild.gameObject.SetActive(true);
            }      
        }
    }
    


}
