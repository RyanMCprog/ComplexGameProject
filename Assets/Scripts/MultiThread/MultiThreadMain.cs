using System.Collections;
using System.Collections.Generic;

using System.Threading;
using UnityEngine;


public class MultiThreadMain : MonoBehaviour
{

    class ThreadInfo
    {
        public int ThreadId;
        public MainThreadQueue MainThreadQueue;
        public int CreateCount;
    }

    MainThreadQueue mainThreadQueue;

    // Start is called before the first frame update
    void Start()
    {
        // Create the queue to do work on the main thread
        mainThreadQueue = new MainThreadQueue();

        // Start some threads
        var threadStart = new ParameterizedThreadStart(OtherThread);
        for (int i = 0; i < 4; ++i)
        {
            var threadInfo = new ThreadInfo();
            threadInfo.ThreadId = i;
            threadInfo.MainThreadQueue = mainThreadQueue;
            threadInfo.CreateCount = Random.Range(5, 20);
            var thread = new Thread(threadStart);
            thread.Start(threadInfo);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Execute commands for up to 5 milliseconds
        mainThreadQueue.Execute(5);
    }

    static void OtherThread(object startParam)
    {
        // Create game objects and set their transforms' positions
        ThreadInfo threadInfo = (ThreadInfo)startParam;
        var mainThreadQueue = threadInfo.MainThreadQueue;
        var newGameObjectResult = new MainThreadQueue.Result<GameObject>();
        var getTransformResult = new MainThreadQueue.Result<Transform>();
        var setPositionResult = new MainThreadQueue.Result();
        for (var i = 0; i < threadInfo.CreateCount; ++i)
        {
            // New game object
            var name = "From Thread " + threadInfo.ThreadId;
            mainThreadQueue.NewGameObject(name, newGameObjectResult);
            var go = newGameObjectResult.Value;

            // Get game object's transform
            mainThreadQueue.GetTransform(go, getTransformResult);
            var transform = getTransformResult.Value;

            // Set transform's position
            var pos = new Vector3(i, i, i);
            mainThreadQueue.SetPosition(transform, pos, setPositionResult);
            setPositionResult.Wait();
        }
    }
}
