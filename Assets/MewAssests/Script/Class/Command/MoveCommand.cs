using NUnit.Framework;
using UnityEngine;

public class MoveCommand : ICommand
{
    private Controller controller;
    private PlayerController player;
    private EnemyController enemy;
    public MoveCommand(Controller controller)
    {
        switch (controller)
        {
            case PlayerController:
                PlayerController playerController = (PlayerController)controller;
                player = playerController;
                this.controller = player;
                break;
            case EnemyController:
                EnemyController enemyController = (EnemyController)controller; 
                enemy = enemyController;
                this.controller = enemy;
                break;
        }
    }

    public void Excute()
    {
        switch (controller)
        {
            case PlayerController:
                player.Move();
                break;
            case EnemyController:
                enemy.Move();
                break;
        }
    }
}
