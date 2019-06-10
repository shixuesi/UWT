using System.Collections;
using System.Collections.Generic;
using Unity.UIWidgets.foundation;
using UnityEngine;

public class IndexController : ChangeNotifier
{
    public const int NEXT = 1;
    public const int PREVIOUS = -1;
    public const int MOVE = 0;

    ////Completer _completer;

    public int index;

    public bool animation;

    public int Event;

    public void move(int index, bool animation = true)
    {
        this.animation = animation;
        this.index = index;
        this.Event = MOVE;
        //_completer = new Completer();
        notifyListeners();
        //return _completer.future;
    }
    //RSG.IPromise
    public void next(bool animation = true)
    {
        this.Event = NEXT;
        this.animation = animation;
        //_completer = new Completer();
        notifyListeners();
        //return _completer.future;
    }

    public void previous(bool animation = true)
    {
        this.Event = PREVIOUS;
        this.animation = animation;
        //_completer = new Completer();
        notifyListeners();
        // return _completer.future;
    }

    public void complete()
    {
        //if (!_completer.isCompleted)
        //{
        //    _completer.complete();
        //}
    }
}
