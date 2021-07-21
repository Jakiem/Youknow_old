using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class Quest : MonoBehaviour {
	//Номер вопроса
	private int Id = 1;
	public Text Id_Text;
	//Номер ответа
	public int Ans = 0;
	//Правльные ответы
	private int R = 0;
	private int All_R;
	//Звуки
	public AudioSource L_Sound;
	public AudioSource W_Sound;
	//Булевые
	private bool Yes = true;
	private bool Pause = false;
	private bool Welcome = false;
	//Сообщения
	public Text Text_CP;
	public Text Score_CP;
	public Text Right_CP;
	public Text All_Right;
	public Text Text_Final;
	public Text Right_Final;
	//Время
	public float Taimer = 60;
	public Text Times;
	public bool Time_On = false;
	public Text Past_t;
	public int Past_Time;
	// Очки
	public Text Score_T;
	public int Score = 0;
	private int X = 0;
	private int Y = 0;
	public Text Get_S;

	//Текст вопроса
	public Text Text_Q;
	//Текст ответов
	public Text Text_A;
	public Text Text_B;
	public Text Text_C;
	public Text Text_D;

	//Массив вопросов
	public string[] List_Q;
	//Массив ответов A
	public string[] Ans_A;
	//Массив ответов B
	public string[] Ans_B;
	//Массив ответов C
	public string[] Ans_C;
	//Массив ответов D
	public string[] Ans_D;


	void Awake()
	{
		Time.timeScale = 1;
		//Загрузка вопроса
		Id = PlayerPrefs.GetInt ("Save_Q");
		//Загрузка правильных ответов
		R = PlayerPrefs.GetInt ("Save_R");

		if(Id == 0)
		{
			Id = 1;
		}
		if(Id == 101)
		{
			Id = 100;
		}
		if (Id == 1) 
		{
			GameObject.Find ("Canvas").transform.FindChild ("Welcome").gameObject.SetActive(true);
			Welcome = true;
		}
		if (Welcome == true) 
		{
			Time.timeScale = 0;
			Welcome = false;
		}

		//Загрузка очков
		Score = PlayerPrefs.GetInt ("Save_S");

		Text_Q = GameObject.Find ("Questions_Obj_1").transform.FindChild ("Quest_1").transform.GetComponent<Text> ();
		Text_A = GameObject.Find ("Questions_Obj_1").transform.FindChild ("A").transform.FindChild ("Text").transform.GetComponent<Text> ();
		Text_B = GameObject.Find ("Questions_Obj_1").transform.FindChild ("B").transform.FindChild ("Text").transform.GetComponent<Text> ();
		Text_C = GameObject.Find ("Questions_Obj_1").transform.FindChild ("C").transform.FindChild ("Text").transform.GetComponent<Text> ();
		Text_D = GameObject.Find ("Questions_Obj_1").transform.FindChild ("D").transform.FindChild ("Text").transform.GetComponent<Text> ();

		Times = GameObject.Find ("Canvas").transform.FindChild ("Bar").transform.FindChild ("Taimer").transform.GetComponent<Text> ();
		Score_T = GameObject.Find ("Canvas").transform.FindChild ("Bar").transform.FindChild ("Score").transform.GetComponent<Text> ();
		Id_Text = GameObject.Find ("Canvas").transform.FindChild ("Bar").transform.FindChild ("Number_Q").transform.GetComponent<Text> ();
		Past_t = GameObject.Find ("Canvas").transform.FindChild ("Dark_T_F").transform.FindChild ("Past_Time").transform.GetComponent<Text> ();
		Get_S = GameObject.Find ("Canvas").transform.FindChild ("Dark_T_F").transform.FindChild ("Get_Score").transform.GetComponent<Text> ();

		Text_CP = GameObject.Find ("Canvas").transform.FindChild ("Dark_Check_Point").transform.FindChild ("Field_Check_Point").transform.FindChild ("Text").transform.GetComponent<Text> ();
		Score_CP = GameObject.Find ("Canvas").transform.FindChild ("Dark_Check_Point").transform.FindChild ("Field_Check_Point").transform.FindChild ("Get_Score_CP").transform.GetComponent<Text> ();
		Right_CP = GameObject.Find ("Canvas").transform.FindChild ("Dark_Check_Point").transform.FindChild ("Field_Check_Point").transform.FindChild ("Right_A").transform.GetComponent<Text> ();
		All_Right = GameObject.Find ("Canvas").transform.FindChild ("Dark_Final").transform.FindChild ("Field_Final").transform.FindChild ("All_Right").transform.GetComponent<Text> ();
		Text_Final = GameObject.Find ("Canvas").transform.FindChild ("Dark_Final").transform.FindChild ("Field_Final").transform.FindChild ("Text").transform.GetComponent<Text> ();
		Right_Final = GameObject.Find ("Canvas").transform.FindChild ("Dark_Final").transform.FindChild ("Field_Final").transform.FindChild ("Right_A").transform.GetComponent<Text> ();


		L_Sound = GameObject.Find ("Audio Source").transform.FindChild ("Lose").transform.GetComponent<AudioSource> ();
		W_Sound = GameObject.Find ("Audio Source").transform.FindChild ("Win").transform.GetComponent<AudioSource> ();
	}
	void Update () 
	{
		//Вывод номера вопроса в бара
		Id_Text.text = "№ " + Id;
		//Старт Вопросов
		if (Id>0  && Yes == true) 
		{
			Invoke ("Quest_Q",0.05f);
			Yes = false;
		}

		#region Время и Очки
		//Время
		if (Time_On == true) 
		{
			Taimer -= 1*Time.deltaTime; 
		}
		//Вывод Времени и очков в бар
		Times.text = ""+(int)Taimer;
		Score_T.text = "Очки: "+Score;
		//Изменения цвета счетчика и подсчет фактических очков
		if (Taimer >= 30) 
		{
			Times.color = new Color (0,180,0);
			Y = ((int)Taimer*2) + X;
		}
		if (Taimer >= 15 && Taimer <=30) 
		{
			Times.color = new Color (30,230,0);
			Y = ((int)Taimer*2) + X;
		}
		if (Taimer >= 0 && Taimer <=15) 
		{
			Times.color = new Color (255,0,0);
			Y = ((int)Taimer*2) + X;
		}
		if (Taimer <= 0) 
		{
			Taimer = 0;
		}
		if(Score <=0)
		{
			Score = 0;
		}
		#endregion

		//Сохранение
		PlayerPrefs.SetInt("Save_Q", Id);
		//Сохранения правильных ответов в каждой стадии
		PlayerPrefs.SetInt ("Save_R", R);
		//Сохранения всех правильных ответов

	}

	#region Ответы
	public void A()
	{
		if (Ans == 1) 
		{
			Past_Time = (60-(int)Taimer);

			R++;
			All_R++;
			True ();
			Id++;
			Yes = true;
			//Сохранение правильных ответов
			PlayerPrefs.SetInt("Save_R",R);
			//Очки
			Score += Y;
			Taimer = 60;
			//Сохранение очков
			PlayerPrefs.SetInt("Save_S", Score);
			//Проигрываем звук победы
			W_Sound.audio.Play();
		}
		if (Ans == 2 || Ans == 3 || Ans == 4) 
		{
			Past_Time = (60-(int)Taimer);
			False ();

			Score -= 200;
			Id++;
			Yes = true;
			Taimer = 60;
			//Проигрываем звук поражения
			L_Sound.audio.Play();
		}
	}
	public void B()
	{
		if (Ans == 2) 
		{
			Past_Time = (60-(int)Taimer);

			R++;
			All_R++;
			True ();
			Id++;
			Yes = true;
			//Сохранение правильных ответов
			PlayerPrefs.SetInt("Save_R",R);
			//Очки
			Score += Y;
			Taimer = 60;
			//Сохранение очков
			PlayerPrefs.SetInt("Save_S", Score);
			//Проигрываем звук победы
			W_Sound.audio.Play();

		}
		if (Ans == 1 || Ans == 3 || Ans == 4) 
		{
			Past_Time = (60-(int)Taimer);
			False ();

			Score -= 200;
			Id++;
			Yes = true;
			Taimer = 60;
			//Проигрываем звук поражения
			L_Sound.audio.Play();

		}
	}
	public void C()
	{
		if (Ans == 3) 
		{
			Past_Time = (60-(int)Taimer);

			R++;
			All_R++;
			True ();
			Id++;
			Yes = true;
			//Сохранение правильных ответов
			PlayerPrefs.SetInt("Save_R",R);
			//Очки
			Score += Y;
			Taimer = 60;
			//Сохранение очков
			PlayerPrefs.SetInt("Save_S", Score);
			//Проигрываем звук победы
			W_Sound.audio.Play();

		}
		if (Ans == 1 || Ans == 2 || Ans == 4) 
		{
			Past_Time = (60-(int)Taimer);
			False ();

			Score -= 200;
			Id++;
			Yes = true;
			Taimer = 60;
			//Проигрываем звук поражения
			L_Sound.audio.Play();
		}
	}
	public void D()
	{
		if (Ans == 4) 
		{
			Past_Time = (60-(int)Taimer);
	
			R++;
			All_R++;
			True ();
			Id++;
			Yes = true;
			//Сохранение правильных ответов
			PlayerPrefs.SetInt("Save_R",R);
			//Очки
			Score += Y;
			Taimer = 60;
			//Сохранение очков
			PlayerPrefs.SetInt("Save_S", Score);
			//Проигрываем звук победы
			W_Sound.audio.Play();
		}
		if (Ans == 1 || Ans == 2 || Ans == 3) 
		{
			Past_Time = (60-(int)Taimer);
			False ();

			Score -= 200;
			Id++;
			Yes = true;
			Taimer = 60;
			//Проигрываем звук поражения
			L_Sound.audio.Play();
		}
	}
	#endregion

	#region Верный и не верный ответы
	void True ()
	{
		Time.timeScale = 0;
		if (Id != 10 && Id != 20 && Id != 30 && Id != 40 && Id != 50 && Id != 60 && Id != 70 && Id != 90 && Id != 100) 
		{
			GameObject.Find ("Canvas").transform.FindChild ("Dark_T_F").gameObject.SetActive (true);
			GameObject.Find ("Canvas").transform.FindChild ("Dark_T_F").transform.FindChild ("True").gameObject.SetActive (true);
			Past_t.text = "Прошло времени:  " + Past_Time;
			Get_S.text = "Очки + " + Y;
		}
		if (Id == 10 || Id == 20 || Id == 30 || Id == 40 || Id == 50 || Id == 60 || Id == 70 || Id == 80 || Id == 90) 
		{
			GameObject.Find ("Canvas").transform.FindChild ("Dark_Check_Point").gameObject.SetActive (true);
			Score_CP.text = "Очки + " + Y;
			if(R >=5)
			{
				Text_CP.text = "Вы молодец, хорошо спраляетесь! Сейчас вам будет показана реклама, спасибо!";
			}
			if(R <=4)
			{
				Text_CP.text = "Не очень хорошо, старайтесь лучше! Но все же, сейчас вам будет показана реклама, спасибо!";
			}
			Right_CP.text = "Правильных ответов: "+ R;
			R = 0;
		}
		if (Id == 100) 
		{
			GameObject.Find ("Canvas").transform.FindChild ("Dark_Final").gameObject.SetActive (true);
			Text_Final.text = "Поздравляю с завершением игры! Надеюсь вам понравилось и вы захотите пройти игру ещё раз, удачи!";
			Right_Final.text = "Правильных ответов: "+ R;
			All_Right.text = "Всего правильных ответов: "+ All_R;
			R = 0;
		}
	}
	
	void False ()
	{
		Time.timeScale = 0;
		if (Id != 10 && Id != 20 && Id != 30 && Id != 40 && Id != 50 && Id != 60 && Id != 70 && Id != 90 && Id != 100)  
		{
			GameObject.Find ("Canvas").transform.FindChild ("Dark_T_F").gameObject.SetActive (true);
			GameObject.Find ("Canvas").transform.FindChild ("Dark_T_F").transform.FindChild ("False").gameObject.SetActive (true);
			Past_t.text = "Прошло времени:  " + Past_Time;
			Get_S.text = "Очки: - " + (200);
		}
		if (Id == 10 || Id == 20 || Id == 30 || Id == 40 || Id == 50 || Id == 60 || Id == 70 || Id == 80 || Id == 90) 
		{
			GameObject.Find ("Canvas").transform.FindChild ("Dark_Check_Point").gameObject.SetActive (true);
			Score_CP.text = "Очки + " + Y;
			if(R >=5)
			{
				Text_CP.text = "Вы молодец, хорошо спраляетесь! Сейчас вам будет показана реклама, спасибо!";
			}
			if(R <=4)
			{
				Text_CP.text = "Не очень хорошо, старайтесь лучше! Но все же, сейчас вам будет показана реклама, спасибо!";
			}
			Right_CP.text = "Правильных ответов: "+ R;
			R = 0;
		}
		if (Id == 100) 
		{
			GameObject.Find ("Canvas").transform.FindChild ("Dark_Final").gameObject.SetActive (true);
			Text_Final.text = "Поздравляю с завершением игры! Надеюсь вам понравилось и вы захотите пройти игру ещё раз, удачи!";
			Right_Final.text = "Правильных ответов: "+ R;
			All_Right.text = "Всего правильных ответов: "+ All_R;
			R = 0;
		}
	}
	#endregion

	#region Пауза
	public void Pause_On()
	{
		Time.timeScale = 0;
	}
	public void Pause_Off()
	{
		Time.timeScale = 1;
	}
	#endregion

	#region Вопросы
	void Quest_Q()
	{
		if (Id == 1) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 0;
		}
		if (Id == 2) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 5;
		}
		if (Id == 3) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 4) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 3;
			Time_On = true;
			X = 5;
		}
		if (Id == 5) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 1;
			Time_On = true;
			X = 5;
		}
		if (Id == 6) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 7) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 5;
		}
		if (Id == 8) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 3;
			Time_On = true;
			X = 5;
		}
		if (Id == 9) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 1;
			Time_On = true;
			X = 5;
		}
		if (Id == 10) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 1;
			Time_On = true;
			X = 5;
		}
		if (Id == 11) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 3;
			Time_On = true;
			X = 5;
		}
		if (Id == 12) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 5;
		}
		if (Id == 13) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 1;
			Time_On = true;
			X = 5;
		}
		if (Id == 14) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 15) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 3;
			Time_On = true;
			X = 5;
		}
		if (Id == 16) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 17) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 5;
		}
		if (Id == 18) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 3;
			Time_On = true;
			X = 5;
		}
		if (Id == 19) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 1;
			Time_On = true;
			X = 5;
		}
		if (Id == 20) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 3;
			Time_On = true;
			X = 5;
		}
		if (Id == 21) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 22) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 5;
		}
		if (Id == 23) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 1;
			Time_On = true;
			X = 5;
		}
		if (Id == 24) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 25) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 3;
			Time_On = true;
			X = 5;
		}
		if (Id == 26) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 27) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 1;
			Time_On = true;
			X = 5;
		}
		if (Id == 28) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 5;
		}
		if (Id == 29) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 30) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 1;
			Time_On = true;
			X = 5;
		}
		if (Id == 31) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 3;
			Time_On = true;
			X = 5;
		}
		if (Id == 32) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 33) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 3;
			Time_On = true;
			X = 5;
		}
		if (Id == 34) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 1;
			Time_On = true;
			X = 5;
		}
		if (Id == 35) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 3;
			Time_On = true;
			X = 5;
		}
		if (Id == 36) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 37) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 5;
		}
		if (Id == 38) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 39) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 5;
		}
		if (Id == 40) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 5;
		}
		if (Id == 41) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 42) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 5;
		}
		if (Id == 43) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 1;
			Time_On = true;
			X = 5;
		}
		if (Id == 44) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 1;
			Time_On = true;
			X = 5;
		}
		if (Id == 45) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 46) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 5;
		}
		if (Id == 47) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 48) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 5;
		}
		if (Id == 49) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 50) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 51) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 5;
		}
		if (Id == 52) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 5;
		}
		if (Id == 53) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 1;
			Time_On = true;
			X = 5;
		}
		if (Id == 54) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 55) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 56) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 1;
			Time_On = true;
			X = 5;
		}
		if (Id == 57) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 5;
		}
		if (Id == 58) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 59) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 3;
			Time_On = true;
			X = 5;
		}
		if (Id == 60) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 5;
		}
		if (Id == 61) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 3;
			Time_On = true;
			X = 5;
		}
		if (Id == 62) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 1;
			Time_On = true;
			X = 5;
		}
		if (Id == 63) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 3;
			Time_On = true;
			X = 5;
		}
		if (Id == 64) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 5;
		}
		if (Id == 65) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 1;
			Time_On = true;
			X = 5;
		}
		if (Id == 66) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 67) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 3;
			Time_On = true;
			X = 5;
		}
		if (Id == 68) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 69) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 1;
			Time_On = true;
			X = 5;
		}
		if (Id == 70) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 71) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 5;
		}
		if (Id == 72) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 1;
			Time_On = true;
			X = 5;
		}
		if (Id == 73) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 74) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 3;
			Time_On = true;
			X = 5;
		}
		if (Id == 75) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 3;
			Time_On = true;
			X = 5;
		}
		if (Id == 76) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 3;
			Time_On = true;
			X = 5;
		}
		if (Id == 77) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 3;
			Time_On = true;
			X = 5;
		}
		if (Id == 78) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 1;
			Time_On = true;
			X = 5;
		}
		if (Id == 79) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 3;
			Time_On = true;
			X = 5;
		}
		if (Id == 80) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 5;
		}
		if (Id == 81) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 82) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 5;
		}
		if (Id == 83) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 5;
		}
		if (Id == 84) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 1;
			Time_On = true;
			X = 5;
		}
		if (Id == 85) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 3;
			Time_On = true;
			X = 5;
		}
		if (Id == 86) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 5;
		}
		if (Id == 87) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 88) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 3;
			Time_On = true;
			X = 5;
		}
		if (Id == 89) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 1;
			Time_On = true;
			X = 5;
		}
		if (Id == 90) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 91) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 92) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 5;
		}
		if (Id == 93) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 3;
			Time_On = true;
			X = 5;
		}
		if (Id == 94) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 4;
			Time_On = true;
			X = 5;
		}
		if (Id == 95) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 1;
			Time_On = true;
			X = 5;
		}
		if (Id == 96) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 1;
			Time_On = true;
			X = 5;
		}
		if (Id == 97) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 5;
		}
		if (Id == 98) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 1;
			Time_On = true;
			X = 5;
		}
		if (Id == 99) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 1;
			Time_On = true;
			X = 5;
		}
		if (Id == 100) 
		{
			Text_Q.text = List_Q[Id-1];
			Text_A.text = Ans_A[Id-1];
			Text_B.text = Ans_B[Id-1];
			Text_C.text = Ans_C[Id-1];
			Text_D.text = Ans_D[Id-1];
			Ans = 2;
			Time_On = true;
			X = 5;
		}
	}
	#endregion
}
