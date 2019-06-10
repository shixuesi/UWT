using System.Collections;
using System.Collections.Generic;
//using UnityEngine;

public class SwiperController : IndexController
{
    // Autoplay is started
    public const int START_AUTOPLAY = 2;

    // Autoplay is stopped.
    public const int STOP_AUTOPLAY = 3;

    // Indicate that the user is swiping
    public const int SWIPE = 4;

    // Indicate that the `Swiper` has changed it's index and is building it's ui ,so that the
    // `SwiperPluginConfig` is available.
    public const int BUILD = 5;

    // available when `event` == SwiperController.BUILD
    SwiperPluginConfig config;

    // available when `event` == SwiperController.SWIPE
    // this value is PageViewController.pos
    float pos;

    new int index;
    new bool animation;
    public bool autoplay;

    public SwiperController()
    {
    }

    void startAutoplay()
    {
        Event = SwiperController.START_AUTOPLAY;
        this.autoplay = true;
        notifyListeners();
    }

    void stopAutoplay()
    {
        Event = SwiperController.STOP_AUTOPLAY;
        this.autoplay = false;
        notifyListeners();
    }
}
