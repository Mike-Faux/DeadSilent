using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface IInteractable
{
    // Start is called before the first frame update

    void Interact();
    void Interact(Player user);
    void Interact(EnemyAI user);

}
