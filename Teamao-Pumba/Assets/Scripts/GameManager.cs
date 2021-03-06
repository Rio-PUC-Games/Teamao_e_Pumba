using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*Script para gerenciamento do jogo 

Author: Vinny
*/
public class GameManager : MonoBehaviour
{
    public GameObject Players; // Os jogadores
    public GameObject Bases; // As bases
    public GameObject Espinhos; // Espinhos na Arena
    public GameObject VictoryCanvas; // O canvas de fim de jogo
    public GameObject PointsCanvas; // O canvas de Pontos
    public GameObject AboveHeadCanvas; // O canvas em cima do Player
    public Text ResultText; // Texto do resultado da partida
    public Text ErrorText; // Um texto de erro caso o jogo comece sem escolher a quantidade de jogadores
    public Text CountdownTimer; // Countdown antes de comecar o jogo
    public Text Timer; // Timer do Jogo
    public Image TimerCircle; // Imagem do circulo
    public List<Text> ValueText;
    static public int DefaultTempo = 90;
    static public float DefaultProbabilidade = 30;
    static public int DefaultPontodeVitoria = 30;
    private bool PlayersSelected;
    private bool CountdownAcabou;
    private int VictoryByPoint = 999;
    [HideInInspector]
    public float tempo = 999;
    [HideInInspector]
    public float Countdown = 4;
    private float movespeed;
    private float MaxTimer = 999;
    private bool movizin = true;
    public GameSettings gameSettings;

    public List<Sprite> telaVitoriaJogador1 = new List<Sprite>();
    public List<Sprite> telaVitoriaJogador2 = new List<Sprite>();
    public List<Sprite> telaVitoriaJogador3 = new List<Sprite>();
    public List<Sprite> telaVitoriaJogador4 = new List<Sprite>();
    public GameObject telaVitoria;

    public AudioSource Win;
    public AudioSource MainMusic;

    public Sprite spriteEmpate;


    public void PlayGame()
    { // Começa o jogo



    }

    void Start()
    {
        PointsCanvas.SetActive(false);
        CountdownAcabou = false;
        PlayersSelected = false;
        VictoryCanvas.SetActive(false);
        CountdownTimer.gameObject.transform.parent.gameObject.SetActive(false);
        movespeed = Players.transform.GetChild(0).transform.GetChild(0).GetComponent<MovimentAxis>().movementSpeed;
        for (int i = 0; i < 4; i++)
        {
            Players.transform.GetChild(i).GetComponent<CharacterSelect>().SetCharacter(gameSettings.getPlayerChoice(i));
            PointsCanvas.transform.GetChild(i).gameObject.SetActive(false);
            Bases.transform.GetChild(i).gameObject.SetActive(false);
            AboveHeadCanvas.transform.GetChild(i).gameObject.SetActive(false);
        }
        Players.transform.GetChild(0).gameObject.SetActive(false);
        Players.transform.GetChild(1).gameObject.SetActive(false);
        Players.transform.GetChild(2).gameObject.SetActive(false);
        Players.transform.GetChild(3).gameObject.SetActive(false);

        TempoDeJogo(DefaultTempo);
        SetVictoryByPoint(DefaultPontodeVitoria);
        gameObject.GetComponent<RandomEvent>().ChanceDeEvento(DefaultProbabilidade);
        CountdownTimer.gameObject.transform.parent.gameObject.SetActive(true);
        PointsCanvas.SetActive(true);
        print(gameSettings.playerNumbers);
        for (int i = 0; i < gameSettings.playerNumbers; i++)
        { //ativa componentes dos jogadores existentes
            //Debug.log(gameSettings.playerNumbers);
            Players.transform.GetChild(i).gameObject.SetActive(true);
            PointsCanvas.transform.GetChild(i).gameObject.SetActive(true);
            Bases.transform.GetChild(i).gameObject.SetActive(true);
            AboveHeadCanvas.transform.GetChild(i).gameObject.SetActive(true);
            //SetCharacterImage();
            // ResetaCoordenada();
        }
        CountdownTimer.text = "-";
    }
    public void TempoDeJogo(int Segundos)
    {
        tempo = Segundos;
        MaxTimer = Segundos;
        DefaultTempo = Segundos;
        ErrorText.text = "";
    }
    public void SetVictoryByPoint(int Pontos)
    {
        VictoryByPoint = Pontos;
        DefaultPontodeVitoria = Pontos;
        ErrorText.text = "";
    }
    void Update()
    {
        CheckCurrentValues();
        if (CountdownAcabou && tempo > 0)
        {
            if (tempo != 999999)
            {
                tempo -= Time.deltaTime;
                TimerCircle.fillAmount = tempo / MaxTimer;
                if (MaxTimer / tempo >= 3)
                {
                    //SetEspinhos();
                }
            }
            if (movizin)
            {
                for (int i = 0; i < 4; i++)
                {
                    /*Players.transform.GetChild(0).transform.GetChild(i).GetComponent<MovimentAxis>().movementSpeed = movespeed;
                    Players.transform.GetChild(1).transform.GetChild(i).GetComponent<MovimentAxis>().movementSpeed = movespeed;
                    Players.transform.GetChild(2).transform.GetChild(i).GetComponent<MovimentAxis>().movementSpeed = movespeed;
                    Players.transform.GetChild(3).transform.GetChild(i).GetComponent<MovimentAxis>().movementSpeed = movespeed;*/
                }
                movizin = false;
            }
        }
        for (int i = 0; i < 4; i++)
        { // Verifica o fim do jogo
            if (Players.transform.GetChild(i).GetComponent<PointSystemPai>().RealPoints >= VictoryByPoint + SegundoMelhor() || tempo < 0)
            {
                StartCoroutine(ShowVictoryCanvas());
                CountdownTimer.gameObject.SetActive(true);
                CountdownTimer.text = "Finish!";

                ShowVictoryScreen();
            }
        }
        ShowPoints();
    }

