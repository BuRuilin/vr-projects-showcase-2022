using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : SQLiteHelper
{
    public int TopicCount;
    private string[] _perLineData;
    private string[][] _xuanzetiArray;
    private string[][] _panduantiArray;
    private  List<bool> IsAnswer = new List<bool>();
    private readonly List<int> SelectAnswerIndex = new List<int>();
    private readonly List<bool> SelectIsRight = new List<bool>();
    public List<Toggle> SelecToggles;
    public List<Text> SelecAnswer;
    public Button _nextTopicBtn;
    // private Button _tipsBtn;
    public  Text _tipCorrectText;
    public Text _quesIndexText;
    public Text _quesContent;
  //  public Text _accuracyText; 
    private int _quesIndex = 0; 
    private int _quesCount;
    private int _rightCount;
    private int _answeredCount;
    public GameObject tip;
    public GameObject allSelect;
    public int trueCount;
    public Text scoreText;
    private void Start()
    {
        _nextTopicBtn.onClick.AddListener(NextTopic); 

          //点击下一个buttom触发监听，调用nexttopic函数

        for (int i = 0; i < 4; i++)
        {
            int count = i;
            SelecToggles[i].onValueChanged.AddListener(delegate (bool isOn)
            {
                if (isOn)
                    JudgeSelect(count);
            });

           //监听选项框是否被打开 并判断打开的这个选项是否正确

        }
        ReadQuestion();
        SetTopic();
        tip.SetActive(false);
        allSelect.SetActive(false); 
    }
    public void LoadUserSql()
    {
        OpenDB();
        // 查询数据
        reader = db.SelectWhere("Login",
            new string[] { "userName" },
            new string[] { "userName" },
            new string[] { "=" },
            new string[] {GameManager.Instance.currentUser});

            //Prevent from being empty

        if(reader.HasRows) 
        {
            //Update data if there is an original user record

            db.UpdateInto("Login", new string[] { "LoginTime" }, new string[] { GetStr(System.DateTime.Now) }, "userName",GetStr(GameManager.Instance.currentUser));
        }
        else
        {
            //If not, insert a new data

            db.InsertInto("Login", new string[] { GetStr(GameManager.Instance.currentUser), GetStr(System.DateTime.Now),GetStr(0) });
        }
        CloseDB();
    }
    public void UpdateScore()  //Update your grades
    {
        OpenDB();

        //Update Results

        db.UpdateInto("Login", new string[] { "Score" }, new string[] { GetStr(trueCount * 5) }, "userName", GetStr(GameManager.Instance.currentUser)); 
        CloseDB();
    }
    
    //Once you open this module to initialize the topic, etc.

    public void ResetTopic()
    {
        LoadUserSql(); 
        _quesIndex = 0;
        trueCount = 0;
        _nextTopicBtn.gameObject.SetActive(true); 
        ReadQuestion();
        SetTopic();
        tip.SetActive(false);
        allSelect.SetActive(false);
    }
    void NextTopic() 
    
    //判断做到第几题了

    {
        if(!IsAnswer[_quesIndex])
        {
            tip.SetActive(true); 
        }
        else
        {
            if (_quesIndex < 10)
            {
                if (_quesIndex == 0)
                {
                    _quesIndex++;
                    SetTopic();
                }
                else
                {
                    _quesIndex++;
                    SetTopic();
                }
                if (_quesIndex == 9)
                    _nextTopicBtn.gameObject.SetActive(false);
            }
        }
       
    }
       //Determine the grade right or wrong function
    void JudgeSelect(int selectIndex)  
    {
        int selectCount = 0;
        for (int i = 0; i < 4; i++)
        {
            if (i == selectIndex)
            {
                SelecToggles[i].isOn = true; //Open the options box in turn
                string xuanze=string.Empty;  //Assign a statically readable string
                string zhengqueDaan=string.Empty; 
                // Converts the selected index to letters and compares it with the options in the array
                switch(selectIndex) 
                {
                    case 0:
                        xuanze = "A";
                        break;
                    case 1:
                        xuanze = "B";
                        break;
                    case 2:
                        xuanze = "C";
                        break;
                    case 3:
                        xuanze = "D";
                        break;

                }
                if(_quesIndex<5) //Judgment for multiple-choice answers
                {
                   //Take out the correct answer under the array number, the correct answer is in the last place of the array,
                  // because the index of the array starts from 0, so you need to subtract 1
                   zhengqueDaan= _xuanzetiArray[_quesIndex][_xuanzetiArray[_quesIndex].Length - 1];
                }
                else
                {
                    //_quesIndex is the counter, judgment questions are the last five, so you need to take the remainder. 
                   //Take the correct answer under the array number, the correct answer is in the last place of the array, 
                   //because the index of the array starts from 0 so it needs to be subtracted by 1
                    zhengqueDaan = _panduantiArray[_quesIndex%5][_panduantiArray[_quesIndex%5].Length - 1];
                }
                if (zhengqueDaan.Equals(xuanze))
                {
                    _tipCorrectText.text = "<color=green>恭喜你，回答正确！请继续答题。</color>";
                    trueCount++;
                }
                else
                {
                    _tipCorrectText.text = string.Format("<color=red>对不起，回答错误！正确答案是{0}。</color>",zhengqueDaan);
                }

                selectCount = i;
            }
            else
                SelecToggles[i].isOn = false;//单选题选完其他的不能选
        }
        IsAnswer[_quesIndex] = true;
        for (int qt = 0; qt < SelecToggles.Count; qt++)
        {
            SelecToggles[qt].interactable = false;
        }
      if(_quesIndex==9) // display ui after selection
        {
            allSelect.SetActive(true);
            scoreText.text = string.Format("您的成绩是:{0}",(trueCount * 5).ToString()); 
        }
    }
    public void SetTopic()
    {

        // _tipsBtn.gameObject.SetActive(false);//初始隐藏提示按钮，错误时显示
        _tipCorrectText.text = "";
        for (int i = 0; i < 4; i++)
            SelecToggles[i].isOn = false;//开始时所有的选择默认为未选 
        int selectCount = 0;
        if (_quesIndex < 5)
        {
            _quesIndexText.text = "选择题：" + (_quesIndex + 1).ToString();  //Print the question number
            _quesContent.text = _xuanzetiArray[_quesIndex][0]; // print the question stem
            selectCount = _xuanzetiArray[_quesIndex].Length - 2;
        }
        else
        {
            _quesIndexText.text = "判断题：" + (_quesIndex + 1).ToString();
            _quesContent.text = _panduantiArray[_quesIndex%5][0];
            selectCount = _panduantiArray[_quesIndex%5].Length - 2; //确定selectcount的值
        }

        for (int i = 0; i < selectCount; i++)//打印 和选项个数有关
        {
            SelecToggles[i].gameObject.SetActive(true); //Activate the option box
            if (_quesIndex < 5)
                SelecAnswer[i].text = _xuanzetiArray[_quesIndex][i + 1]; //打印选项
            else
                SelecAnswer[i].text = _panduantiArray[_quesIndex%5][i + 1];
        }
        if (selectCount < SelecToggles.Count)//判断选项是否有3个，如果没有则隐藏多余的    
        {
            for (int i = selectCount; i < SelecToggles.Count; i++)
                SelecToggles[i].gameObject.SetActive(false);
        }
        if (!IsAnswer[_quesIndex])//可以勾选
        {
            for (int qt = 0; qt < selectCount; qt++)
            {
                SelecToggles[qt].interactable = true;
            }
        }
        else
        {
            for (int qt = 0; qt < selectCount; qt++)
            {
                SelecToggles[qt].interactable = false;
            }
        }
    }
    public void ReadQuestion()
    {
        ReadXuanzeti();
        ReadPanduanti();
        IsAnswer = new List<bool>();
        for (int i = 0; i <= TopicCount * 2; i++)
        {
            IsAnswer.Add(false);//根据题目数量添加记录值   
            SelectIsRight.Add(false);
            SelectAnswerIndex.Add(-1);
        }
    }
    public void ReadPanduanti()
    {
        _panduantiArray = new string[TopicCount][]; // New two-dimensional array
        List<int> saveRand = new List<int>();
        int randomNum;
        int num = 0;

        //一共有五个topic

        while (saveRand.Count < TopicCount) 
        {
            randomNum = Random.Range(1, 11);    //生成五个随机数
            if (!saveRand.Contains(randomNum))    //将五个题目逐个存进新数组
            {
                saveRand.Add(randomNum);
                _panduantiArray[num] = LoadPanDuanTi(randomNum);
                num++;
            }
            else continue;
        }
    }
    //Read all the topics in the database and put them into an array
    public string[] LoadPanDuanTi(int num)  
    {
        OpenDB();
        reader = db.Select("Pandu", "id", num.ToString());
        if (reader.HasRows)
        {
            reader.Read();
            string[] strs = new string[] {reader.GetString(reader.GetOrdinal("Timu")), reader.GetString(reader.GetOrdinal("XuanZe1")) ,
                reader.GetString(reader.GetOrdinal("XuanZe2")),reader.GetString(reader.GetOrdinal("Daan")) };
            CloseDB();
            return strs;
        }
        else
        {
            CloseDB();
            return null;
        }
    }
    public void ReadXuanzeti()
    {
        _xuanzetiArray = new string[TopicCount][];
        List<int> saveRand = new List<int>();
        int randomNum;
        int num = 0;
        while (saveRand.Count < TopicCount)
        {
            randomNum = Random.Range(1, 11);
            if (!saveRand.Contains(randomNum))
            {
                saveRand.Add(randomNum);
                _xuanzetiArray[num] = LoadXuanZeTi(randomNum);
                num++;
            }
            else continue;
        }
    }
    public string[] LoadXuanZeTi(int num)
    {
        OpenDB();
        reader = db.Select("XuanZe", "id", num.ToString());
        if (reader.HasRows)
        {
            reader.Read();
            string[] strs = new string[] { reader.GetString(reader.GetOrdinal("Timu")), reader.GetString(reader.GetOrdinal("XuanXiang1")),
                reader.GetString(reader.GetOrdinal("XuanXiang2")),reader.GetString(reader.GetOrdinal("XuanXiang3")),
                reader.GetString(reader.GetOrdinal("XuanXiang4")),reader.GetString(reader.GetOrdinal("Daan"))
            };
            Debug.Log(reader.GetString(reader.GetOrdinal("Timu")));
            Debug.Log(reader.GetString(reader.GetOrdinal("XuanXiang1")));
            Debug.Log(reader.GetString(reader.GetOrdinal("XuanXiang2")));
            Debug.Log(reader.GetString(reader.GetOrdinal("XuanXiang3")));
            Debug.Log(reader.GetString(reader.GetOrdinal("XuanXiang4")));
            Debug.Log(reader.GetString(reader.GetOrdinal("Daan")));
            CloseDB();
           
            return strs;
            //Debug.Log( reader.GetString( reader.GetOrdinal("Timu")));
            //Debug.Log(reader.GetString(reader.GetOrdinal("XuanXiang1")));
            //Debug.Log(reader.GetString(reader.GetOrdinal("XuanXiang2")));
            //Debug.Log(reader.GetString(reader.GetOrdinal("XuanXiang3")));
            //Debug.Log(reader.GetString(reader.GetOrdinal("XuanXiang4")));
            //Debug.Log(reader.GetString(reader.GetOrdinal("Daan"))); 
        }
        else { CloseDB(); return null; }

    }
}
