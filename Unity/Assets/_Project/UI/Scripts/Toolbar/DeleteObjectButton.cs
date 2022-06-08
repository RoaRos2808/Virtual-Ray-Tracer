using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Project.Ray_Tracer.Scripts;

public class DeleteObjectButton : MonoBehaviour
{
    public RTSceneManager rTSceneManager;


    public void DeleteSelectedObject()
    {
        rTSceneManager.DeleteSelected();
    }
}