    private void ShowVictoryScreen()
    {
        if (!empate())
        {
            if (MaiorValor() == "Player1")
            {
                if (gameSettings.getPlayerChoice(0) == "tucano")
                {

                    telaVitoria.GetComponent<Image>().sprite = telaVitoriaJogador1[0];
                }
                if (gameSettings.getPlayerChoice(0) == "capivara")
                {

                    telaVitoria.GetComponent<Image>().sprite = telaVitoriaJogador1[1];

                }
                if (gameSettings.getPlayerChoice(0) == "lico")
                {

                    telaVitoria.GetComponent<Image>().sprite = telaVitoriaJogador1[2];
                }
                if (gameSettings.getPlayerChoice(0) == "jess")
                {

                    telaVitoria.GetComponent<Image>().sprite = telaVitoriaJogador1[3];
                }
            }
            if (MaiorValor() == "Player2")
            {
                if (gameSettings.getPlayerChoice(1) == "tucano")
                {

                    telaVitoria.GetComponent<Image>().sprite = telaVitoriaJogador2[0];

                }
                if (gameSettings.getPlayerChoice(1) == "capivara")
                {

                    telaVitoria.GetComponent<Image>().sprite = telaVitoriaJogador2[1];

                }
                if (gameSettings.getPlayerChoice(1) == "lico")
                {

                    telaVitoria.GetComponent<Image>().sprite = telaVitoriaJogador2[2];

                }
                if (gameSettings.getPlayerChoice(1) == "jess")
                {

                    telaVitoria.GetComponent<Image>().sprite = telaVitoriaJogador2[3];

                }
            }
            if (MaiorValor() == "Player3")
            {
                if (gameSettings.getPlayerChoice(2) == "tucano")
                {

                    telaVitoria.GetComponent<Image>().sprite = telaVitoriaJogador3[0];

                }
                if (gameSettings.getPlayerChoice(2) == "capivara")
                {

                    telaVitoria.GetComponent<Image>().sprite = telaVitoriaJogador3[1];

                }
                if (gameSettings.getPlayerChoice(2) == "lico")
                {

                    telaVitoria.GetComponent<Image>().sprite = telaVitoriaJogador3[2];

                }
                if (gameSettings.getPlayerChoice(2) == "jess")
                {

                    telaVitoria.GetComponent<Image>().sprite = telaVitoriaJogador3[3];

                }
            }
            if (MaiorValor() == "Player4")
            {
                if (gameSettings.getPlayerChoice(3) == "tucano")
                {

                    telaVitoria.GetComponent<Image>().sprite = telaVitoriaJogador4[0];

                }
                if (gameSettings.getPlayerChoice(3) == "capivara")
                {

                    telaVitoria.GetComponent<Image>().sprite = telaVitoriaJogador4[1];

                }
                if (gameSettings.getPlayerChoice(3) == "lico")
                {

                    telaVitoria.GetComponent<Image>().sprite = telaVitoriaJogador4[2];

                }
                if (gameSettings.getPlayerChoice(3) == "jess")
                {

                    telaVitoria.GetComponent<Image>().sprite = telaVitoriaJogador4[3];

                }
            }
        }
        else
        {
            telaVitoria.GetComponent<Image>().sprite = spriteEmpate;
        }

    }

