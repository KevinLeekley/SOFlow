# SOFlow

--- *Original concept credited to [Ryan Hipple](https://www.youtube.com/watch?v=raQ3iHhE_Kk&t=2s)* --- 

# Releases

[SOFlow v1](https://github.com/BLUDRAG/SOFlow/releases/tag/v1)

# Requirements

Any Unity version supporting API compatibility version .NET 4.x

# Summary

The purpose of this SDK is to help bolster production of games and applications built within Unity by
taking advantage of `ScriptableObjects`. As explained in [Ryan Hipple's](https://www.youtube.com/watch?v=raQ3iHhE_Kk&t=2s) talk, `ScriptableObjects` have great potential to decouple components from
scenes and other components. This is achieved by encapsulating functionality normally contained within
`MonoBehaviour` components into `ScriptableObjects` instead. By doing this, we automatically already
remove any scene dependencies. But `ScriptableObjects` can do so much more. This SDK is an attempt to
realize that potential.

# Pros

1. Completely scene and prefab independent referencing of data and events. Player data, game manager states, pretty much anything can be shared across scenes and prefabs with this SDK.
2. All data is serialized natively by Unity and can easily be exported to JSON. Data can also be imported back into `ScriptableObjects` from JSON. This can be very handy for adding support for custom player levels, maps, or even just saving player data quick and dirty like.
3. Entire games can be made purely by using the SDK as is. Unlike visual scripting tools that aim to completely migrate the game development flow into a visual scripting interface, SOFlow attempts to blend with Unity as is.
4. A fully debuggable `Game Event` system. Since a lot of functionality is now directly linked to data assets and `Game Objects`, it can become difficult to debug when things go wrong. I have made my best efforts to alleviate this with bug tracking for `Game Events` right inside Unity. And at times when we need to get down and dirty with code, hyperlinks to the code in question are available as well.
5. Customize the look and feel of most inspectors. All components that do not have a dedicated `Custom Editor` will automatically use SOFlow to display their contents.
6. A wide variety of useful components always coming to the SDK. The SDK is ever-evolving, but each new features will maintain the same general workflow encouraged by SOFlow.

# Cons

What are '_cons_'?

# Core Features

- **Data Types**

One of the foundations of the SDK. Other components in one form or another build upon what is known as
`Primitive Data Types`. Theoretically, one could create an infinite number of `Data Types` supported by
the SDK. The SDK itself provides the most commonly used `Data Types` ready to be used within projects
as illustrated below:

<p align="center"><img src="https://i.imgur.com/iLdAI2A.gif"></p>

**So what are `Data Types` exactly?**

They are containers for different types of data. The data they store is normally data stored exclusively 
within components used within scenes. This exclusivity makes it cumbersome, and at times difficult, to
interact or share the data with other components. Storing this data within containers instead allows for
far greater flexibility and provides immediate independence from scenes.

The containers come in the form of files stored within the project (`ScriptableObjects`). These containers
can then be used by other components in a variety of ways. One example is the `Data Text Setter` component,
which takes any arbitrary `Primitive Data Type` and displays the current data within the container in a
`TextMesh Pro Text` component:

<p align="center"><img src="https://i.imgur.com/9JbJqnk.gif"></p>

It is sometimes desirable to keep exclusivity of data on a per scene/component basis. `Primitive Data Types`
can be used for both cases. Each data type comes with a native ability to toggle between using data
containers or using exclusive values:

<p align="center"><img src="https://i.imgur.com/wMo2J0c.gif"></p>

NOTE: Using the `Primitive Data` root class as a variable does not support toggling between data containers
and exclusive values, as there would be no data defined as exclusive values.

- **Game Events And Game Event Listeners**

Another one of the foundations of the SDK. Not to be confused with `UnityEvents` or `UltEvents`. `Game
Events` act as messengers between components. That is essentially their only purpose. Though as dull as that
may sound, it is actually their most powerful (if only) feature. `Game Events` work together with
`Game Event Listeners`, who act as receivers of the messages carried by `Game Events`. A short breakdown of
what this means for us:

1. We choose when to send out a message.
2. `Game Events` ensure our message goes out to all interested components.
3. `Game Event Listeners` receive the message and lets us know the message has arrived.
4. We decide what to do after having received said message.

To demonstrate this concept, let's use the previous example with setting text to some data. Previously,
we had to enter Play Mode for the text to update. This time, we can simply send out a message to say when
the text should be updated: 

<p align="center"><img src="https://i.imgur.com/6Z8WeFs.gif"></p>

`Game Events` can also be dragged directly into a scene and will automatically create an associated
`Game Event Listener`: 

<p align="center"><img src="https://i.imgur.com/2fmIFG7.gif"></p>

Messages can be sent directly through code by exposing a `GameEvent` field, then calling `MyGameEvent.Raise()`.
My preferred method, and one the SDK encourages, is to send messages using `UltEvents` (or `UnityEvents` if
`UltEvents` are not available).

- **[UltEvents](https://kybernetikgames.github.io/ultevents/)**

`UltEvents` are a wonder on their own. Think of them as severely overpowered `UnityEvents`. If you are
unfamiliar with either `UnityEvents` or `UltEvents`, I highly suggest [checking them out.](https://www.youtube.com/watch?v=pjWqsFDozSo)
`UltEvents` can serve an large number of purposes, and for the SDK, they provide the perfect interface
to create interactions between `Data Types` and `Game Events`.

In the example below, a `Comparison` component uses `UltEvents`, one for if what is being compared for
matches, and another if the comparison does not match. In our case, when the comparison matches, we
send off our example message:

<p align="center"><img src="https://i.imgur.com/cdM4aFV.gif"></p>

- **Game Event Log**

Even in the safest of project environments, things can and eventually will go wrong. When they do go
wrong, it's always great to have a tool or two handy to figure out what went wrong. That is the main
goal of the `Game Event Log`. The `Game Event Log` keeps track of all `Game Events` that send messages
during a session. It will color code `Game Events` based on what happened when they sent their messages.
The coloring will depend on your individual color settings for the SDK. For me, green means everything
went smoothly, and red means an error occurred with that `Game Event`.

The `Game Event Log` itself does not track exactly what went wrong, instead the `Game Events` themselves
keep their own logs of what happened during each message they have sent. If an error occurred when a
message was sent, we can check the logs within the `Game Event` to see exactly what the error was, and
if necessary, navigate straight to the line of code that caused the error.

Below is an example of both the `Game Event Log` and `Game Event` error logging in action:

<p align="center"><img src="https://i.imgur.com/2tNlx7C.gif"></p>

# Additional Features

Each feature listed here will have example scenes and assets explaining and demonstrating how they work
within Unity itself. These examples can be found under : Assets/Code/Scripts/SOFlow/Examples

- `Audio Source` sharing through `Scriptable Objects`.
- `Camera` sharing through `Scriptable Objects`.
- Screen resolution tracking.
- Fading and data lerping using `Fadables` and `Faders`.
- `Managed Components`, a more controlled form of updating components vs directly using `Update()`.
- Simple motion of objects using the `Basic Motion` component.
