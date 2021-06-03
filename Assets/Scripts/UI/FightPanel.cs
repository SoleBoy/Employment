using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightPanel : MonoBehaviour
{
    private Text titleText;
    private Text timeText;

    private Button fixBtn;
    private Button backBtn;
    private Button fastBtn;

    private GameObject settlePanel;
    private Transform roleParent;
    private Transform foeParent;

    private int roleAttack; 
    private int foeAttack;
    private int roleIndex;
    private int foeIndex;
    private float currentTime;
    private bool isVictory;
    public List<int> attackRoles = new List<int>();
    public List<int> attackFoes = new List<int>();
    private List<FightItem> roles = new List<FightItem>();
    private List<FightItem> foes = new List<FightItem>();
    private void Awake()
    {
        roleParent = transform.Find("OurSide");
        foeParent = transform.Find("Opponent");
        settlePanel = transform.Find("SettlePanel").gameObject;

        titleText = transform.Find("SettlePanel/TitleText").GetComponent<Text>();
        timeText = transform.Find("TimeImage/TimeText").GetComponent<Text>();

        fixBtn = transform.Find("SettlePanel/FixBtn").GetComponent<Button>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        fastBtn= transform.Find("Bottom/FastBtn").GetComponent<Button>();
        fixBtn.onClick.AddListener(OpenReward);
        fastBtn.onClick.AddListener(OpenFast);
        backBtn.onClick.AddListener(ClosePanel);
        InitData();
    }

    public void OpenPanel(string enemyName,float grade,int time)
    {
        currentTime = time;
        isVictory = false;
        attackRoles.Clear();
        attackFoes.Clear();
        gameObject.SetActive(true);
        timeText.text = string.Format("{0}秒",time);
        for (int i = 0; i < foes.Count; i++)
        {
            attackFoes.Add(i);
            if (i == 0)
            {
                foes[i].SetFight(enemyName,grade,grade*20);
            }
            else
            {
                foes[i].SetFight("兽"+i,Random.Range(15,20), Random.Range(100, 200));
            }
        }
        for (int i = 0; i < roles.Count; i++)
        {
            attackRoles.Add(i);
            if (i == 0)
            {
                roles[i].SetFight("张小牛",15,300);
            }
            else
            {
                roles[i].SetFight("兽" + i,Random.Range(15, 20), Random.Range(100, 200));
            }
        }
        roleAttack = Random.Range(0,attackRoles.Count);
        foeAttack = Random.Range(0, attackFoes.Count);
        Search(true);
        Search(false);
        StartCoroutine(AttackAnimation());
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    //寻找最低血量
    public void Search(bool isRole)
    {
        if (isRole)
        {
            for (int i = 0; i < attackRoles.Count; i++)
            {
                if (roles[attackRoles[i]].curretHp <= 0)
                {
                    attackRoles.Remove(attackRoles[i]);
                    break;
                }
            }
            if(attackRoles.Count <= 0)
            {
                JudeVictory(false);
            }
        }
        else
        {
            for (int i = 0; i < attackFoes.Count; i++)
            {
                if (foes[attackFoes[i]].curretHp <= 0)
                {
                    attackFoes.Remove(attackFoes[i]);
                    break;
                }
            }
            if (attackFoes.Count <= 0)
            {
                JudeVictory(true);
            }
        }
    }
    //玩家攻击
    public void RoleAttack(int count,bool isFast,float hurt)
    {
        if (roleIndex >= attackRoles.Count)
        {
            roleIndex = 0;
        }
        for (int i = 0; i < count; i++)
        {
            if (foeAttack >= attackFoes.Count)
            {
                foeAttack = 0;
            }
            if (attackFoes.Count >= 1)
            {
                foes[attackFoes[foeAttack]].OnHit(isFast, hurt);
                foeAttack += 1;
            }
        }
        roleIndex += 1;
    }
    //敌对攻击
    public void FoeAttack(int count,bool isFast,float hurt)
    {
        if (foeIndex >= attackFoes.Count)
        {
            foeIndex = 0;
        }
        for (int i = 0; i < count; i++)
        {
            if (roleAttack >= attackRoles.Count)
            {
                roleAttack = 0;
            }
            if(attackRoles.Count >= 1)
            {
                roles[attackRoles[roleAttack]].OnHit(isFast, hurt);
                roleAttack += 1;
            }
        }
        foeIndex += 1;
    }
    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        if(!isVictory)
        {
            RoleAttack(Random.Range(1, attackFoes.Count), false,Random.Range(45, 80));
            yield return new WaitForSeconds(0.5f);
            if (!isVictory)
            {
                FoeAttack(Random.Range(1, attackRoles.Count),false,Random.Range(45, 80));
                StartCoroutine(AttackAnimation());
            }
        }
    }
    //数据初始
    private void InitData()
    {
        for (int i = 0; i < roleParent.childCount; i++)
        {
            FightItem item = new FightItem(roleParent.GetChild(i));
            roles.Add(item);
        }
        for (int i = 0; i < foeParent.childCount; i++)
        {
            FightItem item = new FightItem(foeParent.GetChild(i));
            foes.Add(item);
        }
    }
    //领取奖励
    private void OpenReward()
    {
        gameObject.SetActive(false);
        settlePanel.SetActive(false);
        UIManager.Instance.CloningTips("领取奖励成功");
    }
    //快速战斗
    private void OpenFast()
    {
        while (!isVictory)
        {
            RoleAttack(1,true,Random.Range(45, 80));
            FoeAttack(1,true,Random.Range(45, 80));
        }
    }
    //胜利or失败
    private void JudeVictory(bool isVictory)
    {
        if(isVictory)
        {
            titleText.text = "胜利";
        }
        else
        {
            titleText.text = "失败";
        }
        this.isVictory = true;
        settlePanel.SetActive(true);
    }
    //private void Update()
    //{
    //    if(!settlePanel.activeInHierarchy)
    //    {
    //        if (currentTime >= 0)
    //        {
    //            currentTime -= Time.deltaTime;
    //            timeText.text = string.Format("{0:F0}秒", currentTime);
    //            if (currentTime <= 0)
    //            {
    //                settlePanel.SetActive(true);
    //            }
    //        }
    //    }
    //}

    //战斗信息
    private class FightItem
    {
        private Transform fightParent;
        private Image headSprite;
        private Image hpbarImage;
        private Text nameText;
        private Text hpText;
        public float curretHp;
        private float maxHp;
        private bool isRole;
        public FightItem(Transform parent)
        {
            fightParent = parent;
            headSprite = parent.Find("Head").GetComponent<Image>();
            hpbarImage = parent.Find("Bar").GetComponent<Image>();
            nameText = parent.Find("NameText").GetComponent<Text>();
            hpText = parent.Find("HpText").GetComponent<Text>();
            isRole = parent.parent.name == "OurSide";
        }
   
        public void SetFight(string name,float grade,float hp)
        {
            curretHp = hp;
            maxHp = curretHp;
            hpbarImage.fillAmount = 1;
            nameText.text = string.Format("{0} LV.{1}", name,grade);
            hpText.text = string.Format("{0}",hp);
            fightParent.gameObject.SetActive(true);
        }
        //受伤
        public void OnHit(bool isFas,float hurt)
        {
            if(curretHp >= 0)
            {
                curretHp -= hurt;
                if (curretHp <= 0)
                {
                    hpbarImage.fillAmount = 0;
                    fightParent.gameObject.SetActive(false);
                    UIManager.Instance.fightPanel.Search(isRole);
                }
                else if(!isFas)
                {
                    hpText.text = string.Format("{0}", curretHp);
                    hpbarImage.fillAmount = curretHp / maxHp;
                    UIManager.Instance.CloningBlood(fightParent.localPosition, hurt);
                }
            }
        }
    }
}