    void FixedUpdate()
    {
        string oldText = CountdownTimer.text;

        CountdownTimer.gameObject.SetActive(true);
        Countdown -= Time.deltaTime;

        string newText = Mathf.RoundToInt((Countdown - 1)).ToString();

        if (oldText != newText)
        {
            CountdownTimer.text = newText;
            if (!CountdownAcabou)
            {
                for (int i = 0; i < 4; i++)
                {
                    Players.transform.GetChild(0).transform.GetChild(i).GetComponent<MovimentAxis>().movementSpeed = 0;
                    Players.transform.GetChild(1).transform.GetChild(i).GetComponent<MovimentAxis>().movementSpeed = 0;
                    Players.transform.GetChild(2).transform.GetChild(i).GetComponent<MovimentAxis>().movementSpeed = 0;
                    Players.transform.GetChild(3).transform.GetChild(i).GetComponent<MovimentAxis>().movementSpeed = 0;
                }

                // A Q U I   H O M S I
                //passou um segundo
                print("PASSOU");
                print(newText);
            }
        }


        if (Countdown - 1 <= 1 && tempo > 0)
        {
            for (int i = 0; i < 4; i++)
            {
                Players.transform.GetChild(0).transform.GetChild(i).GetComponent<MovimentAxis>().movementSpeed = movespeed;
                Players.transform.GetChild(1).transform.GetChild(i).GetComponent<MovimentAxis>().movementSpeed = movespeed;
                Players.transform.GetChild(2).transform.GetChild(i).GetComponent<MovimentAxis>().movementSpeed = movespeed;
                Players.transform.GetChild(3).transform.GetChild(i).GetComponent<MovimentAxis>().movementSpeed = movespeed;
            }
            CountdownTimer.text = "Start!";
            CountdownAcabou = true;
            Timer.gameObject.SetActive(true);
            if (tempo != 999999)
            {
                Timer.text = Mathf.RoundToInt(tempo).ToString();
            }
            else
            {
                Timer.text = "∞";
            }
        }
        if (Countdown - 1 <= 0 && tempo > 0)
        {
            CountdownTimer.gameObject.SetActive(false);
        }

    }

