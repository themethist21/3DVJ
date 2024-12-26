using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class PlayerSelectController : MonoBehaviour
{
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;
    [SerializeField] private float changeSpeed;
    [SerializeField] private float initHeight;
    public TextMeshProUGUI playerText;

    private List<Transform> players;
    private List<string> playerNames;

    private bool changePlayer;
    private int currentPlayer;
    private int selectedPlayer;

    void Start()
    {
        currentPlayer = selectedPlayer = 0;
        changePlayer = false;
        players = new List<Transform>();
        playerNames = new List<string>();
        players.Add(player1);
        playerNames.Add("DOG");
        players.Add(player2);
        playerNames.Add("???");
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentPlayer == 0) ChangePlayer(players.Count - 1);
            else ChangePlayer(currentPlayer - 1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentPlayer == players.Count - 1) ChangePlayer(0);
            else ChangePlayer(currentPlayer + 1);
        }

        if (changePlayer && currentPlayer != selectedPlayer)
        {
            players[selectedPlayer].Translate(changeSpeed * Time.deltaTime * Vector3.up);
            players[currentPlayer].Translate(changeSpeed * Time.deltaTime * Vector3.down);
            if (players[selectedPlayer].position.y >= initHeight)
            {
                players[selectedPlayer].SetPositionAndRotation(new Vector3(players[selectedPlayer].position.x, initHeight, players[selectedPlayer].position.z), players[selectedPlayer].rotation);
                currentPlayer = selectedPlayer;
            }
        }
        else changePlayer = false;
    }

    public void ChangePlayer(int player)
    {
        changePlayer = true;
        selectedPlayer = player;
        playerText.text = playerNames[selectedPlayer];
    }
}
