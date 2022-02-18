using MiniJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BattlePanel : MonoBehaviour
{
    private Text ouserText;
    private Text rivalText;
    private Text timerText;
    private Text scoreText;
    private Text resultText;
    private Text oursideText;
    private Text opponentText;

    private Image ouserImage;
    private Image rivalImage;
    private Image timerImage;

    private Button startBtn;
    private Button backBtn;
    private Button resultBtn;
    private Button againBtn;

    private GameObject userPlayer;
    private GameObject gamePlayer;
    private GameObject resultPanel;
    private Transform honeybeeParent;
    private Transform honeybee;

    private Vector3 honeybeePoint;
    private int ourScore;
    private int enemyScore;
    private float sumTime = -10;
    private float lastTime = 0;
    private float createTime = 0;

    private bool isStart;
    private void Awake()
    {
        honeybeeParent = transform.Find("GamePanel/HoneybeeParent");
        honeybee = transform.Find("GamePanel/Honeybee");
        userPlayer = transform.Find("UserPlayer").gameObject;
        gamePlayer = transform.Find("GamePanel").gameObject;
        resultPanel = transform.Find("ResultPanel").gameObject;

        ouserText = transform.Find("UserPlayer/Ourse/Text").GetComponent<Text>();
        rivalText = transform.Find("UserPlayer/Rival/Text").GetComponent<Text>();
        timerText = transform.Find("GamePanel/Timer/Reading").GetComponent<Text>();
        scoreText = transform.Find("GamePanel/ScoreText").GetComponent<Text>();
        resultText = transform.Find("ResultPanel/Epilogue").GetComponent<Text>();
        oursideText = transform.Find("ResultPanel/UserText").GetComponent<Text>();
        opponentText = transform.Find("ResultPanel/EnemyText").GetComponent<Text>();

        ouserImage = transform.Find("UserPlayer/Ourse/Image").GetComponent<Image>();
        rivalImage = transform.Find("UserPlayer/Rival/Image").GetComponent<Image>();
        timerImage = transform.Find("GamePanel/Timer/Image").GetComponent<Image>();

        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        startBtn = transform.Find("UserPlayer/StartBtn").GetComponent<Button>();
        resultBtn = transform.Find("ResultPanel/BackBtn").GetComponent<Button>();
        againBtn = transform.Find("ResultPanel/AgainBtn").GetComponent<Button>();

        backBtn.onClick.AddListener(ClosePanel);
        resultBtn.onClick.AddListener(ClosePanel);
        startBtn.onClick.AddListener(OpenGame);
        againBtn.onClick.AddListener(AgainGame);
    }

    public void OpenPanel()
    {
        isStart = false;
        gameObject.SetActive(true);
        userPlayer.SetActive(true);
        gamePlayer.SetActive(false);
        resultPanel.SetActive(false);
        ouserText.text = DataTool.roleName;
        StartCoroutine(RandomNickname());
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void AddScore(int number)
    {
        ourScore += number;
        scoreText.text = string.Format("得分:{0}", ourScore);
    }

    private void OpenGame()
    {
        sumTime = 30;
        ourScore = 0;
        enemyScore = Random.Range(20,30);
        userPlayer.SetActive(false);
        gamePlayer.SetActive(true);
        timerImage.fillAmount = 1;
        timerText.text = "30s";
        scoreText.text = string.Format("得分:{0}", ourScore);
        isStart = true;
        CreateHoneybee();
    }

    private void OpenResult()
    {
        resultPanel.SetActive(true);
        oursideText.text = string.Format("{0}:得分:{1}", DataTool.roleName, ourScore);
        opponentText.text = string.Format("{0}:得分:{1}", rivalText.text, enemyScore);
        if(ourScore > enemyScore)
        {
            DataTool.farmData.gamePK += 1;
            resultText.text = "恭喜，获得胜利！";
        }
        else if (ourScore == enemyScore)
        {
            resultText.text = "平局，继续加油！";
        }
        else
        {
            resultText.text = "失败，再接再厉！";
        }
    }

    private void AgainGame()
    {

    }

    private void Update()
    {
        if (isStart)
        {
            if (sumTime >= 0)
            {
                lastTime += Time.deltaTime;
                if (lastTime >= 1)
                {
                    lastTime = 0;
                    sumTime -= 1;
                    timerText.text = sumTime.ToString("F0");
                    timerImage.fillAmount = sumTime / 30;
                    if (sumTime <= 0)
                    {
                        isStart = false;
                        OpenResult();
                        ObjectPool.Instance.Clear("Honeybee");
                        Debug.Log("游戏结束");
                    }
                }
                if(sumTime >= 5)
                {
                    createTime += Time.deltaTime;
                    if (createTime >= 3)
                    {
                        createTime = 0;
                        CreateHoneybee();
                    }
                }
            }
        }
    }
    private void CreateHoneybee()
    {
        int ran = Random.Range(3,5);
        for (int i = 0; i < ran; i++)
        {
            StartCoroutine(DelyHoneybee(i));
        }
    }

    private IEnumerator DelyHoneybee(float dely)
    {
        yield return new WaitForSeconds(dely);
        var item = ObjectPool.Instance.CreateObject("Honeybee", honeybee.gameObject);
        item.SetActive(true);
        item.transform.SetParent(honeybeeParent);
        item.transform.localScale = Vector3.zero;
        honeybeePoint.x = Random.Range(-335, 335);
        item.transform.localPosition = honeybeePoint;
        item.GetComponent<HoneybeeMove>().SetMove();
    }

    //获取随机昵称
    private IEnumerator RandomNickname()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(DataTool.getRamdomNickName);
        webRequest.SetRequestHeader("Authorization", DataTool.token);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.error != null)
        {
            Debug.Log("请求网络错误:" + webRequest.error);
        }
        else
        {
            Debug.Log("随机昵称" + webRequest.downloadHandler.text);
            Dictionary<string, object> pageData = Json.Deserialize(webRequest.downloadHandler.text) as Dictionary<string, object>;
            if (pageData["code"].ToString() == "0")
            {
                rivalText.text = pageData["data"].ToString();
            }
        }
    }
}