    private string MaiorValor()
    { // Pega o Melhor jogador e devolve seu nome
        string nome = "";
        int maior = 0;
        if (Players.transform.GetChild(0).GetComponent<PointSystemPai>().RealPoints > maior)
        {
            maior = Players.transform.GetChild(0).GetComponent<PointSystemPai>().RealPoints;
            nome = "Player1";
        }
        if (Players.transform.GetChild(1).GetComponent<PointSystemPai>().RealPoints > maior)
        {
            maior = Players.transform.GetChild(1).GetComponent<PointSystemPai>().RealPoints;
            nome = "Player2";
        }
        if (Players.transform.GetChild(2).GetComponent<PointSystemPai>().RealPoints > maior)
        {
            maior = Players.transform.GetChild(2).GetComponent<PointSystemPai>().RealPoints;
            nome = "Player3";
        }
        if (Players.transform.GetChild(3).GetComponent<PointSystemPai>().RealPoints > maior)
        {
            maior = Players.transform.GetChild(3).GetComponent<PointSystemPai>().RealPoints;
            nome = "Player4";
        }
        Debug.Log(nome);
        return nome;
    }
    private int SegundoMelhor()
    { // Pega o Segundo melhor jogador
        int sm = 0;
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < 4; i++)
        {
            list.Add(Players.transform.GetChild(i).gameObject);
        }
        foreach (GameObject g in list)
        {
            if (g.GetComponent<PointSystemPai>().RealPoints > sm)
            {
                sm = g.GetComponent<PointSystemPai>().RealPoints;
            }
        }
        foreach (GameObject g in list)
        {
            if (g.GetComponent<PointSystemPai>().RealPoints == sm)
            {
                list.Remove(g);
                sm = 0;
                break;
            }
        }
        foreach (GameObject g in list)
        {
            if (g.GetComponent<PointSystemPai>().RealPoints > sm)
            {
                sm = g.GetComponent<PointSystemPai>().RealPoints;
            }
        }
        return sm;
    }
    private bool empate()
    { // Verifica se houve empate no final da partida
        int sm = 0;
        List<GameObject> list = new List<GameObject>();
        list.Add(Players.transform.GetChild(0).gameObject);
        list.Add(Players.transform.GetChild(1).gameObject);
        list.Add(Players.transform.GetChild(2).gameObject);
        list.Add(Players.transform.GetChild(3).gameObject);
        foreach (GameObject g in list)
        {
            if (g.GetComponent<PointSystemPai>().RealPoints > sm)
            {
                sm = g.GetComponent<PointSystemPai>().RealPoints;
            }
        }
        if (sm == SegundoMelhor())
        {
            return true;
        }
        return false;
    }
    private void ShowPoints()
    {
        for (int i = 0; i < 4; i++)
        {
            PointsCanvas.transform.GetChild(i).transform.GetChild(5).GetComponent<Text>().text = Players.transform.GetChild(i).GetComponent<PointSystemPai>().RealPoints.ToString();

            PointsCanvas.transform.GetChild(i).transform.GetChild(6).GetComponent<Image>().fillAmount = Players.transform.GetChild(i).GetComponent<PointSystemPai>().RealPoints / 100f;

            AboveHeadCanvas.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = Players.transform.GetChild(i).GetComponent<PointSystemPai>().VirtualPoints.ToString() + "/" + Players.transform.GetChild(i).GetComponent<PointSystemPai>().MaxItem;

            for (int j = 0; j < 6; j++)
            {
                AboveHeadCanvas.transform.GetChild(i).transform.GetChild(0).transform.GetChild(3).transform.GetChild(j).gameObject.SetActive(false);
            }
            int points = Players.transform.GetChild(i).GetComponent<PointSystemPai>().VirtualPoints;
            AboveHeadCanvas.transform.GetChild(i).transform.GetChild(0).transform.GetChild(3).transform.GetChild(points).gameObject.SetActive(true);

            if (Players.transform.GetChild(i).GetComponent<PointSystemPai>().VirtualPoints == Players.transform.GetChild(i).GetComponent<PointSystemPai>().MaxItem)
            {
                //CarryCanvas.transform.GetChild(i+4).gameObject.SetActive(true);
            }
            else
            {
                //CarryCanvas.transform.GetChild(i+4).gameObject.SetActive(false);
            }
        }
    }

    IEnumerator ShowVictoryCanvas()
    {
        yield return new WaitForSeconds(1);
        VictoryCanvas.SetActive(true);
        MainMusic.Pause();
        Win.Play();
        Time.timeScale = 0f;
    }

    private void SetCharacterImage()
    {
        int indice1 = 0;
        int indice2 = 0;
        int indice3 = 0;
        int indice4 = 0;
        int i;
        for (i = 0; i < 4; i++)
        {
            if (Players.transform.GetChild(0).transform.GetChild(i).gameObject.activeSelf)
            {
                indice1 = i;
            }
        }
        for (i = 0; i < 4; i++)
        {
            if (Players.transform.GetChild(1).transform.GetChild(i).gameObject.activeSelf)
            {
                indice2 = i;
            }
        }
        for (i = 0; i < 4; i++)
        {
            if (Players.transform.GetChild(2).transform.GetChild(i).gameObject.activeSelf)
            {
                indice3 = i;
            }
        }
        for (i = 0; i < 4; i++)
        {
            if (Players.transform.GetChild(3).transform.GetChild(i).gameObject.activeSelf)
            {
                indice4 = i;
            }
        }
        PointsCanvas.transform.GetChild(0).transform.GetChild(indice1 + 1).gameObject.SetActive(true);
        PointsCanvas.transform.GetChild(1).transform.GetChild(indice2 + 1).gameObject.SetActive(true);
        PointsCanvas.transform.GetChild(2).transform.GetChild(indice3 + 1).gameObject.SetActive(true);
        PointsCanvas.transform.GetChild(3).transform.GetChild(indice4 + 1).gameObject.SetActive(true);

    }
    private void ResetaCoordenada()
    {
        for (int i = 0; i < transform.GetComponent<ButtonSelect>().CoordenadaPlayers.Count; i++)
        {
            transform.GetComponent<ButtonSelect>().CoordenadaPlayers[i] = 0;
        }
    }
    private void SetEspinhos()
    {
        for (int i = 0; i < 4; i++)
        {
            Espinhos.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    private void CheckCurrentValues()
    {
        ValueText[0].text = DefaultTempo.ToString();
        ValueText[1].text = DefaultProbabilidade.ToString();
        ValueText[2].text = DefaultPontodeVitoria.ToString() + " Pontos";
        if (DefaultTempo == 999999)
        {
            ValueText[0].text = "∞";
        }
        if (DefaultProbabilidade == 0)
        {
            ValueText[1].text = "Nenhuma";
        }
        if (DefaultPontodeVitoria == 999999)
        {
            ValueText[2].text = "Desligado";
        }
    }

}